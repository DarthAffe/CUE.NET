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

            // Based on https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/

            PointF startPoint = new PointF(StartPoint.X * rectangle.Width, StartPoint.Y * rectangle.Height);
            PointF endPoint = new PointF(EndPoint.X * rectangle.Width, EndPoint.Y * rectangle.Height);

            PointF intersectingPoint;
            if (startPoint.Y.Equals(endPoint.Y)) // Horizontal case
                intersectingPoint = new PointF(point.X, startPoint.Y);

            else if (startPoint.X.Equals(endPoint.X)) // Vertical case
                intersectingPoint = new PointF(startPoint.X, point.Y);

            else // Diagnonal case
            {
                float slope = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
                float orthogonalSlope = -1 / slope;
                
                float startYIntercept = startPoint.Y - slope * startPoint.X;
                float pointYIntercept = point.Y - orthogonalSlope * point.X;

                float intersectingPointX = (pointYIntercept - startYIntercept) / (slope - orthogonalSlope);
                float intersectingPointY = slope * intersectingPointX + startYIntercept;
                intersectingPoint = new PointF(intersectingPointX, intersectingPointY);
            }

            // Calculate distances relative to the vector start
            float intersectDistance = CalculateDistance(intersectingPoint, startPoint, endPoint);
            float gradientLength = CalculateDistance(endPoint, startPoint, endPoint);

            float offset = intersectDistance / gradientLength;

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

        // Based on https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        /// <summary>
        /// Returns the signed magnitude of a point on a vector
        /// </summary>
        private float CalculateDistance(PointF point, PointF origin, PointF direction)
        {
            float distance = (float)Math.Sqrt((point.Y - origin.Y) * (point.Y - origin.Y) + (point.X - origin.X) * (point.X - origin.X));

            return (((point.Y < origin.Y) && (direction.Y > origin.Y)) ||
                ((point.Y > origin.Y) && (direction.Y < origin.Y)) ||
                ((point.Y.Equals(origin.Y)) && (point.X < origin.X) && (direction.X > origin.X)) ||
                ((point.Y.Equals(origin.Y)) && (point.X > origin.X) && (direction.X < origin.X)))
                ? -distance : distance;
        }

        #endregion
    }
}
