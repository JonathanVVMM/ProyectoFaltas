using System;
using System.Globalization;
using Microsoft.Maui.Graphics;

namespace ProyectoFaltas.Metodos
{
    public class HexColorConverter : IValueConverter
    {
        private static readonly Dictionary<string, Color> ColorNameMap = new Dictionary<string, Color>
        {
            { "Rojo", Colors.Red },
            { "Verde", Colors.Green },
            { "Azul", Colors.Blue },
            { "Amarillo", Colors.Yellow },
            { "Gris", Colors.Gray },
            { "Naranja", Colors.Orange },
            { "Morado", Colors.Purple },
            { "Rosa", Colors.Pink },
            { "Marron", Colors.Brown },
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString && !string.IsNullOrEmpty(colorString))
            {
                if (ColorNameMap.ContainsKey(colorString.Trim()))
                {
                    return ColorNameMap[colorString.Trim()];
                }

                if (colorString.StartsWith("#"))
                {
                    try
                    {
                        return Color.FromArgb(colorString);
                    }
                    catch
                    {
                        return Colors.Gray;
                    }
                }
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
