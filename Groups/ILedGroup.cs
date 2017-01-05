// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Effects;

namespace CUE.NET.Groups
{
    /// <summary>
    /// Represents a basic led-group.
    /// </summary>
    public interface ILedGroup : IEffectTarget<ILedGroup>
    {
        /// <summary>
        /// Gets or sets the brush which should be drawn over this group.
        /// </summary>
        IBrush Brush { get; set; }

        /// <summary>
        /// Gets or sets the z-index of this ledgroup to allow ordering them before drawing. (lowest first) (default: 0)
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// Gets a list containing all LEDs of this group.
        /// </summary>
        /// <returns>The list containing all LEDs of this group.</returns>
        IEnumerable<CorsairLed> GetLeds();
    }
}
