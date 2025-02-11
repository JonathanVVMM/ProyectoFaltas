using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

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

    public bool ValidarNombreCurso(string curso)
    {

        if (Regex.Match(curso, "2[0-9]{3}/2[0-9]{3}").Success)
        {
            string[] partes = curso.Split("/");
            return int.Parse(partes[0]) == int.Parse(partes[1]) - 1;
        }
        else return false;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (ValidarNombreCurso(Nombre))
        {
            if (await _databaseService.ExisteCurso(Nombre))
            {
                await DisplayAlert("EXISTE CURSO", "Este curso ya existe, no se puede crear", "Entendido");
            }
            else
            {
                if (await App.Current.MainPage.DisplayAlert("Crear Curso", $"¿ Está seguro de crear el curso {Nombre} ?", "Confirmar", "Cancelar"))
                {
                    var nuevoCurso = new Curso { NombreCurso = Nombre };
                    await _databaseService.AddCursoAsync(nuevoCurso);
                    await SeleccionarCursoAsync(nuevoCurso.Id);
                    Nombre = "";
                }
            }
        }
        else
        {
            await DisplayAlert("FORMATO NO VALIDO", "El nombre del curso debe ser 2 años consecutivos separados por \"/\" , ejemplo:\n\n 2024/2025    2027/2028", "Entendido");
        }



    }

    public void IconoHome()
    {
        DisplayAlert("Información", "Esta página es para crear un curso:\n - Si el curso existe no te deja crearlo\n - Si ya existe un curso con ese nombre tampoco", "Entendido");
    }

    private async void Volver_HomePage_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }
}
