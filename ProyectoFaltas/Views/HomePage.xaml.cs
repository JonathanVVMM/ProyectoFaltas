using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFaltas.Views;

public partial class HomePage : ContentPage, INotifyPropertyChanged
{
    private DatabaseService database = new DatabaseService();

    public HomePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        cargarCursos();
    }


    public bool CursoSeleccionado => CursoBindeado != null;
    public Curso CursoBindeado
    {
        get => Curso.CursoActual;
        set
        {
            if (Curso.CursoActual != value)
            {
                Curso.CursoActual = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CursoSeleccionado));
            }
        }
    }

    public async void cargarCursos()
    {
        ListaCursos = new ObservableCollection<Curso>(await database.GetCursosAsync());
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ObservableCollection<Curso> _ListaCursos;
    public ObservableCollection<Curso> ListaCursos
    {
        get => _ListaCursos;
        set
        {
            _ListaCursos = value; OnPropertyChanged();
        }
    }

    private async void Crear_Curso_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("viewCreateYear");
    }

    private async void Calendar_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewCalendar");
    }

    private async void CreateTeacher_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewCreateTeacher");
    }

    private async void EditTeacher_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewEditTeacher");
    }

    private async void EditFaltas_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewTiposFaltas");
    }

    private async void Ayuda_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Información de ayuda",
                            "Bienvenido a la página principal:\n\n" +
                             "- Puedes seleccionar un curso desde el selector desplegable y utilizar las opciones disponibles.\n" +
                             "- Tienes accesos directos para gestionar el calendario, crear profesores, editar profesores y editar tipos de faltas. Estos botones estarán habilitados únicamente si seleccionas un curso.\n" +
                             "- Usa el botón 'Crear curso' si necesitas agregar un nuevo curso a la lista.\n\n" +
                             "Recuerda que cada sección tiene un propósito específico para organizar mejor la información y facilitar la navegación.",
                             "Entendido");

    }
}