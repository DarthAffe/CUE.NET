using System.Collections.Generic;
using System.Drawing;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public interface IKeyGroup
    {
        IEnumerable<CorsairKey> Keys { get; }

        void SetColor(Color color);
    }
}
