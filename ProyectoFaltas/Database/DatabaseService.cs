using ProyectoFaltas.Models;
using SQLite;
using System;
using System.Collections.Generic;
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
        public async Task<int> DeleteActivoAsync(Activo activo)
        {
            await Init();
            return await Database.DeleteAsync(activo);
        }


        // -------------------------- TABLA CURSO -------------------------- 
        public async Task<int> AddCursoAsync(Curso curso)
        {
            await Init();
            return await Database.InsertAsync(curso);
        }

        public async Task<List<Curso>> GetCursosAsync()
        {
            await Init();
            return await Database.Table<Curso>().ToListAsync();
        }

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


        // -------------------------- TABLA FALTA -------------------------- 
        public async Task<int> AddFaltaAsync(Falta falta)
        {
            await Init();
            return await Database.InsertAsync(falta);
        }

        public async Task<List<Falta>> GetFaltasAsync()
        {
            await Init();
            return await Database.Table<Falta>().ToListAsync();
        }

        public async Task<Falta> GetFaltaAsync(int id)
        {
            await Init();
            return await Database.Table<Falta>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteFaltaAsync(Falta falta)
        {
            await Init();
            return await Database.DeleteAsync(falta);
        }

        // -------------------------- TABLA TIPOFALTA -------------------------- 
        public async Task<int> AddTipoFaltaAsync(TipoFalta tipoFalta)
        {
            await Init();
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
        public async Task<int> AddProfesorAsync(Profesor profesor)
        {
            await Init();
            return await Database.InsertAsync(profesor);
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
    }
}
