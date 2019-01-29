using CUE.NET;
using CUE.NET.Devices.CommanderPro;
using CUE.NET.Devices.HeadsetStand;
using System;
using System.Collections.Generic;
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
            CueSDK.Initialize(true);
            CorsairCommanderPro cooler = CueSDK.CommanderProSDK;
            Console.Write("Cooler: "+cooler); Console.ReadLine();
            CorsairHeadsetStand headset = CueSDK.HeadsetStandSDK;
            Console.Write("HeadSet: " + headset); Console.ReadLine();
        }
    }
}
