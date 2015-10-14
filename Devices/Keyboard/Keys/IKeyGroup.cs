using System.Collections.Generic;
using CUE.NET.Devices.Keyboard.Brushes;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public interface IKeyGroup
    {
        IEnumerable<CorsairKey> Keys { get; }

        IBrush Brush { get; set; }

        int ZIndex { get; set; }
    }
}
