using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFaltas.Models
{
    public class ColorOption
    {
        public string Name { get; set; }
        public Color ColorValue { get; set; }


        //Crear la lista de colores:
        public static ObservableCollection<ColorOption> ColorOptions { get; } = new ObservableCollection<ColorOption>
        {
            //Los que ya tenían
            new ColorOption { Name = "Verde", ColorValue = Colors.Green },
            new ColorOption { Name = "Amarillo", ColorValue = Colors.Yellow },
            new ColorOption { Name = "Rojo", ColorValue = Colors.Red },
            new ColorOption { Name = "Azul", ColorValue = Colors.Blue },
            new ColorOption { Name = "Gris", ColorValue = Colors.Gray },

            //Más opciones
            new ColorOption { Name = "Naranja", ColorValue = Colors.Orange },
            new ColorOption { Name = "Morado", ColorValue = Colors.Purple },
            new ColorOption { Name = "Rosa", ColorValue = Colors.Pink },
            new ColorOption { Name = "Marron", ColorValue = Colors.Brown }
        };
    }
}
