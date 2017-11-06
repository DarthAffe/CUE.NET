using CUE.NET.Devices.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using CUE.NET.Devices.Generic.Enums;
using HidLibrary;

namespace CUE.NET.Devices.Mouse
{
    public class GlaiveMouse : CorsairMouse
    {
        private const int vid = 0x1b1c;
        private const int pid = 0x1b34;

        private HidDevice dev;

        private Color bars;
        private Color front;
        private Color logo;

        private bool initialized = false;

        public GlaiveMouse(CorsairMouseDeviceInfo info) : base(info)
        { }

        public static GlaiveMouse FromCorsairMouse(CorsairMouse mouse)
        {
            return new GlaiveMouse(mouse.MouseDeviceInfo);
        }

        public override void Initialize()
        {
            dev = HidDevices.Enumerate(vid, pid).Where(s=>s.DevicePath.Contains(@"&mi_02#")).Single();

            initialized = true;

            base.Initialize();
        }

        protected override void UpdateLeds(ICollection<LedUpateRequest> updateRequests)
        {
            updateRequests = updateRequests.Where(x => x.Color != CorsairColor.Transparent).ToList();

            OnLedsUpdating(updateRequests);

            if (updateRequests.Any())
            {
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
                buff[5] = 3;
                buff[7] = 255;
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

                dev.Write(buff);
            }
            else throw new Exception("not initialized");
        }
    }
}
