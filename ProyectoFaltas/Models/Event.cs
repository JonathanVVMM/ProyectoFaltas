using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } // Nueva propiedad Date

        // Lista de profesores que faltaron en este evento
        public ObservableCollection<Profesor> Profesores { get; set; } = new ObservableCollection<Profesor>();

        // Tipo de falta asociado con este evento
        public TipoFalta TipoFalta { get; set; }
    }


}
