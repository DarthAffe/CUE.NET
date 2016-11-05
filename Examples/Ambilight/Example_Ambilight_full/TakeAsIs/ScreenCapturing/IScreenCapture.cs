using System;
using System.Windows.Media;

namespace Example_Ambilight_full.TakeAsIs.ScreenCapturing
{
    public interface IScreenCapture : IDisposable
    {
        int Width { get; }
        int Height { get; }
        PixelFormat PixelFormat { get; }

        /// <summary>
        /// As Pixel-Data BGRA
        /// </summary>
        /// <returns>The Pixel-Data</returns>
        byte[] CaptureScreen();
    }
}
