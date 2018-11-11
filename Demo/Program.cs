using CUE.NET;
using CUE.NET.Devices.Mouse;
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
            CorsairMouse mouse = CueSDK.MouseSDK;
            Console.Write("Hello: "+mouse); Console.ReadLine();
        }
    }
}
