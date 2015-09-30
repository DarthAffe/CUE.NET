using System;
using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    //TODO DarthAffe 30.09.2015: Like this the brush seems kinda useless. Think about making it cool.
    public class RandomColorBrush : AbstractBrush
    {
        #region Properties & Fields

        private Random _random = new Random();

        #endregion

        #region Methods

        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            return FinalizeColor(ColorHelper.ColorFromHSV((float)_random.NextDouble() * 360f, 1, 1));
        }

        #endregion
    }
}
