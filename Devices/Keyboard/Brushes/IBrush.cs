using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public interface IBrush
    {
        Color GetColorAtPoint(RectangleF rectangle, PointF point);
    }
}