using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFaltas.Views;

[QueryProperty(nameof(IdProfesor), "IdProfesor")]
public partial class ViewTeacherNonAttendances : ContentPage, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private DatabaseService database = new DatabaseService();

    private int _idProfesor;
    public int IdProfesor
    {
        get => _idProfesor;
        set
        {
            _idProfesor = value;
            OnPropertyChanged();
            recargarDatos();
        }
    }

    private Profesor _profesorMostrado;
    public Profesor ProfesorMostrado
    {
        get => _profesorMostrado;
        set
        {
            _profesorMostrado = value;
            OnPropertyChanged();

        }
    }

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

    public ViewTeacherNonAttendances()
    {
        InitializeComponent();

        BindingContext = this;
    }

    public async void recargarDatos()
    {
        ProfesorMostrado = await database.GetProfesorAsync(IdProfesor);
        ListaFaltas = new ObservableCollection<Falta>(await database.GetFaltasProfesorAsync(IdProfesor));

    }
}