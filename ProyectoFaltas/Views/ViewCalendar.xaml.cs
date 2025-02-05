using Plugin.Maui.Calendar.Models;
using ProyectoFaltas.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ProyectoFaltas.Database;

namespace ProyectoFaltas.Views
{
    public partial class ViewCalendar : ContentPage, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private DatabaseService database = new DatabaseService();

        private ObservableCollection<Falta> _listaFaltas;
        public ObservableCollection<Falta> ListaFaltas
        {
            get => _listaFaltas;
            set
            {
                _listaFaltas = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Profesor> _listaProfesores;
        public ObservableCollection<Profesor> ListaProfesores
        {
            get => _listaProfesores;
            set
            {
                _listaProfesores = value;
                OnPropertyChanged();
            }
        }

        private Profesor _profesorSeleccionado;
        public Profesor ProfesorSeleccionado
        {
            get => _profesorSeleccionado;
            set
            {
                _profesorSeleccionado = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TipoFalta> _listaTipoFaltas;
        public ObservableCollection<TipoFalta> ListaTipoFaltas
        {
            get => _listaTipoFaltas;
            set
            {
                _listaTipoFaltas = value;
                OnPropertyChanged();
            }
        }

        private TipoFalta _tipoFaltaSeleccionado;
        public TipoFalta TipoFaltaSeleccionado
        {
            get => _tipoFaltaSeleccionado;
            set
            {
                _tipoFaltaSeleccionado = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDay;
        public DateTime SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                recargarLista();
                OnPropertyChanged();
            }
        }

        public ViewCalendar()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ListaTipoFaltas = new ObservableCollection<TipoFalta>(await database.GetTipoFaltasAsync());
            ListaProfesores = new ObservableCollection<Profesor>(await database.GetProfesoresAsync());
            recargarLista();
        }

        //---------------------------------------------------------- recargarLista ----------------------------------------------------------
        private async void recargarLista()
        {
            if (Curso.CursoActual != null)
            {
                ListaFaltas = new ObservableCollection<Falta>(await database.GetFaltasDatosPorDiaAsync(SelectedDay));

                BindingContext = this; // Establecer el BindingContext después de cargar los datos.
            }
        }

        private async void AddFalta_Clicked(object sender, EventArgs e)
        {
            var falta = new Falta();
            falta.Fecha = SelectedDay;
            falta.IdCursos = Curso.CursoActual.Id;
            falta.IdProfesores = ProfesorSeleccionado.Id;
            falta.IdTipoFalta = TipoFaltaSeleccionado.Id;
            falta.UltimaModificacion = DateTime.Now;
            await database.AddFaltaAsync(falta);
            recargarLista();
        }
    }

}