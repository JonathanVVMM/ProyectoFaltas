using ProyectoFaltas.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProyectoFaltas.Database
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection Database;

        public DatabaseService() { }

        private async Task Init() //Metodo para crear la conexion con la base de datos, TODOS LOS METODOS QUE HAGAN ALGO SOBRE LA BASE DE DATOS DEBEN DE ESPERAR A ESTE
        {
            if (Database is not null) return;
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags); //Si ha llegado hasta aqui, es porque no está conectado a la base de datos y la crea o se conecta a ella
            await Database.CreateTableAsync<Activo>();
            await Database.CreateTableAsync<Curso>();
            await Database.CreateTableAsync<Falta>();
            await Database.CreateTableAsync<TipoFalta>();
            await Database.CreateTableAsync<Festivo>();
            await Database.CreateTableAsync<Profesor>();
        }

        // -------------------------- TABLA ACTIVO --------------------------
        public async Task<int> AddActivoAsync(Activo activo)
        {
            await Init();
            if (activo.Id != 0)
                return await Database.UpdateAsync(activo);
            else
                return await Database.InsertAsync(activo);
        }
        public async Task<List<Activo>> GetActivosAsync()
        {
            await Init();
            return await Database.Table<Activo>().ToListAsync();
        }
        public async Task<Activo> GetActivoAsync(int id)
        {
            await Init();
            return await Database.Table<Activo>().Where(i => i.Id == id).FirstOrDefaultAsync(); //Similar a SQL
        }

        // Permite borrar a un profesor activo en un curso, PERO el profesor asi quede inactivo en ese curso no será eliminado de la tabla
        public async Task<int> DeleteActivoAsync(int idProfesor, int idCurso)
        {
            await Init();

            var activo = await Database.Table<Activo>()
                              .Where(a => a.IdCursos == idCurso && a.IdProfesores == idProfesor)
                              .FirstOrDefaultAsync();

            if (activo != null)
            {
                // Si encontramos el activo, lo eliminamos
                await Database.DeleteAsync(activo);
            }

            return await Database.DeleteAsync(activo);
        }

        // Permite borrar todos los activos de un profesor
        public async Task DeleteActivosAsync(int idProfesor)
        {
            await Init();

            // Elimina todos los registros de la tabla Activo donde el ProfesorId coincida
            var registrosActivos = await Database.Table<Activo>()
                                                 .Where(a => a.IdProfesores == idProfesor)
                                                 .ToListAsync();

            foreach (var activo in registrosActivos)
            {
                await Database.DeleteAsync(activo);
            }
        }

        //Obetener el ultimo curso
        public async Task<int?> GetUltimoAnoCursoAsync()
        {
            await Init();
            var cursoMasReciente = await Database.Table<Curso>().OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            return cursoMasReciente != null ? (int?)cursoMasReciente.Id : null;
        }

        // Obtener los profesores activos de un curso en especifico
        public async Task<List<Profesor>> GetProfesoresActivosPorCursoAsync()
        {
            await Init();
            var activos = await Database.Table<Activo>().Where(a => a.IdCursos == Curso.CursoActual.Id).ToListAsync();
            var profesoresIds = activos.Select(a => a.IdProfesores).ToList();

            return await Database.Table<Profesor>().Where(p => profesoresIds.Contains(p.Id)).ToListAsync();
        }



        // -------------------------- TABLA CURSO -------------------------- 

        // Crea un curso
        public async Task<int> AddCursoAsync(Curso curso)
        {
            await Init();
            if (curso.Id != 0)
                return await Database.UpdateAsync(curso);
            else
                return await Database.InsertAsync(curso);
        }

        // Lista los cursos existentes
        public async Task<List<Curso>> GetCursosAsync()
        {
            await Init();
            return await Database.Table<Curso>().ToListAsync();
        }

        // Obtiene el curso seleccionado
        public async Task<Curso> GetCursoAsync(int id)
        {
            await Init();
            return await Database.Table<Curso>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteCursoAsync(Curso curso)
        {
            await Init();
            return await Database.DeleteAsync(curso);
        }

        public async Task<bool> ExisteCurso(string nombreCurso)
        {
            return await Database.Table<Curso>().Where(c => c.NombreCurso == nombreCurso).FirstOrDefaultAsync().ContinueWith(t => t.Result != null);
        }

        // -------------------------- TABLA FALTA -------------------------- 
        public async Task<int> AddFaltaAsync(Falta falta)
        {
            await Init();
            if (falta.Id != 0)
                return await Database.UpdateAsync(falta);
            else
                return await Database.InsertAsync(falta);
        }

        // Devuelve las faltas generadas en un curso
        public async Task<List<Falta>> GetFaltasAsync(int idCurso)
        {
            await Init();
            return await Database.Table<Falta>().Where(i => i.IdCursos == idCurso).ToListAsync();
        }

        // Devuelve las faltas de un dia en especifico
        public async Task<List<Falta>> GetFaltasPorDiaAsync(DateTime fecha)
        {
            await Init();
            return await Database.Table<Falta>().Where(i => i.Fecha == fecha && i.IdCursos == Curso.CursoActual.Id).ToListAsync();
        }

        public async Task<Falta> GetFaltaAsync(int id)
        {
            await Init();
            Falta faltaProvisional = await Database.Table<Falta>().Where(i => i.Id == id).FirstOrDefaultAsync();
            TipoFalta tipoFaltaProvisional = await Database.Table<TipoFalta>().Where(i => i.Id == faltaProvisional.IdTipoFalta).FirstOrDefaultAsync();

            Falta nuevaFalta = new Falta();
            nuevaFalta.Fecha = faltaProvisional.Fecha;
            nuevaFalta.Id = faltaProvisional.Id;
            nuevaFalta.IdProfesores = faltaProvisional.IdProfesores;
            nuevaFalta.IdTipoFalta = faltaProvisional.IdTipoFalta;
            nuevaFalta.UltimaModificacion = faltaProvisional.UltimaModificacion;
            nuevaFalta.IdCursos = faltaProvisional.IdCursos;
            nuevaFalta.nombreTipoFalta = tipoFaltaProvisional.Tipo;
            nuevaFalta.Color = tipoFaltaProvisional.Color;

            return nuevaFalta;
        }


        public async Task<List<Falta>> GetFaltasProfesorAsync(int idProfesor)
        {
            await Init();

            var listaFaltas = new List<Falta>();

            var profesor = await Database.Table<Profesor>().Where(i => i.Id == idProfesor).FirstOrDefaultAsync();

            var faltas = await Database.Table<Falta>().Where(i => i.IdProfesores == idProfesor && i.IdCursos == Curso.CursoActual.Id).ToListAsync();



            foreach (Falta item in faltas)
            {
                var tipoFalta = await Database.Table<TipoFalta>().Where(i => i.Id == item.IdTipoFalta).FirstOrDefaultAsync();

                item.profesorNombreApellido = profesor != null ? profesor.Nombre + " " + profesor.Apellidos : "";
                item.nombreTipoFalta = tipoFalta != null ? tipoFalta.Tipo : "";
                item.Color = tipoFalta.Color;
                listaFaltas.Add(item);
            }

            return listaFaltas;
        }

        public async Task<int> DeleteFaltaAsync(Falta falta)
        {
            await Init();
            return await Database.DeleteAsync(falta);
        }

        // Devuelve las faltas de un dia en especifico agregando los datos de nombre y apellido de Profesor y y nombre de tipoFalta

        public async Task<List<Falta>> GetFaltasDatosPorDiaAsync(DateTime fecha)
        {
            await Init();

            //Obtiene las faltas por fecha
            var faltas = await Database.Table<Falta>().Where(i => i.Fecha == fecha && i.IdCursos == Curso.CursoActual.Id).ToListAsync();

            var listaFaltas = new List<Falta>();

            foreach (var falta in faltas)
            {
                // Obtener el profesor asociado a esta falta
                var profesor = await Database.Table<Profesor>().Where(p => p.Id == falta.IdProfesores).FirstOrDefaultAsync();

                // Obtener el tipo de falta asociado a esta falta
                var tipoFalta = await Database.Table<TipoFalta>().Where(tf => tf.Id == falta.IdTipoFalta).FirstOrDefaultAsync();

                // Crear un nuevo objeto Falta con los datos completos
                listaFaltas.Add(new Falta
                {
                    Id = falta.Id,
                    IdProfesores = falta.IdProfesores,
                    IdTipoFalta = falta.IdTipoFalta,
                    Fecha = falta.Fecha,
                    UltimaModificacion = falta.UltimaModificacion,
                    profesorNombreApellido = profesor != null ? profesor.Nombre + " " + profesor.Apellidos : "",
                    nombreTipoFalta = tipoFalta != null ? tipoFalta.Tipo : "",
                    Color = tipoFalta.Color,

                });
            }

            return listaFaltas;
        }

        public async Task<bool> ExisteFalta(int idProfesor, DateTime fecha)
        {

            Falta falta = await Database.Table<Falta>().Where(p => p.IdProfesores == idProfesor && p.Fecha == fecha && p.IdCursos == Curso.CursoActual.Id).FirstOrDefaultAsync();

            if (falta != null) return true;
            else return false;
        }




        // -------------------------- TABLA TIPOFALTA -------------------------- 
        public async Task<int> SaveTipoFaltaAsync(TipoFalta tipoFalta)
        {
            await Init();
            if (tipoFalta.Id != 0)
                return await Database.UpdateAsync(tipoFalta);
            else
                return await Database.InsertAsync(tipoFalta);
        }

        public async Task<List<TipoFalta>> GetTipoFaltasAsync()
        {
            await Init();
            return await Database.Table<TipoFalta>().ToListAsync();
        }

        public async Task<TipoFalta> GetTipoFaltaAsync(int id)
        {
            await Init();
            return await Database.Table<TipoFalta>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteTipoFaltaAsync(TipoFalta tipoFalta)
        {
            await Init();
            return await Database.DeleteAsync(tipoFalta);
        }

        // -------------------------- TABLA FESTIVO --------------------------
        public async Task<int> AddFestivoAsync(Festivo festivo)
        {
            await Init();
            return await Database.InsertAsync(festivo);
        }

        public async Task<List<Festivo>> GetFestivosAsync()
        {
            await Init();
            return await Database.Table<Festivo>().ToListAsync();
        }

        public async Task<Festivo> GetFestivoAsync(int id)
        {
            await Init();
            return await Database.Table<Festivo>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteFestivoAsync(Festivo festivo)
        {
            await Init();
            return await Database.DeleteAsync(festivo);
        }

        // -------------------------- TABLA PROFESOR --------------------------

        // Se registra un profesor nuevo
        public async Task<int> SaveProfesorAsync(Profesor profesor)
        {
            await Init();
            if (profesor.Id != 0)
            {
                return await Database.UpdateAsync(profesor);
            }
            else
            {
                return await Database.InsertAsync(profesor);
            }

        }

        public async Task<List<Profesor>> GetProfesoresAsync()
        {
            await Init();
            return await Database.Table<Profesor>().ToListAsync();
        }

        public async Task<Profesor> GetProfesorAsync(int id)
        {
            await Init();
            return await Database.Table<Profesor>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteProfesorAsync(Profesor profesor)
        {
            await Init();
            return await Database.DeleteAsync(profesor);
        }

        public async Task<List<Profesor>> GetProfesoresByFaltaAsync(int faltaId)
        {
            await Init();

            // Obtener los activos asociados a la falta
            var activos = await Database.Table<Activo>().Where(a => a.Id == faltaId).ToListAsync();

            // Obtener los IDs de los profesores asociados a la falta
            var profesoresIds = activos.Select(a => a.IdProfesores).ToList();

            // Obtener los profesores que corresponden a esos IDs
            return await Database.Table<Profesor>().Where(p => profesoresIds.Contains(p.Id)).ToListAsync();
        }



    }
}
