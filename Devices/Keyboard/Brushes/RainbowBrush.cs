using System;
using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class RainbowBrush : IBrush
    {
        #region Properties & Fields

        public PointF StartPoint { get; set; } = new PointF(0f, 0.5f);
        public PointF EndPoint { get; set; } = new PointF(1f, 0.5f);
        public float StartHue { get; set; }
        public float EndHue { get; set; }

        #endregion

        #region Constructors

        public RainbowBrush(float startHue = 0f, float endHue = 360f)
        {
            this.StartHue = startHue;
            this.EndHue = endHue;
        }

        public RainbowBrush(PointF startPoint, PointF endPoint, float startHue = 0f, float endHue = 360f)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.StartHue = startHue;
            this.EndHue = endHue;
        }

        #endregion

        #region Methods

        public Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            PointF startPoint = new PointF(StartPoint.X * rectangle.Width, StartPoint.Y * rectangle.Height);
            PointF endPoint = new PointF(EndPoint.X * rectangle.Width, EndPoint.Y * rectangle.Height);

            float offset = GradientHelper.CalculateGradientOffset(startPoint, endPoint, point);
            float range = EndHue - StartHue;
            float progress = (StartHue + (range * offset)) / 360f;

            float div = (Math.Abs(progress % 1) * 6);
            int value = (int)((div % 1) * 255);

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, value, 0);
                case 1:
                    return Color.FromArgb(255, 255 - value, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, value);
                case 3:
                    return Color.FromArgb(255, 0, 255 - value, 255);
                case 4:
                    return Color.FromArgb(255, value, 0, 255);
                case 5:
                    return Color.FromArgb(255, 255, 0, 255 - value);
                default:
                    return Color.Transparent;
            }
        }

        #endregion
    }
}
