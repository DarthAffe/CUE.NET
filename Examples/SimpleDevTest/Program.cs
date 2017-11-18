using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Effects;
using CUE.NET.Exceptions;
using CUE.NET.Gradients;
using CUE.NET.Groups;

namespace SimpleDevTest
{
    internal class Program
    {
        private static readonly Random Rand = new Random();

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
                bool test = CueSDK.IsSDKAvailable();

                // Initialize CUE-SDK
                CueSDK.Initialize();
                Console.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");

                CueSDK.KeyPressed += (sender, eventArgs) => Console.WriteLine($"Key {eventArgs.KeyId} {(eventArgs.IsPressed ? "pressed" : "released")}");

                //CueSDK.KeyboardSDK.Brush = (SolidColorBrush)Color.Black;
                //CueSDK.KeyboardSDK[CorsairLedId.Z].Color = Color.Red;
                //CueSDK.KeyboardSDK[CorsairLedId.Z].IsLocked = true;

                float thirdKeyboardWidth = CueSDK.KeyboardSDK.DeviceRectangle.Width / 3f;
                ILedGroup left = new RectangleLedGroup(CueSDK.KeyboardSDK, new RectangleF(CueSDK.KeyboardSDK.DeviceRectangle.X, CueSDK.KeyboardSDK.DeviceRectangle.Y, thirdKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Height));
                ILedGroup mid = new RectangleLedGroup(CueSDK.KeyboardSDK, new RectangleF(CueSDK.KeyboardSDK.DeviceRectangle.X + thirdKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Y, thirdKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Height));
                ILedGroup right = new RectangleLedGroup(CueSDK.KeyboardSDK, new RectangleF(CueSDK.KeyboardSDK.DeviceRectangle.X + thirdKeyboardWidth * 2, CueSDK.KeyboardSDK.DeviceRectangle.Y, thirdKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Height));

                //CueSDK.KeyboardSDK.Brush = new LinearGradientBrush(new LinearGradient(true, new GradientStop(0, Color.Blue), new GradientStop(0.5f, Color.Red)));
                left.Brush = new ConicalGradientBrush(new PointF(0.6f, 0.7f), new RainbowGradient(360, 0));
                left.Brush.AddEffect(new MoveGradientEffect());
                left.Brush.AddEffect(new FlashEffect { Attack = 2, Sustain = 1f, Release = 2, Interval = 1f });

                mid.Brush = new ConicalGradientBrush(new PointF(0.5f, 0.3f), new RainbowGradient());
                mid.Brush.AddEffect(new MoveGradientEffect());
                mid.Brush.AddEffect(new FlashEffect { Attack = 2, Sustain = 1f, Release = 2, Interval = 1f });

                right.Brush = new ConicalGradientBrush(new PointF(0.4f, 0.7f), new RainbowGradient(360, 0));
                right.Brush.AddEffect(new MoveGradientEffect());
                right.Brush.AddEffect(new FlashEffect { Attack = 2, Sustain = 1f, Release = 2, Interval = 1f });

                //float halfKeyboardWidth = CueSDK.KeyboardSDK.DeviceRectangle.Width / 2f;
                //ILedGroup left = new RectangleLedGroup(CueSDK.KeyboardSDK, new RectangleF(CueSDK.KeyboardSDK.DeviceRectangle.X, CueSDK.KeyboardSDK.DeviceRectangle.Y, halfKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Height));
                //ILedGroup right = new RectangleLedGroup(CueSDK.KeyboardSDK, new RectangleF(CueSDK.KeyboardSDK.DeviceRectangle.X + halfKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Y, halfKeyboardWidth, CueSDK.KeyboardSDK.DeviceRectangle.Height));

                ////CueSDK.KeyboardSDK.Brush = new LinearGradientBrush(new LinearGradient(true, new GradientStop(0, Color.Blue), new GradientStop(0.5f, Color.Red)));
                //left.Brush = new ConicalGradientBrush(new PointF(0.6f, 0.6f), new RainbowGradient(360, 0));
                //left.Brush.AddEffect(new MoveGradientEffect());

                //right.Brush = new ConicalGradientBrush(new PointF(0.4f, 0.6f), new RainbowGradient());
                //right.Brush.AddEffect(new MoveGradientEffect());

                CueSDK.UpdateMode = UpdateMode.Continuous;

                //IBrush rainbowBrush = new LinearGradientBrush(new RainbowGradient());
                //rainbowBrush.AddEffect(new FlashEffect { Attack = 5f, Sustain = 1f, Decay = 0, Release = 5f, Interval = 1f });
                //rainbowBrush.AddEffect(new MoveRainbowEffect());
                //rainbowBrush.AddEffect(new RemoveRedEffect());

                //foreach (ICueDevice device in CueSDK.InitializedDevices)
                //    AddTestBrush(device, rainbowBrush);

                //// Get connected keyboard or throw exception if there is no light controllable keyboard connected
                //CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
                //if (keyboard == null)
                //    throw new WrapperException("No keyboard found");

                //const float SPEED = 100f; // mm/sec
                //const float BRUSH_MODE_CHANGE_TIMER = 2f;
                //Random random = new Random();

                //keyboard.UpdateMode = UpdateMode.Continuous;
                //keyboard.Brush = new SolidColorBrush(Color.Black);

                //RectangleF spot = new RectangleF(keyboard.DeviceRectangle.Width / 2f, keyboard.DeviceRectangle.Y / 2f, 160, 80);
                //PointF target = new PointF(spot.X, spot.Y);
                //RectangleLedGroup spotGroup = new RectangleLedGroup(keyboard, spot) { Brush = new LinearGradientBrush(new RainbowGradient()) };

                //float brushModeTimer = BRUSH_MODE_CHANGE_TIMER;
                //keyboard.Updating += (sender, eventArgs) =>
                //{
                //    brushModeTimer -= eventArgs.DeltaTime;
                //    if (brushModeTimer <= 0)
                //    {
                //        spotGroup.Brush.BrushCalculationMode = spotGroup.Brush.BrushCalculationMode == BrushCalculationMode.Relative
                //                                                ? BrushCalculationMode.Absolute : BrushCalculationMode.Relative;
                //        brushModeTimer = BRUSH_MODE_CHANGE_TIMER + brushModeTimer;
                //    }

                //    if (spot.Contains(target))
                //        target = new PointF((float)(keyboard.DeviceRectangle.X + (random.NextDouble() * keyboard.DeviceRectangle.Width)),
                //                (float)(keyboard.DeviceRectangle.Y + (random.NextDouble() * keyboard.DeviceRectangle.Height)));
                //    else
                //        spot.Location = Interpolate(spot.Location, target, eventArgs.DeltaTime * SPEED);
                //    spotGroup.Rectangle = spot;
                //};

                //CorsairMousemat mousemat = CueSDK.MousematSDK;
                //mousemat.UpdateMode = UpdateMode.Continuous;

                //// Left
                //mousemat[CorsairMousematLedId.Zone1].Color = Color.Red;
                //mousemat[CorsairMousematLedId.Zone2].Color = Color.Red;
                //mousemat[CorsairMousematLedId.Zone3].Color = Color.Red;
                //mousemat[CorsairMousematLedId.Zone4].Color = Color.Red;
                //mousemat[CorsairMousematLedId.Zone5].Color = Color.Red;
                //// Bottom
                //mousemat[CorsairMousematLedId.Zone6].Color = Color.LawnGreen;
                //mousemat[CorsairMousematLedId.Zone7].Color = Color.LawnGreen;
                //mousemat[CorsairMousematLedId.Zone8].Color = Color.LawnGreen;
                //mousemat[CorsairMousematLedId.Zone9].Color = Color.LawnGreen;
                //mousemat[CorsairMousematLedId.Zone10].Color = Color.LawnGreen;
                //// Right
                //mousemat[CorsairMousematLedId.Zone11].Color = Color.Blue;
                //mousemat[CorsairMousematLedId.Zone12].Color = Color.Blue;
                //mousemat[CorsairMousematLedId.Zone13].Color = Color.Blue;
                //mousemat[CorsairMousematLedId.Zone14].Color = Color.Blue;
                //mousemat[CorsairMousematLedId.Zone15].Color = Color.Blue;

                // Random colors to show update rate
                //foreach (var mousematLed in mousemat.Leds)
                //    mousematLed.Color = GetRandomRainbowColor();

                //mousemat.Updating += (sender, eventArgs) =>
                //{
                //    foreach (var mousematLed in mousemat.Leds)
                //    {
                //        mousematLed.Color = ShiftColor(mousematLed.Color, 20);
                //    }
                //};

                //keyboard.Brush = new SolidColorBrush(Color.Black);
                //ILedGroup group = new RectangleLedGroup(keyboard, CorsairKeyboardKeyId.F1, CorsairKeyboardKeyId.RightShift);
                //group.Brush = new LinearGradientBrush(new RainbowGradient());
                //bool tmp = false;
                //while (true)
                //{
                //    group.Brush.BrushCalculationMode = tmp ? BrushCalculationMode.Absolute : BrushCalculationMode.Relative;

                //    tmp = !tmp;
                //    keyboard.Update();
                //    Wait(1);
                //}

                //keyboard.Brush = new SolidColorBrush(Color.Aqua);
                //keyboard.Update();

                //ILedGroup specialKeyGroup = new ListLedGroup(keyboard, CorsairKeyboardKeyId.Brightness, CorsairKeyboardKeyId.WinLock);
                //specialKeyGroup.Brush = new SolidColorBrush(Color.Aqua);
                //keyboard.Update();

                //// Replacing the specialKeyGroup with this won't work
                //keyboard[CorsairKeyboardKeyId.Brightness].Led.Color = Color.Aqua;
                //keyboard[CorsairKeyboardKeyId.Brightness].Led.IsLocked = true;
                //keyboard[CorsairKeyboardKeyId.WinLock].Led.Color = Color.Aqua;
                //keyboard[CorsairKeyboardKeyId.WinLock].Led.IsLocked = true;
                //keyboard.Update();

                //Wait(3);

                //CueSDK.Reinitialize();
                ////keyboard.Brush = CueProfiles.LoadProfileByID()[null];
                ////keyboard.Update();

                //Wait(3);

                //// My Profile 'K95 RGB Default 2' is all black - this could lead to different behavior than cue has since transparent isn't black in CUE.NET
                //// To swap a profile like CUE does we would need to black out the keyboard before 
                //// OR work with a key group containing all keys and leave the background black - this should be always the prefered solution
                //keyboard.Brush = new SolidColorBrush(Color.Black);
                //keyboard.Update();
                ////keyboard.Brush = CueProfiles.LoadProfileByID()["K95 RGB Default 2"];
                ////keyboard.Update();

                //Wait(3);

                //ListLedGroup ledGroup = new ListLedGroup(keyboard, keyboard['R'].KeyId);
                //ledGroup.Brush = new SolidColorBrush(Color.White);
                //keyboard.Update();
                //Wait(2);
                //ledGroup.RemoveKey(keyboard['R'].KeyId);
                //keyboard['R'].Led.Color = Color.Black;
                //ledGroup.AddKey(keyboard['T'].KeyId);
                //keyboard.Update();

                //Wait(10);

                //return;

                // ---------------------------------------------------------------------------
                // First we'll look at some basic coloring

                //Console.WriteLine("Basic color-test ...");
                //// Ink all numbers on the keypad except the '5' purple, we want that to be gray
                //ListLedGroup purpleGroup = new RectangleLedGroup(keyboard, CorsairKeyboardKeyId.Keypad7, CorsairKeyboardKeyId.Keypad3)
                //{ Brush = new SolidColorBrush(Color.Purple) }
                //.Exclude(CorsairKeyboardKeyId.Keypad5);
                //keyboard[CorsairKeyboardKeyId.Keypad5].Led.Color = Color.Gray;

                //// Ink the Keys 'r', 'g', 'b' in their respective color
                //// The char access fails for everything except letters (SDK doesn't return a valid keyId)
                //keyboard['R'].Led.Color = Color.Red;
                //keyboard[CorsairKeyboardKeyId.G].Led.Color = Color.Green;
                //keyboard['B'].Led.Color = Color.Blue;

                //// Lock the 'r', 'g', 'b' keys. We want them to stay like this forever (commented since it looks quite stupid later, but feel free tu uncomment this)
                ////keyboard['R'].Led.IsLocked = true;
                ////keyboard['G'].Led.IsLocked = true;
                ////keyboard['B'].Led.IsLocked = true;

                //// Ink the letters of 'white' white
                //ListLedGroup whiteGroup = new ListLedGroup(keyboard, CorsairKeyboardKeyId.W, CorsairKeyboardKeyId.H, CorsairKeyboardKeyId.I, CorsairKeyboardKeyId.T, CorsairKeyboardKeyId.E)
                //{ Brush = new SolidColorBrush(Color.White) };

                //// Ink the keys '1' to '0' yellow
                //RectangleLedGroup yellowGroup = new RectangleLedGroup(keyboard, CorsairKeyboardKeyId.D1, CorsairKeyboardKeyId.D0)
                //{ Brush = new SolidColorBrush(Color.Yellow) };

                //// Update the keyboard to show the configured colors, (your CUE settings defines the rest)
                //keyboard.Update();

                //Wait(3);

                //// Remove all the groups we created above to clear the keyboard
                //purpleGroup.Detach();
                //whiteGroup.Detach();
                //yellowGroup.Detach();


                //// ---------------------------------------------------------------------------
                //// Next we add a nice linear gradient brush over the keyboard and play around with the offset of one stop

                //Console.WriteLine("gradient-brush-test");

                //// Create our gradient stop to play with
                //GradientStop moveableStop = new GradientStop(0, Color.FromArgb(0, 255, 0));

                //// Create a basic (by default horizontal) brush ...
                //LinearGradientBrush linearBrush = new LinearGradientBrush(new LinearGradient(new GradientStop(0, Color.Blue), moveableStop, new GradientStop(1f, Color.White)));

                //// ... and add it as the keyboard background
                //keyboard.Brush = linearBrush;

                //// Move the brush from left to right
                //for (float offset = 0; offset <= 1f; offset += 0.02f)
                //{
                //    moveableStop.Offset = offset;
                //    keyboard.Update();
                //    Thread.Sleep(100);
                //}

                //// And back to center
                //for (float offset = 1f; offset >= 0.5f; offset -= 0.02f)
                //{
                //    moveableStop.Offset = offset;
                //    keyboard.Update();
                //    Thread.Sleep(100);
                //}

                //// "Rotate" the brush (this is of course not the best implementation for this but you see the point)
                //for (float rotateX = 0, rotateY = 0; rotateX <= 1f; rotateX += 0.02f, rotateY = 0.04f)
                //{
                //    if (rotateY > 1f)
                //        rotateY = 1f - (rotateY - 1f);

                //    linearBrush.StartPoint = new PointF(rotateX, rotateY);
                //    linearBrush.EndPoint = new PointF(1f - rotateX, 1f - rotateY);

                //    keyboard.Update();
                //    Thread.Sleep(100);
                //}

                //Wait(2);

                //// ---------------------------------------------------------------------------
                //// Time for an even better brush: rainbow

                //Console.WriteLine("rainbow-test");

                //// Create an simple horizontal rainbow containing two times the full spectrum
                //RainbowGradient rainbowGradient = new RainbowGradient(0, 720);

                //// Add the rainbow to the keyboard and perform an initial update
                //keyboard.Brush = new LinearGradientBrush(rainbowGradient);
                //keyboard.Update();

                //// Let the rainbow move around for 10 secs
                //for (int i = 0; i < 100; i++)
                //{
                //    rainbowGradient.StartHue += 10f;
                //    rainbowGradient.EndHue += 10f;
                //    keyboard.Update();
                //    Thread.Sleep(100);
                //}

                //Wait(2);


                // ---------------------------------------------------------------------------
                // Now let us move some points random over the keyboard
                // Something like this could become some sort of effect

                // Initialize needed stuff
                //    const float SPEED = 6f; // mm/tick
                //Random random = new Random();

                //// Flash whole keyboard three times to ... well ... just to make it happen
                //for (int i = 0; i < 3; i++)
                //{
                //    keyboard.Brush = new SolidColorBrush(Color.Aquamarine);
                //    keyboard.Update();
                //    Thread.Sleep(160);
                //    keyboard.Brush = new SolidColorBrush(Color.Black);
                //    keyboard.Update();
                //    Thread.Sleep(200);
                //}

                //// Set keyboard 'background' to black with low alpha (this will add a nice "fade" effect instead of just clearing the keyboard every frame)
                //keyboard.Brush = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));

                //// Define how many points we have
                //const int NUM_POINTS = 6;

                //// The points we want to draw (rectangle since circles are too hard to calculate :p)
                //RectangleF[] points = new RectangleF[NUM_POINTS];

                //// KeyGroups which represents our point on the keyboard
                //RectangleLedGroup[] pointGroups = new RectangleLedGroup[NUM_POINTS];

                //// Target of our movement
                //PointF[] targets = new PointF[NUM_POINTS];

                //// Initialize all the stuff
                //for (int i = 0; i < NUM_POINTS; i++)
                //{
                //    // Spawn our point  in the top-left corner (right over G1 or on ESC depending on your keyboard)
                //    points[i] = new RectangleF(keyboard.KeyboardRectangle.X, keyboard.KeyboardRectangle.Y, 60, 60);
                //    pointGroups[i] = new RectangleLedGroup(keyboard, points[i], 0.1f) { Brush = new SolidColorBrush(Color.White) };
                //    targets[i] = new PointF(points[i].X, points[i].Y);
                //}

                //// We set colors manually since white points are kinda boring (notice, that we use alpha values)
                //pointGroups[0].Brush = new RadialGradientBrush(new LinearGradient(new GradientStop(0, Color.FromArgb(127, 255, 0, 0)), new GradientStop(0.5f, Color.FromArgb(127, 255, 0, 0)), new GradientStop(1, Color.FromArgb(0, 255, 0, 0))));
                //pointGroups[1].Brush = new RadialGradientBrush(new LinearGradient(new GradientStop(0, Color.FromArgb(127, 0, 255, 0)), new GradientStop(0.5f, Color.FromArgb(127, 0, 255, 0)), new GradientStop(1, Color.FromArgb(0, 0, 255, 0))));
                //pointGroups[2].Brush = new RadialGradientBrush(new LinearGradient(new GradientStop(0, Color.FromArgb(127, 0, 0, 255)), new GradientStop(0.5f, Color.FromArgb(127, 0, 0, 255)), new GradientStop(1, Color.FromArgb(0, 0, 0, 255))));
                //pointGroups[3].Brush = new RadialGradientBrush(new LinearGradient(new GradientStop(0, Color.FromArgb(127, 255, 0, 255)), new GradientStop(0.5f, Color.FromArgb(127, 255, 0, 255)), new GradientStop(1, Color.FromArgb(0, 255, 0, 255))));
                //pointGroups[4].Brush = new RadialGradientBrush(new LinearGradient(new GradientStop(0, Color.FromArgb(127, 255, 255, 0)), new GradientStop(0.5f, Color.FromArgb(127, 255, 255, 0)), new GradientStop(1, Color.FromArgb(0, 255, 255, 0))));
                //pointGroups[5].Brush = new RadialGradientBrush(new LinearGradient(new GradientStop(0, Color.FromArgb(127, 0, 255, 255)), new GradientStop(0.5f, Color.FromArgb(127, 0, 255, 255)), new GradientStop(1, Color.FromArgb(0, 0, 255, 255))));

                //while (true)
                //{
                //    // Calculate all the points
                //    for (int i = 0; i < NUM_POINTS; i++)
                //    {
                //        // Choose new target if we arrived
                //        if (points[i].Contains(targets[i]))
                //            targets[i] = new PointF((float)(keyboard.KeyboardRectangle.X + (random.NextDouble() * keyboard.KeyboardRectangle.Width)),
                //                    (float)(keyboard.KeyboardRectangle.Y + (random.NextDouble() * keyboard.KeyboardRectangle.Height)));
                //        else
                //            // Calculate movement
                //            points[i].Location = Interpolate(points[i].Location, targets[i], SPEED); // It would be better to calculate from the center of our rectangle but the easy way is enough here

                //        // Move our rectangle to the new position
                //        pointGroups[i].Rectangle = points[i];
                //    }

                //    // Update changed leds
                //    keyboard.Update();

                //    // 20 updates per sec should be enought for this
                //    Thread.Sleep(50);
                //}
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

        private static void AddTestBrush(ICueDevice device, IBrush brush)
        {
            if (device == null) return;

            device.Brush = (SolidColorBrush)Color.Black;
            ILedGroup leds = new ListLedGroup(device, device);
            leds.Brush = brush;
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

        /// <summary>
        ///     Comes up with a 'pure' psuedo-random color
        /// </summary>
        /// <returns>The color</returns>
        public static Color GetRandomRainbowColor()
        {
            List<int> colors = new List<int>();
            for (int i = 0; i < 3; i++)
                colors.Add(Rand.Next(0, 256));

            int highest = colors.Max();
            int lowest = colors.Min();
            colors[colors.FindIndex(c => c == highest)] = 255;
            colors[colors.FindIndex(c => c == lowest)] = 0;

            Color returnColor = Color.FromArgb(255, colors[0], colors[1], colors[2]);

            return returnColor;
        }
        public static Color ShiftColor(Color c, int shiftAmount)
        {
            int newRed = c.R;
            int newGreen = c.G;
            int newBlue = c.B;

            // Red to purple
            if (c.R == 255 && c.B < 255 && c.G == 0)
                newBlue = newBlue + shiftAmount;
            // Purple to blue
            else if (c.B == 255 && c.R > 0)
                newRed = newRed - shiftAmount;
            // Blue to light-blue
            else if (c.B == 255 && c.G < 255)
                newGreen = newGreen + shiftAmount;
            // Light-blue to green
            else if (c.G == 255 && c.B > 0)
                newBlue = newBlue - shiftAmount;
            // Green to yellow
            else if (c.G == 255 && c.R < 255)
                newRed = newRed + shiftAmount;
            // Yellow to red
            else if (c.R == 255 && c.G > 0)
                newGreen = newGreen - shiftAmount;

            newRed = BringIntInColorRange(newRed);
            newGreen = BringIntInColorRange(newGreen);
            newBlue = BringIntInColorRange(newBlue);

            return Color.FromArgb(c.A, newRed, newGreen, newBlue);
        }

        private static int BringIntInColorRange(int i)
        {
            if (i < 0)
                return 0;
            if (i > 255)
                return 255;

            return i;
        }
    }
}
