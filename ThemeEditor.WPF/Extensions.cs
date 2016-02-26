// --------------------------------------------------
// 3DS Theme Editor - Extensions.cs
// --------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ThemeEditor.Common.Graphics;

using Color = System.Windows.Media.Color;

namespace ThemeEditor.WPF
{
    internal static class Extensions
    {
        public static Bitmap ScrCap(Rectangle area)
        {
            IntPtr desktopHandle = NativeMethods.GetDesktopWindow();
            IntPtr desktopDC = NativeMethods.GetWindowDC(desktopHandle);
            IntPtr destinationDC = NativeMethods.CreateCompatibleDC(desktopDC);
            IntPtr bitmapHandle = NativeMethods.CreateCompatibleBitmap(desktopDC, area.Width, area.Height);
            IntPtr oldBitmapHandle = NativeMethods.SelectObject(destinationDC, bitmapHandle);

            const CopyPixelOperation operation = CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt;
            NativeMethods.BitBlt(destinationDC, 0, 0, area.Width, area.Height, desktopDC, area.X, area.Y, operation);

            Bitmap desktopCapture = Image.FromHbitmap(bitmapHandle);
            NativeMethods.SelectObject(destinationDC, oldBitmapHandle);
            NativeMethods.DeleteObject(bitmapHandle);
            NativeMethods.DeleteDC(destinationDC);
            NativeMethods.ReleaseDC(desktopHandle, desktopDC);

            return desktopCapture;
        }

        public static ImageSource CreateResizedImage(ImageSource source, int width, int height, int margin = 0)
        {
            var rect = new Rect(margin, margin, width - margin * 2, height - margin * 2);

            var group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
                drawingContext.DrawDrawing(group);

            var resizedImage = new RenderTargetBitmap(
                width, height,         // Resized dimensions
                96, 96,                // Default DPI values
                PixelFormats.Default); // Default pixel format
            resizedImage.Render(drawingVisual);

            return BitmapFrame.Create(resizedImage);
        }

        public static Color Blend(this Color bg, Color fg, float factor)
        {
            float alphaFg = fg.A / 255.0f * factor;
            float alphaBg = bg.A / 255.0f;

            float preRfg = fg.R * alphaFg;
            float preGfg = fg.G * alphaFg;
            float preBfg = fg.B * alphaFg;

            float preRbg = bg.R * alphaBg;
            float preGbg = bg.G * alphaBg;
            float preBbg = bg.B * alphaBg;

            float newAlpha = alphaFg + alphaBg * (1 - alphaFg);

            byte newR = (byte) ((preRfg + preRbg * (1 - alphaFg)) / newAlpha).Clamp(0, 255);
            byte newG = (byte) ((preGfg + preGbg * (1 - alphaFg)) / newAlpha).Clamp(0, 255);
            byte newB = (byte) ((preBfg + preBbg * (1 - alphaFg)) / newAlpha).Clamp(0, 255);
            byte newA = (byte) (newAlpha * 255).Clamp(0, 255);

            return Color.FromArgb(newA, newR, newG, newB);
        }

        public static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(max) > 0)
                return max;
            if (value.CompareTo(min) < 0)
                return min;
            return value;
        }

        public static byte[] GetBgr24Data(this BitmapSource bmp)
        {
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int s = w * 3;
            var bgr24Data = new byte[h * s];

            var pbgr32Bmp = new FormatConvertedBitmap(bmp, PixelFormats.Pbgra32, null, 0);
            var bgr24Bmp = new FormatConvertedBitmap(pbgr32Bmp, PixelFormats.Bgr24, null, 0);

            bgr24Bmp.CopyPixels(bgr24Data, s, 0);

            return bgr24Data;
        }

        public static IDictionary<string, object> GetResources(string pattern)
        {
            pattern = pattern.ToLowerInvariant();
            var asm = Assembly.GetEntryAssembly();
            var resName = asm.GetName().Name + ".g.resources";
            var stream = asm.GetManifestResourceStream(resName);
            var reader = new ResourceReader(stream);

            var dict = new Dictionary<string, object>();
            foreach (DictionaryEntry res in reader)
            {
                var path = (string) res.Key;
                if (Regex.IsMatch(path, pattern))
                    dict.Add(path, res.Value);
            }
            return dict;
        }

        public static ColorRgb888 ToColorRgb888(this Color c)
        {
            return new ColorRgb888(c.R, c.G, c.B);
        }

        public static ColorArgb8888 ToColorArgb8888(this Color c)
        {
            return new ColorArgb8888(c.A, c.R, c.G, c.B);
        }

        public static Color ToMediaColor(this ColorRgb888 c)
        {
            return Color.FromArgb(255, c.R, c.G, c.B);
        }

        public static Color ToMediaColor(this ColorArgb8888 c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}
