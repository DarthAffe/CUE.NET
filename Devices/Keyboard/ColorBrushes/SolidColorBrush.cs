using System.Drawing;

namespace CUE.NET.Devices.Keyboard.ColorBrushes
{
    public class SolidColorBrush : IBrush
    {
        Color color;

        #region Constructors

        public SolidColorBrush(Color color)
        {
            this.color = color;
        }

        #endregion

        #region Methods

        public Color getColorAtPoint(Point point)
        {
            /* a solid color brush returns the same color no matter the point */
            return this.color;
        }

        public Color getColor()
        {
            return this.color;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        #endregion
    }
}