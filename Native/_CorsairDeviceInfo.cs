#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System;
using System.Runtime.InteropServices;
using CUE.NET.Enums;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    public class _CorsairDeviceInfo	                   // contains information about device
    {
        internal CorsairDeviceType type;               // enum describing device type
        internal IntPtr model;                         // null - terminated device model(like “K95RGB”)
        internal int physicalLayout;                   // enum describing physical layout of the keyboard or mouse
        internal int logicalLayout;                    // enum describing logical layout of the keyboard as set in CUE settings
        internal int capsMask;					       // mask that describes device capabilities, formed as logical “or” of CorsairDeviceCaps enum values
    }
}
