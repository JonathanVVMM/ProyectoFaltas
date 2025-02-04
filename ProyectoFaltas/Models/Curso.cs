using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class Curso
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreCurso { get; set; }
    }
}
