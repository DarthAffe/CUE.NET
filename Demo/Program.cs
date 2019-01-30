using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.CommanderPro;
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
            CorsairCommanderPro commander = CueSDK.CommanderProSDK;
            CueSDK.CommanderProSDK.Brush = (SolidColorBrush)Color.Black;
            ILedGroup rainbowLeds = new ListLedGroup(CueSDK.CommanderProSDK, CueSDK.CommanderProSDK);
            rainbowLeds.Brush = new LinearGradientBrush(new RainbowGradient());
            CueSDK.CommanderProSDK.Update();
            Console.Write("CommanderPro: " + commander); Console.ReadLine();
        }
    }
}
