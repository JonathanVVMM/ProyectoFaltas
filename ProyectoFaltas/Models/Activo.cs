using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProyectoFaltas.Models
{
    public class Activo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int IdCursos { get; set; }

        [Indexed]
        public int IdProfesores { get; set; }


    }
}
