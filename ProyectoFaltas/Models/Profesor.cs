using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class Profesor
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public string Nombre { get; set; } // VARCHAR(50)

        public string Apellidos { get; set; } // VARCHAR(100)

        public string Tipo { get; set; } // ENUM(...) (puedes ajustarlo según tus necesidades)
    }
}
