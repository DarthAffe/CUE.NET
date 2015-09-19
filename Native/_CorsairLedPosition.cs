#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System.Runtime.InteropServices;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    public class _CorsairLedPosition // contains led id and position of led rectangle.Most of the keys are rectangular. In case if key is not rectangular(like Enter in ISO / UK layout) it returns the smallest rectangle that fully contains the key
    {
        internal int ledId;          // identifier of led
        internal double top;
        internal double left;
        internal double height;
        internal double width;   	 // values in mm
    }
}
