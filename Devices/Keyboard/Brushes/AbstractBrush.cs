using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public abstract class AbstractBrush : IBrush
    {
        #region Properties & Fields

        public float Brightness { get; set; }
        public float Opacity { get; set; }

        #endregion

        #region Constructors

        protected AbstractBrush(float brightness = 1f, float opacity = 1f)
        {
            this.Brightness = brightness;
            this.Opacity = opacity;
        }

        #endregion

        #region Methods

        public abstract Color GetColorAtPoint(RectangleF rectangle, PointF point);

        protected virtual Color FinalizeColor(Color color)
        {
            // Since we use HSV to calculate there is no way to make a color 'brighter' than 100%
            // Be carefull with the naming: Since we use HSV the correct term is 'value' but outside we call it 'brightness'
            // THIS IS NOT A HSB CALCULATION!!!
            float finalBrightness = color.GetHSVValue() * (Brightness < 0 ? 0 : (Brightness > 1f ? 1f : Brightness));
            byte finalAlpha = (byte)(color.A * (Opacity < 0 ? 0 : (Opacity > 1f ? 1f : Opacity)));
            return ColorHelper.ColorFromHSV(color.GetHue(), color.GetHSVSaturation(), finalBrightness, finalAlpha);
        }

        #endregion
    }
}
