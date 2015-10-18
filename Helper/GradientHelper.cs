// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Drawing;

namespace CUE.NET.Helper
{
    /// <summary>
    /// Offers some extensions and helper-methods for gradient related things.
    /// </summary>
    public static class GradientHelper
    {
        // Based on https://dotupdate.wordpress.com/2008/01/28/find-the-color-of-a-point-in-a-lineargradientbrush/
        /// <summary>
        /// Calculates the offset of an given point on an gradient.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient.</param>
        /// <param name="endPoint">The end point of the gradient.</param>
        /// <param name="point">The point on the gradient to which the offset is calculated.</param>
        /// <returns>The offset of the point on the gradient.</returns>
        public static float CalculateLinearGradientOffset(PointF startPoint, PointF endPoint, PointF point)
        {
            PointF intersectingPoint;
            if (startPoint.Y.Equals(endPoint.Y)) // Horizontal case
                intersectingPoint = new PointF(point.X, startPoint.Y);

            else if (startPoint.X.Equals(endPoint.X)) // Vertical case
                intersectingPoint = new PointF(startPoint.X, point.Y);

            else // Diagonal case
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
        /// Returns the signed magnitude of a point on a vector.
        /// </summary>
        /// <param name="point">The point on the vector of which the magnitude should be calculated.</param>
        /// <param name="origin">The origin of the vector.</param>
        /// <param name="direction">The direction of the vector.</param>
        /// <returns>The signed magnitude of a point on a vector.</returns>
        public static float CalculateDistance(PointF point, PointF origin, PointF direction)
        {
            float distance = CalculateDistance(point, origin);

            return (((point.Y < origin.Y) && (direction.Y > origin.Y)) ||
                ((point.Y > origin.Y) && (direction.Y < origin.Y)) ||
                ((point.Y.Equals(origin.Y)) && (point.X < origin.X) && (direction.X > origin.X)) ||
                ((point.Y.Equals(origin.Y)) && (point.X > origin.X) && (direction.X < origin.X)))
                ? -distance : distance;
        }

        /// <summary>
        /// Calculated the distance between two points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The distance between the two points.</returns>
        public static float CalculateDistance(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt((point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.X - point2.X) * (point1.X - point2.X));
        }
    }
}
