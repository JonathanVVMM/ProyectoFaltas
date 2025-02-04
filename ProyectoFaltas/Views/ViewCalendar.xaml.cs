using Plugin.Maui.Calendar.Models;
using ProyectoFaltas.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ProyectoFaltas.Database;

namespace ProyectoFaltas.Views
{
    public partial class ViewCalendar : ContentPage, INotifyPropertyChanged
    {
        private DatabaseService database = new DatabaseService();

        public ObservableCollection<Event> Events { get; set; }
        private Event _selectedEvent;

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    OnPropertyChanged(nameof(SelectedEvent));
                    OnPropertyChanged(nameof(IsEventSelected));
                }
            }
        }

        private ObservableCollection<Falta> _listaFaltas;

        public ObservableCollection<Falta> ListaFaltas
        {
            get => _listaFaltas;
            set
            {
                _listaFaltas = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDay;

        public DateTime SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                OnPropertyChanged();
            }
        }

        public ICommand DayTappedCommand { get; }

        public ViewCalendar()
        {
            InitializeComponent();
            Events = new ObservableCollection<Event>
            {
                new Event { Name = "Evento 1", Description = "Descripción del Evento 1", Date = new DateTime(2025, 1, 21) },
                new Event { Name = "Evento 2", Description = "Descripción del Evento 2", Date = new DateTime(2025, 1, 21) }
            };
            DayTappedCommand = new Command<DateTime>(OnDayTapped);  // Enlazamos el comando
            BindingContext = this; // Set the BindingContext
        }

        private void OnDayTapped(DateTime selectedDate)
        {
            // Filtrar eventos según la fecha seleccionada
            SelectedEvent = Events.FirstOrDefault(evento => evento.Date.Date == selectedDate);
            SelectedDay = selectedDate;
            // Si no hay evento en esa fecha, se puede limpiar el evento seleccionado
            if (SelectedEvent == null)
            {
                SelectedEvent = null;
            }
        }

        public bool IsEventSelected => SelectedEvent != null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class Event
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; } // Nueva propiedad Date
        }

        private void AddEventInThisDay_Clicked(object sender, EventArgs e)
        {
            DateTime fechaActual = DateTime.Now;
            Events.Add(new Event { Date = fechaActual });
        }

        private async void recargarLista()
        {
            if (Curso.CursoActual != null)
            {
                ListaFaltas = new ObservableCollection<Falta>(await database.GetFaltasAsync(Curso.CursoActual.Id));
                Events = new ObservableCollection<Event>();
                foreach (Falta item in ListaFaltas)
                {
                    Events.Add(new Event { Date = item.Fecha });
                }
            }



        }
    }
}

//agregar a la clase event una observable collection profesores para mostrar cuando elijamos un dia la lista de profesores que han faltado