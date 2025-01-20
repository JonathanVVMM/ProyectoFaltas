using ProyectoFaltas.Views;

namespace ProyectoFaltas
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ViewCreateTeacher), typeof(ViewCreateTeacher));
            Routing.RegisterRoute(nameof(ViewEditTeacher), typeof(ViewEditTeacher));

        }
    }
}
