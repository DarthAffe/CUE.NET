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
        public int Alpha { get; set; } = 255;

        #endregion

        #region Constructors

        public RainbowBrush(float startHue = 0f, float endHue = 360f, int alpha = 255)
        {
            this.StartHue = startHue;
            this.EndHue = endHue;
            this.Alpha = alpha;
        }

        public RainbowBrush(PointF startPoint, PointF endPoint, float startHue = 0f, float endHue = 360f, int alpha = 255)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.StartHue = startHue;
            this.EndHue = endHue;
            this.Alpha = alpha;
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
                    return Color.FromArgb(Alpha, 255, value, 0);
                case 1:
                    return Color.FromArgb(Alpha, 255 - value, 255, 0);
                case 2:
                    return Color.FromArgb(Alpha, 0, 255, value);
                case 3:
                    return Color.FromArgb(Alpha, 0, 255 - value, 255);
                case 4:
                    return Color.FromArgb(Alpha, value, 0, 255);
                case 5:
                    return Color.FromArgb(Alpha, 255, 0, 255 - value);
                default:
                    return Color.Transparent;
            }
        }

        #endregion
    }
}
