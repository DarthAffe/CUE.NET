#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System;
using System.Runtime.InteropServices;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    internal class _CorsairLedPositions     // contains number of leds and arrays with their positions
    {
        internal int numberOfLed;         // integer value.Number of elements in following array
        internal IntPtr pLedPosition;     // array of led positions
    }
}
