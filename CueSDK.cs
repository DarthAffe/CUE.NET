// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using CUE.NET.Devices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Headset;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Mouse;
using CUE.NET.Devices.Mousemat;
using CUE.NET.Exceptions;
using CUE.NET.Native;

namespace CUE.NET
{
    /// <summary>
    /// Static entry point to work with the Corsair-SDK.
    /// </summary>
    public static partial class CueSDK
    {
        #region Properties & Fields

        // ReSharper disable UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public static bool IsInitialized { get; private set; }

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
        /// Gets all initialized devices managed by the CUE-SDK.
        /// </summary>
        public static IEnumerable<ICueDevice> InitializedDevices { get; private set; }

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

        /// <summary>
        /// Gets the managed representation of a moustmat managed by the CUE-SDK.
        /// Note that currently only one connected mousemat is supported.
        /// </summary>
        public static CorsairMousemat MousematSDK { get; private set; }

        // ReSharper restore UnusedAutoPropertyAccessor.Global

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the SDK for the provided <see cref="CorsairDeviceType"/> is available or checks if CUE is installed and SDK supported enabled if null is provided.
        /// </summary>
        /// <param name="sdkType">The <see cref="CorsairDeviceType"/> to check or null to check for general SDK availability.</param>
        /// <returns>The availability of the provided <see cref="CorsairDeviceType"/>.</returns>
        public static bool IsSDKAvailable(CorsairDeviceType? sdkType = null)
        {
            try
            {
                // ReSharper disable once RedundantIfElseBlock
                if (IsInitialized)
                {
                    // ReSharper disable once SwitchStatementMissingSomeCases - everything else is true
                    switch (sdkType)
                    {
                        case CorsairDeviceType.Keyboard:
                            return KeyboardSDK != null;
                        case CorsairDeviceType.Mouse:
                            return MouseSDK != null;
                        case CorsairDeviceType.Headset:
                            return HeadsetSDK != null;
                        case CorsairDeviceType.Mousemat:
                            return MousematSDK != null;
                        default:
                            return true;
                    }
                }
                else
                {
                    _CUESDK.CorsairPerformProtocolHandshake();

                    if (sdkType == null || sdkType == CorsairDeviceType.Unknown)
                        return LastError == CorsairError.Success;

                    int deviceCount = _CUESDK.CorsairGetDeviceCount();
                    for (int i = 0; i < deviceCount; i++)
                    {
                        GenericDeviceInfo info = new GenericDeviceInfo((_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo)));
                        if (info.CapsMask.HasFlag(CorsairDeviceCaps.Lighting) && info.Type == sdkType.Value)
                            return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        // ReSharper disable once ExceptionNotThrown
        /// <summary>
        /// Initializes the CUE-SDK. This method should be called exactly ONE time, before anything else is done.
        /// </summary>
        /// <param name="exclusiveAccess">Specifies whether the application should request exclusive access or not.</param>
        /// <exception cref="WrapperException">Thrown if the SDK is already initialized, the SDK is not compatible to CUE or if CUE returns unknown devices.</exception>
        /// <exception cref="CUEException">Thrown if the CUE-SDK provides an error.</exception>
        public static void Initialize(bool exclusiveAccess = false)
        {
            if (IsInitialized)
                throw new WrapperException("CueSDK is already initialized.");

            _CUESDK.Reload();

            ProtocolDetails = new CorsairProtocolDetails(_CUESDK.CorsairPerformProtocolHandshake());

            CorsairError error = LastError;
            if (error != CorsairError.Success)
                Throw(error);

            if (ProtocolDetails.BreakingChanges)
                throw new WrapperException("The SDK currently used isn't compatible with the installed version of CUE.\r\n"
                    + $"CUE-Version: {ProtocolDetails.ServerVersion} (Protocol {ProtocolDetails.ServerProtocolVersion})\r\n"
                    + $"SDK-Version: {ProtocolDetails.SdkVersion} (Protocol {ProtocolDetails.SdkProtocolVersion})");

            if (exclusiveAccess)
            {
                if (!_CUESDK.CorsairRequestControl(CorsairAccessMode.ExclusiveLightingControl))
                    Throw(LastError);

                HasExclusiveAccess = true;
            }

            IList<ICueDevice> devices = new List<ICueDevice>();
            int deviceCount = _CUESDK.CorsairGetDeviceCount();
            for (int i = 0; i < deviceCount; i++)
            {
                _CorsairDeviceInfo nativeDeviceInfo = (_CorsairDeviceInfo)Marshal.PtrToStructure(_CUESDK.CorsairGetDeviceInfo(i), typeof(_CorsairDeviceInfo));
                GenericDeviceInfo info = new GenericDeviceInfo(nativeDeviceInfo);
                if (!info.CapsMask.HasFlag(CorsairDeviceCaps.Lighting))
                    continue; // Everything that doesn't support lighting control is useless

                ICueDevice device;
                switch (info.Type)
                {
                    case CorsairDeviceType.Keyboard:
                        device = KeyboardSDK = new CorsairKeyboard(new CorsairKeyboardDeviceInfo(nativeDeviceInfo));
                        break;
                    case CorsairDeviceType.Mouse:
                        device = MouseSDK = new CorsairMouse(new CorsairMouseDeviceInfo(nativeDeviceInfo));
                        break;
                    case CorsairDeviceType.Headset:
                        device = HeadsetSDK = new CorsairHeadset(new CorsairHeadsetDeviceInfo(nativeDeviceInfo));
                        break;
                    case CorsairDeviceType.Mousemat:
                        device = MousematSDK = new CorsairMousemat(new CorsairMousematDeviceInfo(nativeDeviceInfo));
                        break;
                    // ReSharper disable once RedundantCaseLabel
                    case CorsairDeviceType.Unknown:
                    default:
                        throw new WrapperException("Unknown Device-Type");
                }

                device.Initialize();
                devices.Add(device);

                error = LastError;
                if (error != CorsairError.Success)
                    Throw(error);
            }

            InitializedDevices = new ReadOnlyCollection<ICueDevice>(devices);

            IsInitialized = true;
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
            if (!IsInitialized)
                throw new WrapperException("CueSDK isn't initialized.");

            KeyboardSDK?.ResetLeds();
            MouseSDK?.ResetLeds();
            HeadsetSDK?.ResetLeds();
            MousematSDK?.ResetLeds();

            _CUESDK.Reload();

            _CUESDK.CorsairPerformProtocolHandshake();

            CorsairError error = LastError;
            if (error != CorsairError.Success)
                Throw(error);

            if (ProtocolDetails.BreakingChanges)
                throw new WrapperException("The SDK currently used isn't compatible with the installed version of CUE.\r\n"
                    + $"CUE-Version: {ProtocolDetails.ServerVersion} (Protocol {ProtocolDetails.ServerProtocolVersion})\r\n"
                    + $"SDK-Version: {ProtocolDetails.SdkVersion} (Protocol {ProtocolDetails.SdkProtocolVersion})");

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
            if (MousematSDK != null)
                if (!reloadedDevices.ContainsKey(CorsairDeviceType.Mousemat)
                    || MousematSDK.MousematDeviceInfo.Model != reloadedDevices[CorsairDeviceType.Mousemat].Model)
                    throw new WrapperException("The previously loaded Mousemat got disconnected.");

            IsInitialized = true;
        }

        private static void Throw(CorsairError error)
        {
            ProtocolDetails = null;
            HasExclusiveAccess = false;
            KeyboardSDK = null;
            MouseSDK = null;
            HeadsetSDK = null;
            MousematSDK = null;
            IsInitialized = false;

            throw new CUEException(error);
        }

        #endregion
    }
}
