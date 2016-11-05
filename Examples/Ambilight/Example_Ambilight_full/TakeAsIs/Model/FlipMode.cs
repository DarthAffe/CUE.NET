using System;

namespace Example_Ambilight_full.TakeAsIs.Model
{
    [Flags]
    public enum FlipMode
    {
        None = 0,
        Vertical = 1 << 0,
        Horizontal = 1 << 1
    }
}
