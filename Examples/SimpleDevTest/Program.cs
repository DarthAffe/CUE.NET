using System;
using System.Drawing;
using CUE.NET;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Exceptions;

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

                RectangleKeyGroup centerGroup = new RectangleKeyGroup(keyboard, CorsairKeyboardKeyId.Keypad7, CorsairKeyboardKeyId.Keypad3);
                centerGroup.SetColor(Color.Purple);

                keyboard[CorsairKeyboardKeyId.R].Led.Color = Color.Red;
                keyboard[CorsairKeyboardKeyId.G].Led.Color = Color.Green;
                keyboard[CorsairKeyboardKeyId.B].Led.Color = Color.Blue;

                SimpleKeyGroup whiteGroup = new SimpleKeyGroup(keyboard, CorsairKeyboardKeyId.W, CorsairKeyboardKeyId.H, CorsairKeyboardKeyId.I, CorsairKeyboardKeyId.T, CorsairKeyboardKeyId.E);
                whiteGroup.SetColor(Color.White);

                RectangleKeyGroup numberGroup = new RectangleKeyGroup(keyboard, CorsairKeyboardKeyId.D1, CorsairKeyboardKeyId.D0);
                numberGroup.SetColor(Color.Yellow);

                keyboard.UpdateLeds(true);

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
