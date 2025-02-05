using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public string profesorNombreApellido { get; set; }

        [Indexed]
        public int IdTipoFalta { get; set; }

        [NotMapped]
        public string nombreTipoFalta { get; set; }

        public DateTime UltimaModificacion { get; set; }

        [Indexed]
        public int IdCursos { get; set; }
    }
}
