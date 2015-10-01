using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Brushes.Gradient
{
    public interface IGradient
    {
        Color GetColor(float offset);
    }
}
