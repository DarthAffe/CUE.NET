// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CUE.NET.Helper;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public class LinearGradientBrush : IBrush
    {
        #region Properties & Fields

        public PointF StartPoint { get; set; } = new PointF(0f, 0.5f);
        public PointF EndPoint { get; set; } = new PointF(1f, 0.5f);
        public IList<GradientStop> GradientStops { get; } = new List<GradientStop>();

        #endregion

        #region Constructor

        public LinearGradientBrush()
        { }

        public LinearGradientBrush(PointF startPoint, PointF endPoint, params GradientStop[] gradientStops)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;

            foreach (GradientStop gradientStop in gradientStops)
                GradientStops.Add(gradientStop);
        }

        public LinearGradientBrush(params GradientStop[] gradientStops)
        {
            foreach (GradientStop gradientStop in gradientStops)
                GradientStops.Add(gradientStop);
        }

        #endregion

        #region Methods

        public Color GetColorAtPoint(RectangleF rectangle, PointF point)
        {
            if (!GradientStops.Any()) return Color.Transparent;
            if (GradientStops.Count == 1) return GradientStops.First().Color;

            PointF startPoint = new PointF(StartPoint.X * rectangle.Width, StartPoint.Y * rectangle.Height);
            PointF endPoint = new PointF(EndPoint.X * rectangle.Width, EndPoint.Y * rectangle.Height);

            float offset = GradientHelper.CalculateGradientOffset(startPoint, endPoint, point);

            // Clip the input if before or after the max/min offset values
            float max = GradientStops.Max(n => n.Offset);
            if (offset > max)
                offset = max;

            float min = GradientStops.Min(n => n.Offset);
            if (offset < min)
                offset = min;

            // Find gradient stops that surround the input value
            GradientStop gsBefore = GradientStops.Where(n => n.Offset <= offset).OrderBy(n => n.Offset).Last();
            GradientStop gsAfter = GradientStops.Where(n => n.Offset >= offset).OrderBy(n => n.Offset).First();

            float blendFactor = 0f;
            if (!gsBefore.Offset.Equals(gsAfter.Offset))
                blendFactor = ((offset - gsBefore.Offset) / (gsAfter.Offset - gsBefore.Offset));

            byte colA = (byte)((gsAfter.Color.A - gsBefore.Color.A) * blendFactor + gsBefore.Color.A);
            byte colR = (byte)((gsAfter.Color.R - gsBefore.Color.R) * blendFactor + gsBefore.Color.R);
            byte colG = (byte)((gsAfter.Color.G - gsBefore.Color.G) * blendFactor + gsBefore.Color.G);
            byte colB = (byte)((gsAfter.Color.B - gsBefore.Color.B) * blendFactor + gsBefore.Color.B);
            return Color.FromArgb(colA, colR, colG, colB);
        }

        #endregion
    }
}
