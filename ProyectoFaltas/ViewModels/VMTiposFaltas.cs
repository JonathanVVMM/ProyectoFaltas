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
using System.Globalization;

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
        public ICommand ModifyTipoFaltaCommand { get; set; }
        public ICommand ActualizarTipoFaltaCommand { get; set; }
        public ICommand CancelarActualizarTipoFaltaCommand { get; set; }


        //---------------------------------------------------------- METODO PRINCIPAL ----------------------------------------------------------
        public VMTiposFaltas()
        {
            RecuperarTiposFaltas();
            AddElementCommand = new Command(AddElement);
            ModifyTipoFaltaCommand = new Command<int>(ModifyTipoFalta);
            ActualizarTipoFaltaCommand = new Command(ActualizarTipoFalta);
            IconTipoFaltaCommand = new Command(IconTipoFalta);
            IconCrearTipoFaltaCommand = new Command(IconCrearTipoFalta);
            IconActualizarTipoFaltaCommand = new Command(IconActualizarTipoFalta);
            CancelarActualizarTipoFaltaCommand = new Command(CancelarActualizarTipoFalta);
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
        private ColorOption _selectedColor;
        public ColorOption SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        //---------------------------------------------------------- EDITADOS USUARIO ----------------------------------------------------------
        // Tipo
        private string _TipoNuevo = "";
        public string TipoNuevo
        {
            get { return _TipoNuevo; }
            set
            {
                _TipoNuevo = value;
                OnPropertyChanged();
            }
        }

        // Color
        private ColorOption _selectedColorNuevo;
        public ColorOption SelectedColorNuevo
        {
            get { return _selectedColorNuevo; }
            set
            {
                _selectedColorNuevo = value;
                OnPropertyChanged();
            }
        }

        //---------------------------------------------------------- PARA LOS METODOS ----------------------------------------------------------
        private TipoFalta _TipoFaltaEditando;
        public TipoFalta TipoFaltaEditando
        {
            get { return _TipoFaltaEditando; }
            set { _TipoFaltaEditando = value; OnPropertyChanged(); }
        }

        private bool _editando = false;
        public bool Editando
        {
            get { return _editando; }
            set
            {
                _editando = value;
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
        private ObservableCollection<ColorOption> _colorOptions = new ObservableCollection<ColorOption>
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
        public ObservableCollection<ColorOption> ColorOptions
        {
            get => _colorOptions;
            set
            {
                _colorOptions = value;
                OnPropertyChanged();
            }
        }


        //---------------------------------------------------------- AddElement ----------------------------------------------------------
        public async void AddElement()
        {
            if (!string.IsNullOrEmpty(TipoIntro) && SelectedColor != null)
            {
                TipoFalta item = new TipoFalta
                {
                    Tipo = TipoIntro,
                    Color = SelectedColor.Name, // Usar el nombre del color
                };

                TipoIntro = "";
                SelectedColor = null; // Restablecer los valores después de agregar el nuevo item

                await TipoFaltaDB.SaveTipoFaltaAsync(item);

                RecuperarTiposFaltas();
            }
        }


        //---------------------------------------------------------- ModifyTipoFalta ----------------------------------------------------------
        public async void ModifyTipoFalta(int ItemId)
        {

            TipoFaltaEditando = MisTiposFalta.FirstOrDefault(p => p.Id == ItemId);
            if (TipoFaltaEditando != null)
            {
                TipoNuevo = TipoFaltaEditando.Tipo;
                SelectedColorNuevo = ColorOptions.FirstOrDefault(c => c.Name == TipoFaltaEditando.Color); // Establecer el color del picker
                Editando = true;

            }
        }


        //---------------------------------------------------------- ActualizarTipoFalta ----------------------------------------------------------
        public async void ActualizarTipoFalta()
        {
            if (await App.Current.MainPage.DisplayAlert("Actualizar Tipo falta", "Está seguro de actualizar el tipo de falta seleccionado?", "Confirmar", "Cancelar"))
            {
                if (!String.IsNullOrEmpty(TipoNuevo))
                {
                    TipoFaltaEditando.Tipo = TipoNuevo;
                }

                if (!String.IsNullOrEmpty(TipoNuevo))
                {
                    TipoFaltaEditando.Color = SelectedColorNuevo.Name;
                }

                await TipoFaltaDB.SaveTipoFaltaAsync(TipoFaltaEditando);
                Editando = false;
                TipoFaltaEditando = null;
                TipoNuevo = ""; SelectedColorNuevo = null;
                RecuperarTiposFaltas();
            }
        }

        //---------------------------------------------------------- CancelarActualizarTipoFalta ----------------------------------------------------------
        public async void CancelarActualizarTipoFalta()
        {
            if (await App.Current.MainPage.DisplayAlert("Cancelación de Actualización", "Está seguro de cancelar la actualización del tipo de falta?", "Confirmar", "Cancelar"))
            {
                TipoFaltaEditando = null;
                Editando = false;
                TipoNuevo = ""; SelectedColorNuevo = null;
            }
        }

        //---------------------------------------------------------- RecuperarTiposFaltas ----------------------------------------------------------
        private async void RecuperarTiposFaltas()
        {
            MisTiposFalta = new ObservableCollection<TipoFalta>(await TipoFaltaDB.GetTipoFaltasAsync());
        }

        //---------------------------------------------------------- BOTONES AYUDA ----------------------------------------------------------

        public ICommand IconTipoFaltaCommand { get; set; }
        public ICommand IconCrearTipoFaltaCommand { get; set; }
        public ICommand IconActualizarTipoFaltaCommand { get; set; }

        public async void IconTipoFalta()
        {
            await App.Current.MainPage.DisplayAlert("Información de ayuda", "Este apartado te muestra los tipos de faltas que hay y te permite modificarlas.\nCuando le das al botón de editar te activo el modo edición y te sale un menu abajo a la derecha", "Salir");
        }

        public async void IconCrearTipoFalta()
        {
            await App.Current.MainPage.DisplayAlert("Información de ayuda", "Tiene 2 campo, un campo que escribes a mano el tipo de falta y una lista que te deja elegir de que color quieres que sea la falta", "Salir");
        }

        public async void IconActualizarTipoFalta()
        {
            await App.Current.MainPage.DisplayAlert("Información de ayuda", "Te permite actualizar la falta que has elegido para editar", "Salir");
        }



    }
}