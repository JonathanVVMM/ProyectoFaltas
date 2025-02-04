using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class Falta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        [Indexed]
        public int IdProfesores { get; set; }

        [Indexed]
        public int IdTipoFalta { get; set; }

        public DateTime UltimaModificacion { get; set; }

        [Indexed]
        public int IdCursos { get; set; }
    }
}
