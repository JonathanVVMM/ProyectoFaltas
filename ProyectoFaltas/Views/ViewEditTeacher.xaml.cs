using ProyectoFaltas.ViewModels;

namespace ProyectoFaltas.Views;

public partial class ViewEditTeacher : ContentPage
{
    public ViewEditTeacher()
    {
        InitializeComponent();
        BindingContext = VMConfigureTeachers.Instance; // RECOJO LA INSTANCIA DEL VIEWMODEL, SINO LA GESTIONO A MANO SE CREAN 2 Y FUNCIONA EL BINDING

    }
}