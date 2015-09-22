using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.ColorBrushes;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public interface IKeyGroup
    {
        IEnumerable<CorsairKey> Keys { get; }

        IBrush Brush { get; set; }
    }
}
