// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Effects;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a basic brush.
    /// </summary>
    public interface IBrush : IEffectTarget<IBrush>
    {
        /// <summary>
        /// Gets or sets the calculation mode used for the rectangle/points used for color-selection in brushes.
        /// </summary>
        BrushCalculationMode BrushCalculationMode { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage brightness of the brush.
        /// </summary>
        float Brightness { get; set; }

        /// <summary>
        /// Gets or sets the overall percentage opacity of the brush.
        /// </summary>
        float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the gamma-value used to correct the colors calculated by the brush.
        /// Values greater than one will make colors brighter, values less than one will make colors darker. 
        /// </summary>
        float Gamma { get; set; }

        /// <summary>
        /// Gets the Rectangle used in the last render pass.
        /// </summary>
        RectangleF RenderedRectangle { get; }

        /// <summary>
        /// Gets a dictionary containing all colors for points calculated in the last render pass.
        /// </summary>
        Dictionary<BrushRenderTarget, CorsairColor> RenderedTargets { get; }

        /// <summary>
        /// Performas the render pass of the brush and calculates the raw colors for all requested points.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="renderTargets">The targets (keys/points) of which the color should be calculated.</param>
        void PerformRender(RectangleF rectangle, IEnumerable<BrushRenderTarget> renderTargets);

        /// <summary>
        /// Performs the finalize pass of the brush and calculates the final colors for all previously calculated points.
        /// </summary>
        void PerformFinalize();
    }
}