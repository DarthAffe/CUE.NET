using CUE.NET.Gradients;

namespace CUE.NET.Brushes
{
    /// <summary>
    /// Represents a basic gradient-brush.
    /// </summary>
    public interface IGradientBrush : IBrush
    {
        /// <summary>
        /// Gets the gradient used by this <see cref="IGradientBrush"/>.
        /// </summary>
        IGradient Gradient { get; }
    }
}
