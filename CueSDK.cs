// ReSharper disable MemberCanBePrivate.Global

using System.Collections.Generic;
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

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public static string LoadedArchitecture => _CUESDK.LoadedArchitecture;

        /// <summary>
        /// Gets the protocol details for the current SDK-connection.
        /// </summary>
        public static CorsairProtocolDetails ProtocolDetails { get; private set; }

        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public static bool HasExclusiveAccess { get; private set; }

        /// <summary>
        /// Gets the last error documented by CUE.
        /// </summary>
        public static CorsairError LastError => _CUESDK.CorsairGetLastError();

        /// <summary>
        /// Gets the managed representation of a keyboard managed by the CUE-SDK.
        /// Note that currently only one connected keyboard is supported.
        /// </summary>
        public static CorsairKeyboard KeyboardSDK { get; private set; }

        /// <summary>
        /// Gets the managed representation of a mouse managed by the CUE-SDK.
        /// Note that currently only one connected mouse is supported.
        /// </summary>
        public static CorsairMouse MouseSDK { get; private set; }

        /// <summary>
        /// Gets the managed representation of a headset managed by the CUE-SDK.
        /// Note that currently only one connected headset is supported.
        /// </summary>
        public static CorsairHeadset HeadsetSDK { get; private set; }

        // ReSharper restore UnusedAutoPropertyAccessor.Global

        #endregion

        #region Methods

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        /// Initializes the CUE-SDK. This method should be called exactly ONE time, before anything else is done.
        /// </summary>
        /// <param name="exclusiveAccess">Specifies whether the application should request exclusive access or not.</param>
        /// <exception cref="WrapperException">Thrown if the SDK is already initialized, the SDK is not compatible to CUE or if CUE returns unknown devices.</exception>
        /// <exception cref="CUEException">Thrown if the CUE-SDK provides an error.</exception>
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
                    Throw(LastError);

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

        /// <summary>
        /// Reinitialize the CUE-SDK and temporarily hand back full control to CUE.
        /// </summary>
        public static void Reinitialize()
        {
            Reinitialize(HasExclusiveAccess);
        }

        /// <summary>
        /// Reinitialize the CUE-SDK and temporarily hand back full control to CUE.
        /// </summary>
        /// <param name="exclusiveAccess">Specifies whether the application should request exclusive access or not.</param>
        public static void Reinitialize(bool exclusiveAccess)
        {
            if (ProtocolDetails == null)
                throw new WrapperException("CueSDK isn't initialized.");

            KeyboardSDK?.ResetLeds();
            MouseSDK?.ResetLeds();
            HeadsetSDK?.ResetLeds();

            _CUESDK.Reload();

            _CUESDK.CorsairPerformProtocolHandshake();

            CorsairError error = LastError;
            if (error != CorsairError.Success)
                Throw(error);

            if (ProtocolDetails.BreakingChanges)
                throw new WrapperException("The SDK currently used isn't compatible with the installed version of CUE.\r\n" +
                    $"CUE-Version: {ProtocolDetails.ServerVersion} (Protocol {ProtocolDetails.ServerProtocolVersion})\r\n" +
                    $"SDK-Version: {ProtocolDetails.SdkVersion} (Protocol {ProtocolDetails.SdkProtocolVersion})");

            if (exclusiveAccess)
                if (!_CUESDK.CorsairRequestControl(CorsairAccessMode.ExclusiveLightingControl))
                    Throw(LastError);
            HasExclusiveAccess = exclusiveAccess;

            int deviceCount = _CUESDK.CorsairGetDeviceCount();
            Dictionary<CorsairDeviceType, GenericDeviceInfo> reloadedDevices = new Dictionary<CorsairDeviceType, GenericDeviceInfo>();
            for (int i = 0; i < deviceCount; i++)
            {
                GenericDeviceInfo info = new GenericDeviceInfo((_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo)));
                if (!info.CapsMask.HasFlag(CorsairDeviceCaps.Lighting))
                    continue; // Everything that doesn't support lighting control is useless

                reloadedDevices.Add(info.Type, info);

                error = LastError;
                if (error != CorsairError.Success)
                    Throw(error);
            }

            if (KeyboardSDK != null)
                if (!reloadedDevices.ContainsKey(CorsairDeviceType.Keyboard)
                || KeyboardSDK.KeyboardDeviceInfo.Model != reloadedDevices[CorsairDeviceType.Keyboard].Model)
                    throw new WrapperException("The previously loaded Keyboard got disconnected.");
            if (MouseSDK != null)
                if (!reloadedDevices.ContainsKey(CorsairDeviceType.Mouse)
                || MouseSDK.MouseDeviceInfo.Model != reloadedDevices[CorsairDeviceType.Mouse].Model)
                    throw new WrapperException("The previously loaded Mouse got disconnected.");
            if (HeadsetSDK != null)
                if (!reloadedDevices.ContainsKey(CorsairDeviceType.Headset)
                || HeadsetSDK.HeadsetDeviceInfo.Model != reloadedDevices[CorsairDeviceType.Headset].Model)
                    throw new WrapperException("The previously loaded Headset got disconnected.");
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
