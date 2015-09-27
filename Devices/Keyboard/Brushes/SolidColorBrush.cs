// ReSharper disable MemberCanBePrivate.Global

using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class SolidColorBrush : AbstractBrush
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

        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            return FinalizeColor(Color);
        }

        #endregion
    }
}