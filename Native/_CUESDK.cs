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

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
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

        /// <summary>
        /// CUE-SDK: set specified leds to some colors. The color is retained until changed by successive calls. This function does not take logical layout into account
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool CorsairSetLedsColors(int size, IntPtr ledsColors);

        //#if WIN64
        //        [DllImport("CUESDK.x64_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        //#else
        //        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        //#endif
        //internal static extern bool CorsairSetLedsColorsAsync(int size, CorsairLedColor* ledsColors, void(*CallbackType)(void*, bool, CorsairError), void* context);

        /// <summary>
        /// CUE-SDK: returns number of connected Corsair devices that support lighting control.
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CorsairGetDeviceCount();

        /// <summary>
        /// CUE-SDK: returns information about device at provided index
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr CorsairGetDeviceInfo(int deviceIndex);

        /// <summary>
        /// CUE-SDK: provides list of keyboard LEDs with their physical positions.
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr CorsairGetLedPositions();

        /// <summary>
        /// CUE-SDK: retrieves led id for key name taking logical layout into account.
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern CorsairKeyboardKeyId CorsairGetLedIdForKeyName(char keyName);

        /// <summary>
        /// CUE-SDK: requestes control using specified access mode.
        /// By default client has shared control over lighting so there is no need to call CorsairRequestControl unless client requires exclusive control
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool CorsairRequestControl(CorsairAccessMode accessMode);

        /// <summary>
        /// CUE-SDK: checks file and protocol version of CUE to understand which of SDK functions can be used with this version of CUE
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern _CorsairProtocolDetails CorsairPerformProtocolHandshake();

        /// <summary>
        /// CUE-SDK: returns last error that occured while using any of Corsair* functions
        /// </summary>
        [DllImport("CUESDK_2013.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern CorsairError CorsairGetLastError();

        #endregion
    }
}
