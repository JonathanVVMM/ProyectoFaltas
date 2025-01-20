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

        private string _nombre;
        private string _apellidos;
        private string _tipo;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value; OnPropertyChanged();
            }
        }

        public string Apellidos
        {
            get => _apellidos;
            set
            {
                _apellidos = value; OnPropertyChanged();
            }
        }

        public string Tipo
        {
            get => _tipo;
            set
            {
                _tipo = value; OnPropertyChanged();
            }
        }
    }
}
