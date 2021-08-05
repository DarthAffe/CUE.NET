using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.LightingNodePro;
using CUE.NET.Devices.HeadsetStand;
using CUE.NET.Gradients;
using CUE.NET.Groups;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            CueSDK.IsSDKAvailable();
            CueSDK.Initialize(false);
            CorsairLightingNodePro commander = CueSDK.LightingNodeProSDK;
            CueSDK.LightingNodeProSDK.Brush = (SolidColorBrush)Color.Black;
            ILedGroup rainbowLeds = new ListLedGroup(CueSDK.LightingNodeProSDK, CueSDK.LightingNodeProSDK);
            rainbowLeds.Brush = new LinearGradientBrush(new RainbowGradient());
            CueSDK.LightingNodeProSDK.Update();
            Console.Write("LightingNodePro: " + commander); Console.ReadLine();
        }
    }
}
