using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFaltas.ViewModels
{
    public class VMConfigureTeachers : INotifyPropertyChanged
    {

        //ESTA PARTE DEL CÓDIGO ES SUPER IMPORTANTE PARA QUE FUNCIONE EL PATRÓN SINGLETON
        //AUNQUE AÑADAS EN MauiProgram que es un singleton no lo gestiona bien así que lo implemento manualmente
        //es necesario porque si crea 2 instancias de la misma clase no se actualizan los datos.
        //Aunque acceden a la misma clase en la view crea 2 instancias
        private static VMConfigureTeachers _instance;
        public static VMConfigureTeachers Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VMConfigureTeachers();
                }
                return _instance;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private VMConfigureTeachers()
        {

            ModifyProfesorCommand = new Command<int>(ModifyProfesor);
            DeleteProfesorCommand = new Command<int>(DeleteProfesor);
            AddProfesorCommand = new Command(AddProfesor);
            CancelarActualizarProfesorCommand = new Command(CancelarActualizarProfesor);
            ActualizarProfesorCommand = new Command(ActualizarProfesor);
            IconoAyudaActualizarDatosCommand = new Command(IconoAyudaActualizarDatos);
            IconoAyudaEditarDatosCommand = new Command(IconoAyudaEditarDatos);
            IconoAyudaListaDatosCommand = new Command(IconoAyudaListaDatos);

            recargarLista();

        }

        private DatabaseService database = new DatabaseService();


        private ObservableCollection<Profesor> _ListaProfesores = new ObservableCollection<Profesor>();
        public ObservableCollection<Profesor> ListaProfesores
        {
            get => _ListaProfesores;
            set
            {
                _ListaProfesores = value; OnPropertyChanged();
            }
        }

        // Propiedades
        private string _nombre = "";
        private string _apellidos = "";
        private string _tipo = "";
        private int _id;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Apellidos
        {
            get => _apellidos;
            set
            {
                if (_apellidos != value)
                {
                    _apellidos = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Tipo
        {
            get => _tipo;
            set
            {
                if (_tipo != value)
                {
                    _tipo = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _nombreNuevo;
        public string NombreNuevo
        {
            get { return _nombreNuevo; }
            set { _nombreNuevo = value; OnPropertyChanged(); }
        }


        private string _apellidosNuevo;
        public string ApellidosNuevo
        {
            get { return _apellidosNuevo; }
            set { _apellidosNuevo = value; OnPropertyChanged(); }
        }

        private string _tipoNuevo;
        public string TipoNuevo
        {
            get { return _tipoNuevo; }
            set { _tipoNuevo = value; OnPropertyChanged(); }
        }

        private Profesor _profesorEditando;
        public Profesor ProfesorEditando
        {
            get { return _profesorEditando; }
            set { _profesorEditando = value; OnPropertyChanged(); }
        }

        private bool _editando = false;
        public bool Editando
        {
            get { return _editando; }
            set { _editando = value; OnPropertyChanged(); }
        }

        //-------------------------------

        public ICommand IconoAyudaActualizarDatosCommand { get; set; }
        public ICommand IconoAyudaEditarDatosCommand { get; set; }
        public ICommand IconoAyudaListaDatosCommand { get; set; }
        public ICommand AddProfesorCommand { get; set; }
        public ICommand DeleteProfesorCommand { get; set; }
        public ICommand ModifyProfesorCommand { get; set; }
        public ICommand ActualizarProfesorCommand { get; set; }
        public ICommand CancelarActualizarProfesorCommand { get; set; }

        private async void recargarLista()
        {
            ListaProfesores = new ObservableCollection<Profesor>(await database.GetProfesoresAsync());
        }

        public async void AddProfesor()
        {
            if (Tipo != "" && Apellidos != "" && Nombre != "")
            {
                Profesor prof = new Profesor();
                prof.Apellidos = Apellidos;
                prof.Nombre = Nombre;
                prof.Tipo = Tipo;

                await database.SaveProfesorAsync(prof);
                recargarLista();
            }
        }

        public async void DeleteProfesor(int ItemId)
        {
            if (await App.Current.MainPage.DisplayAlert("Borrar Profesor", "Está seguro de borrar el profesor seleccionado?", "Confirmar", "Cancelar"))
            {
                Profesor p = await database.GetProfesorAsync(ItemId);

                NombreNuevo = ""; ApellidosNuevo = ""; TipoNuevo = "";
                ProfesorEditando = null;
                Editando = false;
                await database.DeleteProfesorAsync(p);
                recargarLista();



            }
        }

        //funcion del imagebutton que es para activar la modificacion
        public async void ModifyProfesor(int ItemId)
        {
            if (await App.Current.MainPage.DisplayAlert("Actualizar Profesor", "Está seguro de actualizar el profesor seleccionado?", "Confirmar", "Cancelar"))
            {
                ProfesorEditando = ListaProfesores.FirstOrDefault(p => p.Id == ItemId);
                Editando = true;
            }
        }

        //funcion del boton para actualizar los datos
        public async void ActualizarProfesor()
        {
            if (await App.Current.MainPage.DisplayAlert("Actualizar Profesor", "Está seguro de actualizar el profesor seleccionado?", "Confirmar", "Cancelar"))
            {
                if (!String.IsNullOrEmpty(NombreNuevo)) ProfesorEditando.Nombre = NombreNuevo;
                if (!String.IsNullOrEmpty(ApellidosNuevo)) ProfesorEditando.Apellidos = ApellidosNuevo;
                if (!String.IsNullOrEmpty(TipoNuevo)) ProfesorEditando.Tipo = TipoNuevo;
                await database.SaveProfesorAsync(ProfesorEditando);
                Editando = false;
                ProfesorEditando = null;
                NombreNuevo = ""; ApellidosNuevo = ""; TipoNuevo = "";
                recargarLista();
            }
        }

        public async void CancelarActualizarProfesor()
        {
            if (await App.Current.MainPage.DisplayAlert("Cancelación de Actualización", "Está seguro de cancelar la actualización del profesor?", "Confirmar", "Cancelar"))
            {
                ProfesorEditando = null;
                Editando = false;
                ApellidosNuevo = ""; NombreNuevo = ""; TipoNuevo = "";
            }
        }

        public async void IconoAyudaActualizarDatos()
        {
            await App.Current.MainPage.DisplayAlert("Información de ayuda", "Este apartado únicamente te dejará editarlo cuando estés editando a algún profesor.\nCuando le des al botón de \"Actualizar datos rellenados\" solo se modificarán los campos que tengan algo escrito.", "Salir");
        }

        public async void IconoAyudaEditarDatos()
        {
            await App.Current.MainPage.DisplayAlert("Información de ayuda", "Este apartado es para ver la lista de profesores donde tienes:\n - Los datos de cada profesor.\n - Un botón para editar al profesor de esa casilla que activará el modo edición de la derecha (verás cambios en la pantalla).\n - Un botón para borrar al profesor.\n\nTe saldrá un aviso aunque le des click, por si acaso, en ambos botones.", "Salir");
        }

        public async void IconoAyudaListaDatos()
        {
            await App.Current.MainPage.DisplayAlert("Información de ayuda", "Este apartado es para ver la lista de profesores donde tienes:\n - Los datos de cada profesor.\n - Un apartado a la derecha donde puedes añadir un profesor ingresando sus datos y dándole al botón.\n\nTe saldrá un aviso aunque le des click, por si acaso, en ambos botones.", "Salir");
        }
    }
}
