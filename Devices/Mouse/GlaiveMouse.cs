using CUE.NET.Devices.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidSharp;
using System.Drawing;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Mouse.Enums;
using CUE.NET.Exceptions;

namespace CUE.NET.Devices.Mouse
{
    public class GlaiveMouse : AbstractCueDevice
    {
        private const int vid = 0x1b1c;
        private const int pid = 0x1b34;

        private HidDevice dev;
        private HidStream stream;

        private Color bars;
        private Color front;
        private Color logo;

        private bool initialized = false;

        public CorsairMouseDeviceInfo MouseDeviceInfo { get; }

        public GlaiveMouse(CorsairMouseDeviceInfo info) : base(info)
        {
            this.MouseDeviceInfo = info;
        }

        public static GlaiveMouse FromCorsairMouse(CorsairMouse mouse)
        {
            return new GlaiveMouse(mouse.MouseDeviceInfo);
        }

        public override void Initialize()
        {
            var loader = new HidDeviceLoader();
            dev = loader.GetDeviceOrDefault(vid, pid);
            if (!dev.TryOpen(out stream)) throw new Exception("Glaive mouse init error!");

            initialized = true;

            switch (MouseDeviceInfo.PhysicalLayout)
            {
                case CorsairPhysicalMouseLayout.Zones1:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    InitializeLed(CorsairMouseLedId.B1, new RectangleF(0, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B2, new RectangleF(1, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B3, new RectangleF(2, 0, 1, 1));
                    InitializeLed(CorsairMouseLedId.B4, new RectangleF(3, 0, 1, 1));
                    break;
                default:
                    throw new WrapperException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }

            base.Initialize();
        }

        protected override void UpdateLeds(ICollection<LedUpateRequest> updateRequests)
        {
            updateRequests = updateRequests.Where(x => x.Color != CorsairColor.Transparent).ToList();

            OnLedsUpdating(updateRequests);

            if (updateRequests.Any()) // CUE seems to crash if 'CorsairSetLedsColors' is called with a zero length array
            {
                //int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
                //IntPtr ptr = Marshal.AllocHGlobal(structSize * updateRequests.Count);
                //IntPtr addPtr = new IntPtr(ptr.ToInt64());
                //foreach (LedUpateRequest ledUpdateRequest in updateRequests)
                //{
                //    _CorsairLedColor color = new _CorsairLedColor
                //    {
                //        ledId = (int)ledUpdateRequest.LedId,
                //        r = ledUpdateRequest.Color.R,
                //        g = ledUpdateRequest.Color.G,
                //        b = ledUpdateRequest.Color.B
                //    };

                //    Marshal.StructureToPtr(color, addPtr, false);
                //    addPtr = new IntPtr(addPtr.ToInt64() + structSize);
                //}
                //_CUESDK.CorsairSetLedsColors(updateRequests.Count, ptr);
                //Marshal.FreeHGlobal(ptr);

                foreach (LedUpateRequest ledUpdateRequest in updateRequests)
                {
                    switch(ledUpdateRequest.LedId)
                    {
                        case CorsairLedId.B1:
                            logo = Color.FromArgb(ledUpdateRequest.Color.R, ledUpdateRequest.Color.G, ledUpdateRequest.Color.B);
                            break;
                        case CorsairLedId.B2:
                            front = Color.FromArgb(ledUpdateRequest.Color.R, ledUpdateRequest.Color.G, ledUpdateRequest.Color.B);
                            break;
                        case CorsairLedId.B3:
                            bars = Color.FromArgb(ledUpdateRequest.Color.R, ledUpdateRequest.Color.G, ledUpdateRequest.Color.B);
                            break;
                        default:
                            break;
                    }
                }

                HidUpdate();
            }

            OnLedsUpdated(updateRequests);
        }

        private void HidUpdate()
        {
            if (initialized)
            {
                byte[] buff = new byte[65];
                buff[1] = 7;
                buff[2] = 34;
                buff[3] = 4;
                buff[4] = 1;



                //dpi indicator (no idea why but this crap doesnt work)
                buff[5] = 3;
                //if (dpiIndicator == 1 || dpiIndicator == 2 || dpiIndicator == 4)
                //buff[6] = 255;
                //if (dpiIndicator >= 2)
                buff[7] = 255;
                //if (dpiIndicator == 2 || dpiIndicator == 5)
                buff[8] = 255;

                //bars rgb
                buff[9] = 6;
                buff[10] = bars.R;
                buff[11] = bars.G;
                buff[12] = bars.B;

                //front rgb
                buff[13] = 1;
                buff[14] = front.R;
                buff[15] = front.G;
                buff[16] = front.B;

                //logo rgb
                buff[17] = 2;
                buff[18] = logo.R;
                buff[19] = logo.G;
                buff[20] = logo.B;

                stream.Write(buff);
            }
            else throw new Exception("not initialized");
        }
    }
}
