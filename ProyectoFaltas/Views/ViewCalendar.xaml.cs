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
                    OnPropertyChanged(nameof(IsEventSelected)); // Aquí llamamos a OnPropertyChanged cuando cambie SelectedEvent
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

        // Nueva propiedad IsEventSelected
        public bool IsEventSelected
        {
            get => SelectedEvent != null; // No necesitamos un setter, solo un getter
        }


        private bool _isAddEventVisible;

        public bool IsAddEventVisible
        {
            get => _isAddEventVisible;
            set
            {
                _isAddEventVisible = value;
                OnPropertyChanged();
            }
        }

        public ViewCalendar()
        {
            InitializeComponent();
            DayTappedCommand = new Command<DateTime>(OnDayTapped);  // Enlazamos el comando
            recargarLista();
        }

        private void OnDayTapped(DateTime selectedDate)
        {
            // Establecer el día seleccionado
            SelectedDay = selectedDate;

            // Verificar si ya hay un evento para ese día
            SelectedEvent = Events.FirstOrDefault(evento => evento.Date.Date == selectedDate);

            // Si hay un evento para el día, mostrar los detalles del evento
            if (SelectedEvent != null)
            {
                // Mostrar detalles del evento seleccionado
                IsAddEventVisible = false; // Ocultar formulario de selección de falta si ya existe un evento
            }
            else
            {
                // Si no hay evento, mostrar el formulario para añadir una nueva falta
                IsAddEventVisible = true;
            }

            // Notificar que la propiedad ha cambiado (para la visibilidad de los elementos)
            OnPropertyChanged(nameof(SelectedDay));
            OnPropertyChanged(nameof(IsAddEventVisible));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void recargarLista()
        {
            if (Curso.CursoActual != null)
            {
                ListaFaltas = new ObservableCollection<Falta>(await database.GetFaltasAsync(Curso.CursoActual.Id));
                Events = new ObservableCollection<Event>();

                foreach (Falta item in ListaFaltas)
                {
                    // Crear el evento
                    var eventItem = new Event { Date = item.Fecha };

                    // Obtener los profesores que faltaron en esta falta
                    var profesores = await database.GetProfesoresByFaltaAsync(item.Id);
                    foreach (var profesor in profesores)
                    {
                        eventItem.Profesores.Add(profesor);
                    }

                    // Obtener el tipo de falta asociado
                    var tipoFalta = await database.GetTipoFaltaAsync(item.Id);  // Falta tiene el campo TipoFaltaId
                    eventItem.TipoFalta = tipoFalta;

                    Events.Add(eventItem);
                }

                BindingContext = this; // Establecer el BindingContext después de cargar los datos.
            }
        }

        private void AddEventInThisDay_Clicked(object sender, EventArgs e)
        {
            DateTime fechaActual = DateTime.Now;
            Events.Add(new Event { Date = fechaActual });
        }

        // Crear el método para añadir la falta con los detalles (el Picker de profesores y tipos de falta)
        private void AddEventWithDetails_Clicked(object sender, EventArgs e)
        {
            // Lógica para añadir la falta
            // Necesitarás recoger el profesor y tipo de falta seleccionados antes de añadir el evento

            var selectedProfesor = ProfesorPicker.SelectedItem as Profesor;
            var selectedTipoFalta = TipoFaltaPicker.SelectedItem as TipoFalta;

            if (selectedProfesor != null && selectedTipoFalta != null)
            {
                var newEvent = new Event
                {
                    Date = SelectedDay,
                    Name = $"Falta de {selectedProfesor.Nombre}",
                    Description = $"Falta de tipo: {selectedTipoFalta.Tipo}",
                    Profesores = new ObservableCollection<Profesor> { selectedProfesor },
                    TipoFalta = selectedTipoFalta
                };

                // Añadir el evento a la lista
                Events.Add(newEvent);
                recargarLista();  // Recargar la lista para reflejar los cambios

                // Ocultar el formulario de selección
                IsAddEventVisible = false;

                // Establecer el evento seleccionado para mostrar sus detalles
                SelectedEvent = newEvent; // Esto actualizará automáticamente IsEventSelected
            }
        }

        private void CancelAddEvent_Clicked(object sender, EventArgs e)
        {
            // Cancelar la adición del evento, cerrar el formulario de selección
            IsAddEventVisible = false;
        }
    }

}