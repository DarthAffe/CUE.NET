using System.Drawing;

namespace CUE.NET.Devices.Keyboard.ColorBrushes
{
    public interface IBrush
    {
        Color GetColorAtPoint(Point point);
    }
}