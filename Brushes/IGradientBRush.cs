using CUE.NET.Gradients;

namespace CUE.NET.Brushes
{
    public interface IGradientBrush : IBrush
    {
        IGradient Gradient { get; }
    }
}
