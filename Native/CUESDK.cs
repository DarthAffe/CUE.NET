using System;
using System.Runtime.InteropServices;
using CUE.NET.Enums;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    internal static class CUESDK
    {
        // set specified leds to some colors. The color is retained until changed by successive calls. This function does not take logical layout into account
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern bool CorsairSetLedsColors(int size, IntPtr ledsColors);

        //[DllImport("CUESDK.x64_2013.dll")]
        //public static extern bool CorsairSetLedsColorsAsync(int size, CorsairLedColor* ledsColors, void(*CallbackType)(void*, bool, CorsairError), void* context);

        // returns number of connected Corsair devices that support lighting control.
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern int CorsairGetDeviceCount();

        // returns information about device at provided index
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern IntPtr CorsairGetDeviceInfo(int deviceIndex);

        // provides list of keyboard LEDs with their physical positions.
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern IntPtr CorsairGetLedPositions();

        // retrieves led id for key name taking logical layout into account.
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern CorsairLedId CorsairGetLedIdForKeyName(char keyName);

        //  requestes control using specified access mode. By default client has shared control over lighting so there is no need to call CorsairRequestControl unless client requires exclusive control
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern bool CorsairRequestControl(CorsairAccessMode accessMode);

        // checks file and protocol version of CUE to understand which of SDK functions can be used with this version of CUE
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern _CorsairProtocolDetails CorsairPerformProtocolHandshake();

        // returns last error that occured while using any of Corsair* functions
        [DllImport("CUESDK.x64_2013.dll")]
        public static extern CorsairError CorsairGetLastError();
    }
}
