using Syncfusion.Maui.Scheduler;

namespace ProyectoFaltas.Views;

public partial class ViewCalendar : ContentPage
{
	public ViewCalendar()
	{
		InitializeComponent();
		SfScheduler scheduler = new SfScheduler();
		this.Content = scheduler;
    }
}