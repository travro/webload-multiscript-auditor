using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;

namespace WMSA_Project.Utilities
{
    public sealed class ColorDispenser
    {
        private Queue<SolidColorBrush> _brushes;
        private static ColorDispenser _colorDispenser;

        private ColorDispenser()
        {
            _brushes = new Queue<SolidColorBrush>(
                new List<SolidColorBrush>
                {
                  Brushes.Coral,
                  Brushes.Blue,
                  Brushes.Goldenrod,
                  Brushes.Green,
                  Brushes.Salmon,
                  Brushes.Gray,
                  Brushes.SeaGreen,
                  Brushes.SteelBlue,
                  Brushes.Pink
                });
        }
        public static ColorDispenser Dispenser
        {
            get
            {
                if (_colorDispenser == null)
                {
                    _colorDispenser = new ColorDispenser();
                }
                return _colorDispenser;
            }
        }

        public SolidColorBrush GetNextColor()
        {
            var brush = _brushes.Dequeue();

            if (brush != null)
            {
                _brushes.Enqueue(brush);
                return brush;
            }
            else return Brushes.Red;
        }
        
    }
}
