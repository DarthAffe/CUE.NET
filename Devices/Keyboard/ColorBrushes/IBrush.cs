using System.Drawing.Color;
using System.Drawing.Point;

namespace CUE.NET.Devices.Keyboard.ColorBrushes

{
    public interface IBrush
    {
		public Color getColorAtPoint(Point point);
    }
}