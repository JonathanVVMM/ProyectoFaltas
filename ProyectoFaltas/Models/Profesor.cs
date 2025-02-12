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
    public class Profesor : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; } // Activo / Inactivo   Esto es un dato para ayudar a la inserción

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public string NombreCompleto => $"{Nombre} {Apellidos}";


    }
}
