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
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ViewCalendar), typeof(ViewCalendar));
            Routing.RegisterRoute(nameof(ViewTeacherNonAttendances), typeof(ViewTeacherNonAttendances));

        }
    }
}
