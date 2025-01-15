using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Database
{
    public class Constants
    {
        // Nombre de la base de datos
        public const string DatabaseFilename = "Faltas.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // Acceso de lectura y escritura a la base de datos
            SQLite.SQLiteOpenFlags.ReadWrite |
            // Crea la base de datos si no existe
            SQLite.SQLiteOpenFlags.Create |
            // Creamos el acceso multihilo a la base de datos
            SQLite.SQLiteOpenFlags.SharedCache;

        // Cadena con la ruta a la base de datos, directorio y nombre del fichero
        public static string DatabasePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DatabaseFilename);
    }
}
