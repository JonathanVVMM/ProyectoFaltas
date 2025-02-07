
using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

    private TipoFalta _tipoFaltaNuevo;
    public TipoFalta TipoFaltaNuevo
    {
        get => _tipoFaltaNuevo;
        set
        {
            _tipoFaltaNuevo = value;
            OnPropertyChanged();
        }
    }

    private bool _modificandoFalta = false;
    public bool ModificandoFalta
    {
        get => _modificandoFalta;
        set
        {
            _modificandoFalta = value;
            OnPropertyChanged();
        }
    }


    public ViewTeacherNonAttendances()
    {
        InitializeComponent();
        ModificarFaltaCommand = new Command<int>(ModificarFalta);
        BindingContext = this;
    }

    public async void recargarDatos()
    {
        ProfesorMostrado = await database.GetProfesorAsync(IdProfesor);
        List<Falta> faltas = await database.GetFaltasProfesorAsync(IdProfesor);

        int filtro;
        if (int.TryParse(eFiltro.Text, out filtro) && filtro > 0 && filtro < 13)
            faltas = faltas.Where(f => f.Fecha.Month == filtro).ToList();

        ListaFaltas = new ObservableCollection<Falta>(faltas);
        ListaTipoFaltas = new ObservableCollection<TipoFalta>(await database.GetTipoFaltasAsync());
    }

    private async void Volver_Calendario_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewCalendar");
    }

    private async void Editar_Profe_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewEditTeacher");
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        recargarDatos();
    }

    private Falta _faltaModificando;
    public Falta FaltaModificando
    {
        get => _faltaModificando;
        set
        {
            _faltaModificando = value;
            OnPropertyChanged();

        }
    }
    public ICommand ModificarFaltaCommand { get; set; }

    public async void ModificarFalta(int idFalta)
    {
        if (await App.Current.MainPage.DisplayAlert("Actualizar Falta", "¿ Está seguro de actualizaciar la falta del profesor?", "Confirmar", "Cancelar"))
        {
            FaltaModificando = await database.GetFaltaAsync(idFalta);
            ModificandoFalta = true;
        }
    }

    public void CancerlarModificarFalta(object sender, EventArgs e)
    {
        FaltaModificando = null;
        ModificandoFalta = false;
    }

    public async void GuardarModificarFalta(object sender, EventArgs e)
    {


        if (TipoFaltaNuevo != null)
        {
            FaltaModificando.IdTipoFalta = TipoFaltaNuevo.Id;
        }
        await database.AddFaltaAsync(FaltaModificando);
        TipoFaltaNuevo = null;
        FaltaModificando = null;
        ModificandoFalta = false;
        recargarDatos();
    }


}