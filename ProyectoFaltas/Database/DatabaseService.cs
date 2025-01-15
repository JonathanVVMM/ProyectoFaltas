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
            await Database.CreateTableAsync<Festivo>();
            await Database.CreateTableAsync<Profesor>();
            await Database.CreateTableAsync<TipoFalta>();
        }

        //TABLA ACTIVO
        public async Task<int> AddActivoAsync(Activo activo)
        {
            return await Database.InsertAsync(activo);
        }
        public Task
    }
}
