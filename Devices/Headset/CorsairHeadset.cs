// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Drawing;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Headset.Enums;

namespace CUE.NET.Devices.Headset
{
    /// <summary>
    /// Represents the SDK for a corsair headset.
    /// </summary>
    public class CorsairHeadset : AbstractCueDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets specific information provided by CUE for the headset.
        /// </summary>
        public CorsairHeadsetDeviceInfo HeadsetDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairHeadset"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the headset</param>
        internal CorsairHeadset(CorsairHeadsetDeviceInfo info)
            : base(info)
        {
            this.HeadsetDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the the headset.
        /// </summary>
        public override void Initialize()
        {
            InitializeLed(CorsairHeadsetLedId.LeftLogo, new RectangleF(0, 0, 1, 1));
            InitializeLed(CorsairHeadsetLedId.RightLogo, new RectangleF(1, 0, 1, 1));

            base.Initialize();
        }

        #endregion
    }
}
