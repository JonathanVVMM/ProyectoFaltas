using ProyectoFaltas.Views;

namespace ProyectoFaltas
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //------------------------- Fuera del tabBar -------------------------
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(viewCreateYear), typeof(viewCreateYear));

            //------------------------- Dentro del tabBar -------------------------
            Routing.RegisterRoute(nameof(ViewCalendar), typeof(ViewCalendar));
            Routing.RegisterRoute(nameof(ViewCreateTeacher), typeof(ViewCreateTeacher));
            Routing.RegisterRoute(nameof(ViewEditTeacher), typeof(ViewEditTeacher));
            Routing.RegisterRoute(nameof(ViewTiposFaltas), typeof(ViewTiposFaltas));

            //------------------------- A parte -------------------------
            Routing.RegisterRoute(nameof(ViewTeacherNonAttendances), typeof(ViewTeacherNonAttendances));
        }
    }
}
