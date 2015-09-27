using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Brushes
{
    public interface IBrush
    {
        float Brightness { get; set; }
        float Opacity { get; set; }

        Color GetColorAtPoint(RectangleF rectangle, PointF point);
    }
}