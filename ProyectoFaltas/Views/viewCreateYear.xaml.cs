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
    }

    public async Task SeleccionarCursoAsync(int? cursoId = null)
    {
        if (!cursoId.HasValue)
        {
            cursoId = await _databaseService.GetUltimoAnoCursoAsync();
            if (!cursoId.HasValue)
                return;
        }

        var profesores = await _databaseService.GetProfesoresActivosPorCursoAsync(cursoId.Value);
        Profesores = new ObservableCollection<Profesor>(profesores.Distinct());
        OnPropertyChanged(nameof(Profesores));
    }

    private async Task CargarProfesoresDelUltimoAnoAsync()
    {
        await SeleccionarCursoAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var nuevoCurso = new Curso { NombreCurso = "Nuevo Curso" };
        await _databaseService.AddCursoAsync(nuevoCurso);
        await SeleccionarCursoAsync(nuevoCurso.Id);
    }
}
