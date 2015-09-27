// ReSharper disable MemberCanBePrivate.Global

using System.Drawing;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class RainbowBrush : AbstractBrush
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

        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            PointF startPoint = new PointF(StartPoint.X * rectangle.Width, StartPoint.Y * rectangle.Height);
            PointF endPoint = new PointF(EndPoint.X * rectangle.Width, EndPoint.Y * rectangle.Height);

            float offset = GradientHelper.CalculateGradientOffset(startPoint, endPoint, point);
            float range = EndHue - StartHue;

            float hue = (StartHue + (range * offset)) % 360f;
            return FinalizeColor(ColorHelper.ColorFromHSV(hue, 1f, 1f));
        }

        #endregion
    }
}
