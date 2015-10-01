// ReSharper disable MemberCanBePrivate.Global

using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes.Gradient
{
    public class RainbowGradient : IGradient
    {
        #region Properties & Fields

        public float StartHue { get; set; }
        public float EndHue { get; set; }

        #endregion

        #region Constructors

        public RainbowGradient(float startHue = 0f, float endHue = 360f)
        {
            this.StartHue = startHue;
            this.EndHue = endHue;
        }

        #endregion

        #region Methods

        #endregion

        public Color GetColor(float offset)
        {
            float range = EndHue - StartHue;
            float hue = (StartHue + (range * offset)) % 360f;
            if (hue < 0)
                hue += 360;
            return ColorHelper.ColorFromHSV(hue, 1f, 1f);
        }
    }
}
