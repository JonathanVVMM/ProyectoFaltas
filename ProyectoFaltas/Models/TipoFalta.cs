using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class TipoFalta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Tipo { get; set; } // VARCHAR(100)

        public string Color { get; set; } // VARCHAR(30)

    }
}
