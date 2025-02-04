using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProyectoFaltas.Models;
using ProyectoFaltas.Database;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics;

namespace ProyectoFaltas.ViewModels
{
    public class VMTiposFaltas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<TipoFalta> _TipoFaltaList = new ObservableCollection<TipoFalta>();

        public ObservableCollection<TipoFalta> MisTiposFalta
        {
            get { return _TipoFaltaList; }
            set
            {
                _TipoFaltaList = value;
                OnPropertyChanged();
            }
        }

        public DatabaseService TipoFaltaDB = new DatabaseService();


        //---------------------------------------------------------- ICommand ----------------------------------------------------------
        public ICommand AddElementCommand { get; set; }
        public ICommand RemoveElementCommand { get; set; }


        //---------------------------------------------------------- METODO PRINCIPAL ----------------------------------------------------------
        public VMTiposFaltas()
        {
            RecuperarTiposFaltas();
            AddElementCommand = new Command(AddElement);

        }

        //---------------------------------------------------------- INTRODUCIDOS USUARIO ----------------------------------------------------------
        // Tipo
        private string _TipoIntro = "";
        public string TipoIntro
        {
            get { return _TipoIntro; }
            set
            {
                _TipoIntro = value;
                OnPropertyChanged();
            }
        }

        // Color
        private string _selectedColor;
        public string SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }


        //---------------------------------------------------------- CONFIGURACIÓN COLOR PICKER A MANO ----------------------------------------------------------
        //Clase para crear el objeto color
        public class ColorOption
        {
            public string Name { get; set; }
            public Color ColorValue { get; set; }
        }

        //Crear la lista de colores:
        private List<ColorOption> _colorOptions = new List<ColorOption>
        {
            //Los que ya tenían
            new ColorOption { Name = "Verde", ColorValue = Colors.Green },
            new ColorOption { Name = "Amarillo", ColorValue = Colors.Yellow },
            new ColorOption { Name = "Rojo", ColorValue = Colors.Red },
            new ColorOption { Name = "Azul", ColorValue = Colors.Blue },
            new ColorOption { Name = "Gris", ColorValue = Colors.Gray },

            //Más opciones
            new ColorOption { Name = "Naranja", ColorValue = Colors.Orange },
            new ColorOption { Name = "Morado", ColorValue = Colors.Purple },
            new ColorOption { Name = "Rosa", ColorValue = Colors.Pink },
            new ColorOption { Name = "Marron", ColorValue = Colors.Brown }
        };

        //metodo para usar los colores:
        public List<ColorOption> ColorOptions
        {
            get { return _colorOptions; }
            set
            {
                _colorOptions = value;
                OnPropertyChanged();
            }
        }


        //---------------------------------------------------------- AddElement ----------------------------------------------------------
        public async void AddElement()
        {
            // Validaciones por precaución y evitar nulos
            if (!string.IsNullOrEmpty(_TipoIntro) && !string.IsNullOrEmpty(_selectedColor))
            {
                TipoFalta item = new TipoFalta
                {
                    Tipo = _TipoIntro,
                    Color = _selectedColor,
                };

                // Limpiar campos
                TipoIntro = "";
                SelectedColor = "";

                // Insertar en la base de datos
                await TipoFaltaDB.AddTipoFaltaAsync(item);

                // Recuperar lista actualizada
                RecuperarTiposFaltas();
            }
        }

        //---------------------------------------------------------- RemoveElement ----------------------------------------------------------


        //---------------------------------------------------------- RecuperarTiposFaltas ----------------------------------------------------------
        private async void RecuperarTiposFaltas()
        {
            MisTiposFalta = new ObservableCollection<TipoFalta>(await TipoFaltaDB.GetTipoFaltasAsync());
        }
    }
}
