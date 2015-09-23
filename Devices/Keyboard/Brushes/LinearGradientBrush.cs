// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

            // Taken from https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/

            float x3 = point.X;
            float y3 = point.Y;

            float x1 = StartPoint.X * rectangle.Width;
            float y1 = StartPoint.Y * rectangle.Height;
            PointF p1 = new PointF(x1, y1); // Starting point

            float x2 = EndPoint.X * rectangle.Width;
            float y2 = EndPoint.Y * rectangle.Height;
            PointF p2 = new PointF(x2, y2);  //End point

            // Calculate intersecting points 
            PointF p4;

            if (y1.Equals(y2)) // Horizontal case
                p4 = new PointF(x3, y1);

            else if (x1.Equals(x2)) // Vertical case
                p4 = new PointF(x1, y3);

            else // Diagnonal case
            {
                float m = (y2 - y1) / (x2 - x1);
                float m2 = -1 / m;
                float b = y1 - m * x1;
                float c = y3 - m2 * x3;

                float x4 = (c - b) / (m - m2);
                float y4 = m * x4 + b;
                p4 = new PointF(x4, y4);
            }

            // Calculate distances relative to the vector start
            float d4 = Dist(p4, p1, p2);
            float d2 = Dist(p2, p1, p2);

            float x = d4 / d2;

            // Clip the input if before or after the max/min offset values
            float max = GradientStops.Max(n => n.Offset);
            if (x > max)
                x = max;

            float min = GradientStops.Min(n => n.Offset);
            if (x < min)
                x = min;

            // Find gradient stops that surround the input value
            GradientStop gs0 = GradientStops.Where(n => n.Offset <= x).OrderBy(n => n.Offset).Last();
            GradientStop gs1 = GradientStops.Where(n => n.Offset >= x).OrderBy(n => n.Offset).First();

            float y = 0f;
            if (!gs0.Offset.Equals(gs1.Offset))
                y = ((x - gs0.Offset) / (gs1.Offset - gs0.Offset));

            byte colA = (byte)((gs1.Color.A - gs0.Color.A) * y + gs0.Color.A);
            byte colR = (byte)((gs1.Color.R - gs0.Color.R) * y + gs0.Color.R);
            byte colG = (byte)((gs1.Color.G - gs0.Color.G) * y + gs0.Color.G);
            byte colB = (byte)((gs1.Color.B - gs0.Color.B) * y + gs0.Color.B);
            return Color.FromArgb(colA, colR, colG, colB);
        }

        // Taken from https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        /// <summary>
        /// Returns the signed magnitude of a point on a vector with origin po and pointing to pf
        /// </summary>
        private float Dist(PointF px, PointF po, PointF pf)
        {
            float d = (float)Math.Sqrt((px.Y - po.Y) * (px.Y - po.Y) + (px.X - po.X) * (px.X - po.X));

            return (((px.Y < po.Y) && (pf.Y > po.Y)) ||
                ((px.Y > po.Y) && (pf.Y < po.Y)) ||
                ((px.Y.Equals(po.Y)) && (px.X < po.X) && (pf.X > po.X)) ||
                ((px.Y.Equals(po.Y)) && (px.X > po.X) && (pf.X < po.X)))
                ? -d : d;
        }

        #endregion
    }
}
