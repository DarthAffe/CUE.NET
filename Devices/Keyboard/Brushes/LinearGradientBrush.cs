// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global

using System.Drawing;
using CUE.NET.Devices.Keyboard.Brushes.Gradient;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class LinearGradientBrush : AbstractBrush
    {
        #region Properties & Fields

        public PointF StartPoint { get; set; } = new PointF(0f, 0.5f);
        public PointF EndPoint { get; set; } = new PointF(1f, 0.5f);
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructor

        public LinearGradientBrush()
        { }

        public LinearGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }

        public LinearGradientBrush(PointF startPoint, PointF endPoint, IGradient gradient)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            if (Gradient == null) return Color.Transparent;

            PointF startPoint = new PointF(StartPoint.X * rectangle.Width, StartPoint.Y * rectangle.Height);
            PointF endPoint = new PointF(EndPoint.X * rectangle.Width, EndPoint.Y * rectangle.Height);

            float offset = GradientHelper.CalculateLinearGradientOffset(startPoint, endPoint, point);
            return FinalizeColor(Gradient.GetColor(offset));
        }

        #endregion
    }
}
