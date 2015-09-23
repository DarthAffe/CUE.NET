using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class GradientStop
    {

        #region Properties & Fields

        public float Offset { get; set; }

        public Color Color { get; set; }

        #endregion

        #region Constructors

        public GradientStop(float offset, Color color)
        {
            this.Offset = offset;
            this.Color = color;
        }

        #endregion
    }
}
