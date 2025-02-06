using ProyectoFaltas.Database;
using ProyectoFaltas.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            FaltasProfesorCommand = new Command<int>(FaltasProfesor);
            AddProfesorCommand = new Command(AddProfesor);
            CancelarActualizarProfesorCommand = new Command(CancelarActualizarProfesor);
            ActualizarProfesorCommand = new Command(ActualizarProfesor);
            IconoAyudaActualizarDatosCommand = new Command(IconoAyudaActualizarDatos);
            IconoAyudaEditarDatosCommand = new Command(IconoAyudaEditarDatos);
            IconoAyudaListaDatosCommand = new Command(IconoAyudaListaDatos);

            recargarEstadoProfesores();

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
        private string _tipo = "Interino";
        private string _estado = "Activo";
        private int _id;

        public string Estado
        {
            get => _estado;
            set
            {
                if (_estado != value)
                {
                    _estado = value;
                    OnPropertyChanged();
                }
            }
        }


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

        private string _estadoNuevo;
        public string EstadoNuevo
        {
            get { return _estadoNuevo; }
            set { _estadoNuevo = value; OnPropertyChanged(); }
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
        public ICommand FaltasProfesorCommand { get; set; }
        public ICommand ModifyProfesorCommand { get; set; }
        public ICommand ActualizarProfesorCommand { get; set; }
        public ICommand CancelarActualizarProfesorCommand { get; set; }

        private async void recargarEstadoProfesores()
        {
            ListaProfesores = new ObservableCollection<Profesor>(await database.GetProfesoresAsync());
            ObservableCollection<Profesor> ListaActivos = new ObservableCollection<Profesor>(await database.GetProfesoresActivosPorCursoAsync(Curso.CursoActual.Id));
            foreach (var item in ListaProfesores)
            {
                int cont = 0;
                foreach (var item2 in ListaActivos)
                {
                    if (item.Id == item2.Id) { item.Estado = "Activo"; cont += 1; }
                }
                if (cont == 0) { item.Estado = "Inactivo"; }
                await database.SaveProfesorAsync(item);
            }
            recargarLista();
        }

        private async void recargarLista()
        {
            ListaProfesores = new ObservableCollection<Profesor>(await database.GetProfesoresAsync());
        }

        public async void AddProfesor()
        {
            if (Apellidos != "" && Nombre != "")
            {
                Profesor prof = new Profesor();
                prof.Apellidos = Apellidos;
                prof.Nombre = Nombre;
                prof.Tipo = Tipo;
                prof.Estado = Estado;
                Apellidos = ""; Nombre = "";

                await database.SaveProfesorAsync(prof);

                if (prof.Estado == "Activo")
                {
                    var activoNuevo = new Activo();
                    activoNuevo.IdCursos = Curso.CursoActual.Id;
                    activoNuevo.IdProfesores = prof.Id;
                    await database.AddActivoAsync(activoNuevo);
                }

                recargarLista();
            }
        }

        public async void FaltasProfesor(int ItemId)
        {
            if (await App.Current.MainPage.DisplayAlert("Faltas del Profesor", "Si acepta será cambiado de página a la lista de faltas de este profesor", "Confirmar", "Cancelar"))
            {
                await Shell.Current.GoToAsync($"//ViewTeacherNonAttendances?IdProfesor={ItemId}");
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
                var cambiarActivo = false;
                if (!String.IsNullOrEmpty(NombreNuevo)) ProfesorEditando.Nombre = NombreNuevo;
                if (!String.IsNullOrEmpty(ApellidosNuevo)) ProfesorEditando.Apellidos = ApellidosNuevo;
                if (!String.IsNullOrEmpty(TipoNuevo)) ProfesorEditando.Tipo = TipoNuevo;
                if (!String.IsNullOrEmpty(EstadoNuevo) && ProfesorEditando.Estado != EstadoNuevo)
                {
                    ProfesorEditando.Estado = EstadoNuevo;
                    cambiarActivo = true;
                }
                await database.SaveProfesorAsync(ProfesorEditando);

                if (cambiarActivo)
                {
                    if (EstadoNuevo == "Activo")
                    {
                        var activoNuevo = new Activo();
                        activoNuevo.IdCursos = Curso.CursoActual.Id;
                        activoNuevo.IdProfesores = ProfesorEditando.Id;
                        await database.AddActivoAsync(activoNuevo);
                    }
                    else
                    {
                        await database.DeleteActivoAsync(ProfesorEditando.Id, Curso.CursoActual.Id);
                    }
                }

                Editando = false;
                ProfesorEditando = null;
                NombreNuevo = ""; ApellidosNuevo = ""; TipoNuevo = ""; EstadoNuevo = "";

                recargarLista();
            }
        }

        public async void CancelarActualizarProfesor()
        {
            if (await App.Current.MainPage.DisplayAlert("Cancelación de Actualización", "Está seguro de cancelar la actualización del profesor?", "Confirmar", "Cancelar"))
            {
                ProfesorEditando = null;
                Editando = false;
                ApellidosNuevo = ""; NombreNuevo = ""; TipoNuevo = ""; EstadoNuevo = "";
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
