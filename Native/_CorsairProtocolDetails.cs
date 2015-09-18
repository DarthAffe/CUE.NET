#pragma warning disable 169 // Field 'x' is never used
#pragma warning disable 414 // Field 'x' is assigned but its value never used
#pragma warning disable 649 // Field 'x' is never assigned

using System;
using System.Runtime.InteropServices;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    [StructLayout(LayoutKind.Sequential)]
    internal struct _CorsairProtocolDetails // contains information about SDK and CUE versions
    {
        internal IntPtr sdkVersion;         // null - terminated string containing version of SDK(like “1.0.0.1”). Always contains valid value even if there was no CUE found
        internal IntPtr serverVersion;      // null - terminated string containing version of CUE(like “1.0.0.1”) or NULL if CUE was not found.
        internal int sdkProtocolVersion;    // integer number that specifies version of protocol that is implemented by current SDK. Numbering starts from 1. Always contains valid value even if there was no CUE found
        internal int serverProtocolVersion; // integer number that specifies version of protocol that is implemented by CUE. Numbering starts from 1. If CUE was not found then this value will be 0
        internal byte breakingChanges;      // boolean value that specifies if there were breaking changes between version of protocol implemented by server and client
    };
}
