using System.Drawing;

namespace CUE.NET.Devices.Keyboard.ColorBrushes
{
    public interface IBrush
    {
        Color GetColorAtPoint(RectangleF rectangle, PointF point);
    }
}