using System;
using System.Drawing;
using CUE.NET.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Wrapper;

namespace SimpleDevTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CueSDK.Initialize();

                CueKeyboard keyboard = CueSDK.KeyboardSDK;
                if (keyboard == null)
                    throw new WrapperException("No keyboard found");

                keyboard.SetKeyColor('r', Color.Red);
                keyboard.SetKeyColor('g', Color.Green);
                keyboard.SetKeyColor('b', Color.Blue);

                keyboard.SetKeyColors(new[] { 'w', 'h', 'i', 't', 'e' }, Color.White);

                Console.WriteLine(CueSDK.LastError);
            }
            catch (CUEException ex)
            {
                Console.WriteLine("CUE Exception! ErrorCode: " + Enum.GetName(typeof(CorsairError), ex.Error));
            }
            catch (WrapperException ex)
            {
                Console.WriteLine("Wrapper Exception! Message:" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception! Message:" + ex.Message);
            }

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }
    }
}
