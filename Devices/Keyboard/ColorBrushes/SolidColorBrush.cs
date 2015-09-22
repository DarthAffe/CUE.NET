using System.Drawing;

namespace CUE.NET.Devices.Keyboard.ColorBrushes
{
    public class SolidColorBrush : IBrush
    {
        #region Properties & Fields

        public Color Color { get; set; }

        #endregion

        #region Constructors

        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        #endregion

        #region Methods

        public Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            return Color; // A solid color brush returns the same color no matter the point
        }

        #endregion
    }
}