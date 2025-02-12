using Plugin.Maui.Calendar.Models;
using ProyectoFaltas.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ProyectoFaltas.Database;
using ProyectoFaltas.Metodos;
using System.Globalization;

namespace ProyectoFaltas.Views
{
    public partial class ViewCalendar : ContentPage, INotifyPropertyChanged
    {
        public CultureInfo Culture => new CultureInfo("es-ES");

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

        private DateTime _selectedDay = DateTime.Now;
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


        public string stringSelectedDay => SelectedDay.ToString("d");


        public ViewCalendar()
        {
            InitializeComponent();
            FaltasProfesorCommand = new Command<int>(FaltasProfesor);
        }

        //---------------------------------------------------------- Refrescar listas ----------------------------------------------------------
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ListaTipoFaltas = new ObservableCollection<TipoFalta>(await database.GetTipoFaltasAsync());
            ListaProfesores = new ObservableCollection<Profesor>(await database.GetProfesoresActivosPorCursoAsync());
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

        //---------------------------------------------------------- AddFalta_Clicked ----------------------------------------------------------
        private async void AddFalta_Clicked(object sender, EventArgs e)
        {

            if (await database.ExisteFalta(ProfesorSeleccionado.Id, SelectedDay))
            {
                await DisplayAlert("FALTA EXISTENTE", "Esta falta ya existe", "Entendido");
            }
            else
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

        //---------------------------------------------------------- FaltasProfesorCommand ----------------------------------------------------------
        public ICommand FaltasProfesorCommand { get; set; }

        public async void FaltasProfesor(int ItemId)
        {

            if (await App.Current.MainPage.DisplayAlert("Faltas del profesor", "Si acepta será cambiado de página a la lista de faltas de este profesor", "Confirmar", "Cancelar"))
            {
                await Shell.Current.GoToAsync($"//ViewTeacherNonAttendances?IdProfesor={ItemId}");
            }
        }

        //METODOS PARA LOS BOTONES

        public async void IconoAyudaFaltasDia(object sender, EventArgs e)
        {
            await DisplayAlert("Mensaje de ayuda", "Aquí se muestran las faltas de los profesores de ese día, es decir, hasta que no insertes una falta no mostrará nada", "Entendido");
        }

        public async void IconoAyudaAdjuntarFaltas(object sender, EventArgs e)
        {
            await DisplayAlert("Mensaje de ayuda", "Este menú sirve para agregar faltas al día elegido en el calendario, consta de: \n\n - Un desplegable para elegir el profesor (debes tenerlo creado) \n - Un desplegable para elegir el tipo de falta (debes tenerla creada)", "Entendido");
        }


        //---------------------------------------------------------- GoHomePage_Clicked ----------------------------------------------------------
        private async void GoHomePage_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//HomePage");
        }
    }

}