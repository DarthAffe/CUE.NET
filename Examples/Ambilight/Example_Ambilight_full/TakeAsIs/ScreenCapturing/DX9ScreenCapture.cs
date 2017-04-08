using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Microsoft.Win32;
using SharpDX;
using SharpDX.Direct3D9;

namespace Example_Ambilight_full.TakeAsIs.ScreenCapturing
{
    public class DX9ScreenCapture : IScreenCapture
    {
        #region Properties & Fields

        private Device _device;
        private Surface _surface;
        private byte[] _buffer;

        public int Width { get; }
        public int Height { get; }
        public PixelFormat PixelFormat => PixelFormats.Bgr24;

        #endregion

        #region Constructors

        public DX9ScreenCapture()
        {
            Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;

            //DarthAffe 08.04.2017: Fix for system using windows-scaling. The primary screen size is reported 'wrong'.
            double scaling = GetScaling();
            if (Math.Abs(scaling - 1.0) > 0.01)
            {
                Width = (int)(Width / scaling);
                Height = (int)(Height / scaling);
            }

            PresentParameters presentParams = new PresentParameters(Width, Height)
            {
                Windowed = true,
                SwapEffect = SwapEffect.Discard
            };

            _device = new Device(new Direct3D(), 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.SoftwareVertexProcessing, presentParams);
            _surface = Surface.CreateOffscreenPlain(_device, Width, Height, Format.A8R8G8B8, Pool.Scratch);
            _buffer = new byte[Width * Height * 4];
        }

        #endregion

        #region Methods

        private double GetScaling()
        {
            try
            {
                int currentDpi = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96);
                return 96.0 / currentDpi;
            }
            catch
            {
                return 1.0;
            }
        }

        public byte[] CaptureScreen()
        {
            _device.GetFrontBufferData(0, _surface);

            DataRectangle dr = _surface.LockRectangle(LockFlags.None);
            Marshal.Copy(dr.DataPointer, _buffer, 0, _buffer.Length);
            _surface.UnlockRectangle();

            return _buffer;
        }

        public void Dispose()
        {
            _device?.Dispose();
            _surface?.Dispose();
        }

        #endregion
    }
}
