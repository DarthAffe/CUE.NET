using System;
using System.Drawing;

namespace CUE.NET.Helper
{
    public static class RectangleHelper
    {
        public static RectangleF CreateRectangleFromPoints(PointF point1, PointF point2)
        {
            float posX = Math.Min(point1.X, point2.X);
            float posY = Math.Min(point1.Y, point2.Y);
            float width = Math.Max(point1.X, point2.X) - posX;
            float height = Math.Max(point1.Y, point2.Y) - posY;

            return new RectangleF(posX, posY, width, height);
        }

        public static RectangleF CreateRectangleFromRectangles(RectangleF point1, RectangleF point2)
        {
            float posX = Math.Min(point1.X, point2.X);
            float posY = Math.Min(point1.Y, point2.Y);
            float width = Math.Max(point1.X + point1.Width, point2.X + point2.Width) - posX;
            float height = Math.Max(point1.Y + point1.Height, point2.Y + point2.Height) - posY;

            return new RectangleF(posX, posY, width, height);
        }

        public static float CalculateIntersectPercentage(RectangleF rect, RectangleF referenceRect)
        {
            if (rect.IsEmpty || referenceRect.IsEmpty) return 0;

            referenceRect.Intersect(rect); // replace referenceRect with intersect
            return referenceRect.IsEmpty ? 0 : (referenceRect.Width * referenceRect.Height) / (rect.Width * rect.Height);
        }
    }
}
