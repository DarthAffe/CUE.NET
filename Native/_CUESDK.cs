using System;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard.Enums;

namespace CUE.NET.Native
{
    // ReSharper disable once InconsistentNaming
    internal static class _CUESDK
    {
        #region Libary Management

        internal static string LoadedArchitecture { get; private set; }

        static _CUESDK()
        {
            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            LoadLibrary((LoadedArchitecture = Environment.Is64BitProcess ? "x64" : "x86") + "/CUESDK_2013.dll");
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        #endregion

        #region SDK-IMPORTS

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // set specified leds to some colors. The color is retained until changed by successive calls. This function does not take logical layout into account
        internal static extern bool CorsairSetLedsColors(int size, IntPtr ledsColors);

        //#if WIN64
        //        [DllImport("CUESDK.x64_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        //#else
        //        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        //#endif
        //internal static extern bool CorsairSetLedsColorsAsync(int size, CorsairLedColor* ledsColors, void(*CallbackType)(void*, bool, CorsairError), void* context);

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // returns number of connected Corsair devices that support lighting control.
        internal static extern int CorsairGetDeviceCount();

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // returns information about device at provided index
        internal static extern IntPtr CorsairGetDeviceInfo(int deviceIndex);

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // provides list of keyboard LEDs with their physical positions.
        internal static extern IntPtr CorsairGetLedPositions();

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // retrieves led id for key name taking logical layout into account.
        internal static extern CorsairKeyboardKeyId CorsairGetLedIdForKeyName(char keyName);

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        //  requestes control using specified access mode. By default client has shared control over lighting so there is no need to call CorsairRequestControl unless client requires exclusive control
        internal static extern bool CorsairRequestControl(CorsairAccessMode accessMode);

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // checks file and protocol version of CUE to understand which of SDK functions can be used with this version of CUE
        internal static extern _CorsairProtocolDetails CorsairPerformProtocolHandshake();

        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        // returns last error that occured while using any of Corsair* functions
        internal static extern CorsairError CorsairGetLastError();

        #endregion
    }
}
