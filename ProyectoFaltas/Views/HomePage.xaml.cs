namespace ProyectoFaltas.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void CreateTeacher_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewCreateTeacher");
    }

    private async void EditTeacher_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ViewEditTeacher");
    }
}