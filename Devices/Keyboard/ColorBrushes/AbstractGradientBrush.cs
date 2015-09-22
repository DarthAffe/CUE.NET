using System.Drawing;

namespace CUE.NET.Devices.Keyboard.ColorBrushes
{
    abstract class AbstractGradientBrush : IBrush
    {
        #region Fields

        private Point anchor;
        //private Vect

        #endregion

        #region Methods

        public abstract Color getColorAtPoint(Point point);

        #endregion
    }
}
