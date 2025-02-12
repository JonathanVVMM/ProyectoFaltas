using ProyectoFaltas.ViewModels;

namespace ProyectoFaltas.Views;

public partial class ViewCreateTeacher : ContentPage
{

    public ViewCreateTeacher()
    {
        InitializeComponent();
        BindingContext = instancia; // RECOJO LA INSTANCIA DEL VIEWMODEL, SINO LA GESTIONO A MANO SE CREAN 2 Y FUNCIONA EL BINDING

    }
    VMConfigureTeachers instancia = VMConfigureTeachers.Instance;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        instancia.recargarEstadoProfesores();
    }
}