using System;
using System.Runtime.InteropServices;
using CUE.NET.Native;

namespace CUE.NET.Wrapper
{
    public abstract class AbstractCueDevice : ICueDevice
    {
        #region Properties & Fields

        public IDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        protected AbstractCueDevice(IDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        //TODO DarthAffe 19.09.2015: Wrap struct
        protected void SetKeyColors(params _CorsairLedColor[] colors)
        {
            int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
            IntPtr ptr = Marshal.AllocHGlobal(structSize * colors.Length);
            IntPtr addPtr = new IntPtr(ptr.ToInt64());
            foreach (_CorsairLedColor color in colors)
            {
                Marshal.StructureToPtr(color, addPtr, false);
                addPtr = new IntPtr(addPtr.ToInt64() + structSize);
            }
            _CUESDK.CorsairSetLedsColors(colors.Length, ptr);
            Marshal.FreeHGlobal(ptr);
        }

        #endregion
    }
}
