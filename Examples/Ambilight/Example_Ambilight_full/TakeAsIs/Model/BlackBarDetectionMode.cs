using System;

namespace Example_Ambilight_full.TakeAsIs.Model
{
    [Flags]
    public enum BlackBarDetectionMode
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Top = 1 << 2,
        Bottom = 1 << 3,
    }
}
