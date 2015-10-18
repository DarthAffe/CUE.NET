using System.Collections.Generic;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace CUE.NET.Devices.Keyboard.Keys
{
    public interface IKeyGroup
    {
        /// <summary>
        /// Gets a read-only collection containing the keys from this group.
        /// </summary>
        IEnumerable<CorsairKey> Keys { get; }

        /// <summary>
        /// Gets or sets the brush which should be drawn over this group.
        /// </summary>
        IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of this keygroup to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// Gets a list containing all LEDs of this group.
        /// </summary>
        /// <returns>The list containing all LEDs of this group.</returns>
        IEnumerable<CorsairLed> GetLeds();
    }
}
