#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System.Runtime.InteropServices;
using CUE.NET.Enums;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    public class _CorsairLedColor    //  contains information about led and its color
    {

        internal CorsairLedId ledId; // identifier of LED to set
        internal int r;              // red   brightness[0..255]
        internal int g;              // green brightness[0..255]
        internal int b;              // blue  brightness[0..255]
    };
}
