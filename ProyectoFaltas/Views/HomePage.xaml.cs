namespace ProyectoFaltas.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
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
}