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
        public long Id { get; set; }

        public DateTime Fecha { get; set; }

        [Indexed]
        public long IdProfesores { get; set; }

        [Indexed]
        public long IdTipoFalta { get; set; }

        public DateTime UltimaModificacion { get; set; }

        [Indexed]
        public long IdCursos { get; set; }
    }
}
