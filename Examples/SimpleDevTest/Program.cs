using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using CUE.NET;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Brushes;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Devices.Keyboard.Extensions;
using CUE.NET.Devices.Keyboard.Keys;
using CUE.NET.Exceptions;

namespace SimpleDevTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit ...");
            Console.WriteLine();
            Task.Factory.StartNew(
                () =>
                {
                    Console.ReadKey();
                    Environment.Exit(0);
                });

            try
            {
                // Initialize CUE-SDK
                CueSDK.Initialize();

                // Get connected keyboard or throw exception if there is no light controllable keyboard connected
                CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
                if (keyboard == null)
                    throw new WrapperException("No keyboard found");


                // ---------------------------------------------------------------------------
                // First we'll look at some basic coloring

                Console.WriteLine("Basic color-test ...");
                // Ink all numbers on the keypad except the '5' purple, we want that to be gray
                ListKeyGroup purpleGroup = new RectangleKeyGroup(keyboard, CorsairKeyboardKeyId.Keypad7, CorsairKeyboardKeyId.Keypad3)
                { Brush = new SolidColorBrush(Color.Purple) }
                .Exclude(CorsairKeyboardKeyId.Keypad5);
                keyboard[CorsairKeyboardKeyId.Keypad5].Led.Color = Color.Gray;

                // Ink the Keys 'r', 'g', 'b' in their respective color
                // The char access fails for everything except letters (SDK doesn't return a valid keyId)
                keyboard['R'].Led.Color = Color.Red;
                keyboard[CorsairKeyboardKeyId.G].Led.Color = Color.Green;
                keyboard['B'].Led.Color = Color.Blue;

                // Lock the 'r', 'g', 'b' keys. We want them to stay like this forever (commented since it looks quite stupid later, but feel free tu uncomment this)
                //keyboard['R'].Led.IsLocked = true;
                //keyboard['G'].Led.IsLocked = true;
                //keyboard['B'].Led.IsLocked = true;

                // Ink the letters of 'white' white
                ListKeyGroup whiteGroup = new ListKeyGroup(keyboard, CorsairKeyboardKeyId.W, CorsairKeyboardKeyId.H, CorsairKeyboardKeyId.I, CorsairKeyboardKeyId.T, CorsairKeyboardKeyId.E)
                { Brush = new SolidColorBrush(Color.White) };

                // Ink the keys '1' to '0' yellow
                RectangleKeyGroup yellowGroup = new RectangleKeyGroup(keyboard, CorsairKeyboardKeyId.D1, CorsairKeyboardKeyId.D0)
                { Brush = new SolidColorBrush(Color.Yellow) };

                // Update the keyboard to show the configured colors, (your CUE settings defines the rest)
                keyboard.UpdateLeds();

                Wait(3);

                // Remove all the groups we created above to clear the keyboard
                purpleGroup.Detach();
                whiteGroup.Detach();
                yellowGroup.Detach();


                // ---------------------------------------------------------------------------
                // Next we add a nice linear gradient brush over the keyboard and play around with the offset of one stop

                Console.WriteLine("gradient-brush-test");

                // Create our gradient stop to play with
                GradientStop moveableStop = new GradientStop(0, Color.FromArgb(0, 255, 0));

                // Create a basic (by default horizontal) brush ...
                LinearGradientBrush linearBrush = new LinearGradientBrush(new GradientStop(0, Color.Blue), moveableStop, new GradientStop(1f, Color.White));

                // ... and add it as the keyboard background
                keyboard.Brush = linearBrush;

                // Move the brush from left to right
                for (float offset = 0; offset <= 1f; offset += 0.02f)
                {
                    moveableStop.Offset = offset;
                    keyboard.UpdateLeds();
                    Thread.Sleep(100);
                }

                // And back to center
                for (float offset = 1f; offset >= 0.5f; offset -= 0.02f)
                {
                    moveableStop.Offset = offset;
                    keyboard.UpdateLeds();
                    Thread.Sleep(100);
                }

                // "Rotate" the brush (this is of course not the best implementation for this but you see the point)
                for (float rotateX = 0, rotateY = 0; rotateX <= 1f; rotateX += 0.02f, rotateY = 0.04f)
                {
                    if (rotateY > 1f)
                        rotateY = 1f - (rotateY - 1f);

                    linearBrush.StartPoint = new PointF(rotateX, rotateY);
                    linearBrush.EndPoint = new PointF(1f - rotateX, 1f - rotateY);

                    keyboard.UpdateLeds();
                    Thread.Sleep(100);
                }

                Wait(2);

                // ---------------------------------------------------------------------------
                // Time for an even better brush: rainbow

                Console.WriteLine("rainbow-test");

                // Create an simple horizontal rainbow containing two times the full spectrum
                RainbowBrush rainbowBrush = new RainbowBrush(0, 720);

                // Add the rainbow to the keyboard and perform an initial update
                keyboard.Brush = rainbowBrush;
                keyboard.UpdateLeds();

                // Let the rainbow move around for 10 secs
                for (int i = 0; i < 100; i++)
                {
                    rainbowBrush.StartHue += 10f;
                    rainbowBrush.EndHue += 10f;
                    keyboard.UpdateLeds();
                    Thread.Sleep(100);
                }

                Wait(2);


                // ---------------------------------------------------------------------------
                // Now let us move some points random over the keyboard
                // Something like this could become some sort of effect

                // Initialize needed stuff
                const float SPEED = 6f; // mm/tick
                Random random = new Random();

                // Flash whole keyboard three times to ... well ... just to make it happen
                for (int i = 0; i < 3; i++)
                {
                    keyboard.Brush = new SolidColorBrush(Color.Aquamarine);
                    keyboard.UpdateLeds();
                    Thread.Sleep(160);
                    keyboard.Brush = new SolidColorBrush(Color.Black);
                    keyboard.UpdateLeds();
                    Thread.Sleep(200);
                }

                // Set keyboard 'background' to black with low alpha (this will add a nice "fade" effect instead of just clearing the keyboard every frame)
                keyboard.Brush = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));

                // Define how many points we have
                const int NUM_POINTS = 6;

                // The points we want to draw (rectangle since circles are too hard to calculate :p)
                RectangleF[] points = new RectangleF[NUM_POINTS];

                // KeyGroups which represents our point on the keyboard
                RectangleKeyGroup[] pointGroups = new RectangleKeyGroup[NUM_POINTS];

                // Target of our movement
                PointF[] targets = new PointF[NUM_POINTS];

                // Initialize all the stuff
                for (int i = 0; i < NUM_POINTS; i++)
                {
                    // Spawn our point  in the top-left corner (right over G1 or on ESC depending on your keyboard)
                    points[i] = new RectangleF(keyboard.KeyboardRectangle.X, keyboard.KeyboardRectangle.Y, 60, 60);
                    pointGroups[i] = new RectangleKeyGroup(keyboard, points[i], 0.1f) { Brush = new SolidColorBrush(Color.White) };
                    targets[i] = new PointF(points[i].X, points[i].Y);
                }

                // We set colors manually since white points are kinda boring (notice, that we use alpha values)
                pointGroups[0].Brush = new SolidColorBrush(Color.FromArgb(127, 255, 0, 0));
                pointGroups[1].Brush = new SolidColorBrush(Color.FromArgb(127, 0, 255, 0));
                pointGroups[2].Brush = new SolidColorBrush(Color.FromArgb(127, 0, 0, 255));
                pointGroups[3].Brush = new SolidColorBrush(Color.FromArgb(127, 255, 0, 255));
                pointGroups[4].Brush = new SolidColorBrush(Color.FromArgb(127, 255, 255, 0));
                pointGroups[5].Brush = new SolidColorBrush(Color.FromArgb(127, 0, 255, 255));

                while (true)
                {
                    // Calculate all the points
                    for (int i = 0; i < NUM_POINTS; i++)
                    {
                        // Choose new target if we arrived
                        if (points[i].Contains(targets[i]))
                            targets[i] = new PointF((float)(keyboard.KeyboardRectangle.X + (random.NextDouble() * keyboard.KeyboardRectangle.Width)),
                                    (float)(keyboard.KeyboardRectangle.Y + (random.NextDouble() * keyboard.KeyboardRectangle.Height)));
                        else
                            // Calculate movement
                            points[i].Location = Interpolate(points[i].Location, targets[i], SPEED); // It would be better to calculate from the center of our rectangle but the easy way is enough here

                        // Move our rectangle to the new position
                        pointGroups[i].Rectangle = points[i];
                    }

                    // Update changed leds
                    keyboard.UpdateLeds();

                    // 20 updates per sec should be enought for this
                    Thread.Sleep(50);
                }
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

            while (true)
                Thread.Sleep(1000); // Don't exit after exception
        }

        private static void Wait(int sec)
        {
            for (int i = sec; i > 0; i--)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }
        }

        private static PointF Interpolate(PointF p1, PointF p2, float length)
        {
            float distance = (float)(Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)));
            if (distance > length)
            {
                float t = length / distance;
                float xt = (1 - t) * p1.X + (t * p2.X);
                float yt = (1 - t) * p1.Y + (t * p2.Y);
                return new PointF(xt, yt);
            }
            return p2;
        }
    }
}
