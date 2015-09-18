// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.InteropServices;
using CUE.NET.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Native;

namespace CUE.NET.Wrapper
{
    public static class CueSDK
    {
        #region Properties & Fields

        // ReSharper disable UnusedAutoPropertyAccessor.Global

        public static CorsairProtocolDetails ProtocolDetails { get; private set; }
        public static bool HasExclusiveAccess { get; private set; }
        public static CorsairError LastError => CUESDK.CorsairGetLastError();

        public static CueKeyboard KeyboardSDK { get; private set; }
        public static CueMouse MouseSDK { get; private set; }
        public static CueHeadset HeadsetSDK { get; private set; }

        // ReSharper restore UnusedAutoPropertyAccessor.Global

        #endregion

        #region Methods

        public static void Initialize(bool exclusiveAccess = false)
        {
            if (ProtocolDetails != null)
                throw new WrapperException("CueSDK is already initialized.");

            ProtocolDetails = new CorsairProtocolDetails(CUESDK.CorsairPerformProtocolHandshake());

            CorsairError error = LastError;
            if (error != CorsairError.CE_Success)
                Throw(error);

            if (exclusiveAccess)
            {
                if (!CUESDK.CorsairRequestControl(CorsairAccessMode.CAM_ExclusiveLightingControl))
                    Throw(error);

                HasExclusiveAccess = true;
            }

            int deviceCount = CUESDK.CorsairGetDeviceCount();
            for (int i = 0; i < deviceCount; i++)
            {
                CorsairDeviceInfo info = new CorsairDeviceInfo((_CorsairDeviceInfo)Marshal.PtrToStructure(CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo)));
                switch (info.Type)
                {
                    case CorsairDeviceType.CDT_Keyboard:
                        KeyboardSDK = new CueKeyboard(info);
                        break;
                    case CorsairDeviceType.CDT_Mouse:
                        MouseSDK = new CueMouse(info);
                        break;
                    case CorsairDeviceType.CDT_Headset:
                        HeadsetSDK = new CueHeadset(info);
                        break;

                    // ReSharper disable once RedundantCaseLabel
                    case CorsairDeviceType.CDT_Unknown:
                    default:
                        throw new WrapperException("Unknown Device-Type");
                }

                error = LastError;
                if (error != CorsairError.CE_Success)
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
