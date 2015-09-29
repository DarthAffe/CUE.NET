using System;
using System.Drawing;

namespace CUE.NET.Helper
{
    public static class GradientHelper
    {
        // Based on https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        public static float CalculateLinearGradientOffset(PointF startPoint, PointF endPoint, PointF point)
        {
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

            return intersectDistance / gradientLength;
        }

        // Based on https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        /// <summary>
        /// Returns the signed magnitude of a point on a vector
        /// </summary>
        public static float CalculateDistance(PointF point, PointF origin, PointF direction)
        {
            float distance = (float)Math.Sqrt((point.Y - origin.Y) * (point.Y - origin.Y) + (point.X - origin.X) * (point.X - origin.X));

            return (((point.Y < origin.Y) && (direction.Y > origin.Y)) ||
                ((point.Y > origin.Y) && (direction.Y < origin.Y)) ||
                ((point.Y.Equals(origin.Y)) && (point.X < origin.X) && (direction.X > origin.X)) ||
                ((point.Y.Equals(origin.Y)) && (point.X > origin.X) && (direction.X < origin.X)))
                ? -distance : distance;
        }
    }
}
