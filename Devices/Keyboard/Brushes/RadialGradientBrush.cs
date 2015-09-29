using System;
using System.Drawing;
using CUE.NET.Devices.Keyboard.Brushes.Gradient;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class RadialGradientBrush : AbstractBrush
    {
        #region Properties & Fields

        public PointF Center { get; set; } = new PointF(0.5f, 0.5f);
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructors

        public RadialGradientBrush()
        { }

        public RadialGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }

        public RadialGradientBrush(PointF center, IGradient gradient)
        {
            this.Center = center;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        public override Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            PointF centerPoint = new PointF(rectangle.X + rectangle.Width * Center.X, rectangle.Y + rectangle.Height * Center.Y);

            // Calculate the distance to the farthest point from the center as reference (this has to be a corner)
            float refDistance = (float)Math.Max(Math.Max(Math.Max(GradientHelper.CalculateDistance(rectangle.Location, centerPoint),
                GradientHelper.CalculateDistance(new PointF(rectangle.X + rectangle.Width, rectangle.Y), centerPoint)),
                GradientHelper.CalculateDistance(new PointF(rectangle.X, rectangle.Y + rectangle.Height), centerPoint)),
                GradientHelper.CalculateDistance(new PointF(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), centerPoint));

            float distance = GradientHelper.CalculateDistance(point, centerPoint);
            float offset = distance / refDistance;
            return FinalizeColor(Gradient.GetColor(offset));
        }

        #endregion
    }
}
