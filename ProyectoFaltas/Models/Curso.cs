using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class Curso
    {

        private static Curso _cursoActual;

        public static Curso CursoActual
        {
            get => _cursoActual;
            set
            {
                _cursoActual = value;
            }
        }


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreCurso { get; set; }

    }
}
