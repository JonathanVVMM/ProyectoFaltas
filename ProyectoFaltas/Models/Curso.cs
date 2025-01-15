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
        public long Id { get; set; }
    }
}
