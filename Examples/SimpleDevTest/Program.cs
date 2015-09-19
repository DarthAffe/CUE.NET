using System;
using System.Drawing;
using CUE.NET.Enums;
using CUE.NET.Enums.Keyboard;
using CUE.NET.Exceptions;
using CUE.NET.Wrapper;
using CUE.NET.Wrapper.Keyboard;

namespace SimpleDevTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CueSDK.Initialize();

                CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
                if (keyboard == null)
                    throw new WrapperException("No keyboard found");

                keyboard[CorsairKeyboardKeyId.R].Led.Color = Color.Red;
                keyboard[CorsairKeyboardKeyId.G].Led.Color = Color.Green;
                keyboard[CorsairKeyboardKeyId.B].Led.Color = Color.Blue;

                keyboard[CorsairKeyboardKeyId.W].Led.Color = Color.White;
                keyboard[CorsairKeyboardKeyId.H].Led.Color = Color.White;
                keyboard[CorsairKeyboardKeyId.I].Led.Color = Color.White;
                keyboard[CorsairKeyboardKeyId.T].Led.Color = Color.White;
                keyboard[CorsairKeyboardKeyId.E].Led.Color = Color.White;

                keyboard.UpdateLeds();

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
