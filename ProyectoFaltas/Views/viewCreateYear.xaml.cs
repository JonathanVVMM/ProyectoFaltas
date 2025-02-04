using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System.Collections.ObjectModel;

namespace ProyectoFaltas.Views;

public partial class viewCreateYear : ContentPage
{
    private readonly DatabaseService _databaseService;
    public ObservableCollection<Profesor> Profesores { get; set; }

    public viewCreateYear()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        Profesores = new ObservableCollection<Profesor>();
        _ = CargarProfesoresDelUltimoAnoAsync();
        BindingContext = this;
    }


    public async Task SeleccionarCursoAsync(int? cursoId = null)
    {
        if (!cursoId.HasValue)
        {
            cursoId = await _databaseService.GetUltimoAnoCursoAsync();
            if (!cursoId.HasValue)
                return;
        }
        Curso.CursoActual = await _databaseService.GetCursoAsync(cursoId.Value);
        var profesores = await _databaseService.GetProfesoresActivosPorCursoAsync(cursoId.Value);
        Profesores = new ObservableCollection<Profesor>(profesores.Distinct());
        OnPropertyChanged(nameof(Profesores));
    }

    private async Task CargarProfesoresDelUltimoAnoAsync()
    {
        await SeleccionarCursoAsync();
    }

    private string _nombre = "";
    public string Nombre
    {
        get => _nombre;
        set
        {
            _nombre = value;
            OnPropertyChanged();
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var nuevoCurso = new Curso { NombreCurso = Nombre };
        await _databaseService.AddCursoAsync(nuevoCurso);
        await SeleccionarCursoAsync(nuevoCurso.Id);
    }
}
