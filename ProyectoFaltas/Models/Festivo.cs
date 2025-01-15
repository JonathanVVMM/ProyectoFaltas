using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class Festivo
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Tipo { get; set; } // VARCHAR(10)

        [Indexed]
        public long IdCursos { get; set; }
    }
}
