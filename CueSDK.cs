// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Headset;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Mouse;
using CUE.NET.Exceptions;
using CUE.NET.Native;

namespace CUE.NET
{
    public static class CueSDK
    {
        #region Properties & Fields

        // ReSharper disable UnusedAutoPropertyAccessor.Global

        public static string LoadedArchitecture => _CUESDK.LoadedArchitecture;
        public static CorsairProtocolDetails ProtocolDetails { get; private set; }
        public static bool HasExclusiveAccess { get; private set; }
        public static CorsairError LastError => _CUESDK.CorsairGetLastError();

        public static CorsairKeyboard KeyboardSDK { get; private set; }
        public static CorsairMouse MouseSDK { get; private set; }
        public static CorsairHeadset HeadsetSDK { get; private set; }

        // ReSharper restore UnusedAutoPropertyAccessor.Global

        #endregion

        #region Methods

        public static void Initialize(bool exclusiveAccess = false)
        {
            if (ProtocolDetails != null)
                throw new WrapperException("CueSDK is already initialized.");

            ProtocolDetails = new CorsairProtocolDetails(_CUESDK.CorsairPerformProtocolHandshake());

            CorsairError error = LastError;
            if (error != CorsairError.Success)
                Throw(error);

            if (ProtocolDetails.BreakingChanges)
                throw new WrapperException("The SDK currently used isn't compatible with the installed version of CUE.\r\n" +
                    $"CUE-Version: {ProtocolDetails.ServerVersion} (Protocol {ProtocolDetails.ServerProtocolVersion})\r\n" +
                    $"SDK-Version: {ProtocolDetails.SdkVersion} (Protocol {ProtocolDetails.SdkProtocolVersion})");

            if (exclusiveAccess)
            {
                if (!_CUESDK.CorsairRequestControl(CorsairAccessMode.ExclusiveLightingControl))
                    Throw(error);

                HasExclusiveAccess = true;
            }

            int deviceCount = _CUESDK.CorsairGetDeviceCount();
            for (int i = 0; i < deviceCount; i++)
            {
                _CorsairDeviceInfo nativeDeviceInfo = (_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo));
                GenericDeviceInfo info = new GenericDeviceInfo(nativeDeviceInfo);
                if (!info.CapsMask.HasFlag(CorsairDeviceCaps.Lighting))
                    continue; // Everything that doesn't support lighting control is useless

                switch (info.Type)
                {
                    case CorsairDeviceType.Keyboard:
                        KeyboardSDK = new CorsairKeyboard(new CorsairKeyboardDeviceInfo(nativeDeviceInfo));
                        break;
                    case CorsairDeviceType.Mouse:
                        MouseSDK = new CorsairMouse(new CorsairMouseDeviceInfo(nativeDeviceInfo));
                        break;
                    case CorsairDeviceType.Headset:
                        HeadsetSDK = new CorsairHeadset(new CorsairHeadsetDeviceInfo(nativeDeviceInfo));
                        break;

                    // ReSharper disable once RedundantCaseLabel
                    case CorsairDeviceType.Unknown:
                    default:
                        throw new WrapperException("Unknown Device-Type");
                }

                error = LastError;
                if (error != CorsairError.Success)
                    Throw(error);
            }
        }

        private static void Throw(CorsairError error)
        {
            ProtocolDetails = null;
            HasExclusiveAccess = false;
            KeyboardSDK = null;
            MouseSDK = null;
            HeadsetSDK = null;

            throw new CUEException(error);
        }

        #endregion
    }
}
