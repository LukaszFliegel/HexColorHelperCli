using CommandLine;
using HexColorHelperCli.Verbs;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace HexColorHelperCli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<HoverColors>(args)
                   .WithParsed<HoverColors>(verb =>
                   {
                       GenerateHoverColors(verb);
                   });
        }

        private static void GenerateHoverColors(HoverColors verb)
        {
            var colorList = verb.ColorHashes.Split(",");
            var hoverColorList = new List<string>();
            var imageCellSizeInPixels = 240;

            Image image = new Bitmap(colorList.Length * imageCellSizeInPixels, 2 * imageCellSizeInPixels);

            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);

            var strength = verb.Strength > 1 ? 0.5 : verb.Strength;
            strength = verb.Strength < 0 ? 0.5 : verb.Strength;

            var colorIndex = 0;
            foreach (var colorHash in colorList)
            {
                //Console.WriteLine($"Generating color for {colorHash}");

                var colorFromHash = ColorTranslator.FromHtml(colorHash);
                var hsvColor = ColorToHSV(colorFromHash);

                if (verb.Darker)
                {
                    // to darker the collor lower it's value (by inverted strength since 0.1 is a small strength but multiply by 0.1 will give "bigger" effect on color that multiply it by 0.9)
                    hsvColor.value *= 1 - strength;
                }
                else
                {
                    // to make color lighter decrese the saturation
                    hsvColor.saturation *= 1 - strength;
                }

                var modifiedColor = ColorFromHSV(hsvColor);
                hoverColorList.Add(ColorTranslator.ToHtml(modifiedColor));

                //Console.WriteLine($"Generated hover color {ColorTranslator.ToHtml(modifiedColor)}");

                DrawPaletteRectangle(imageCellSizeInPixels, graphics, colorIndex, colorFromHash, modifiedColor);

                colorIndex++;
            }

            var hoverColors = BuildStringArrayOfColors(hoverColorList);

            Console.WriteLine("Generated array of hover colors:");
            Console.WriteLine(hoverColors);

            if (!string.IsNullOrEmpty(verb.FileName))
            {
                GenearetFile(verb, image);
            }
        }

        private static void GenearetFile(HoverColors verb, Image image)
        {            
            image.Save($"{verb.FileName}.png", ImageFormat.Png);

            if (verb.OpenFile)
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo($"{verb.FileName}.png")
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }            
        }

        private static void DrawPaletteRectangle(int imageCellSizeInPixels, Graphics graphics, int index, Color colorFromHash, Color modifiedColor)
        {
            Pen pen = new(colorFromHash);
            Rectangle rect = new Rectangle(index * imageCellSizeInPixels, 0, imageCellSizeInPixels, imageCellSizeInPixels);
            graphics.FillRectangle(pen.Brush, rect);

            pen = new Pen(modifiedColor);
            rect = new Rectangle(index * imageCellSizeInPixels, imageCellSizeInPixels, imageCellSizeInPixels, imageCellSizeInPixels);
            graphics.FillRectangle(pen.Brush, rect);
        }

        private static string BuildStringArrayOfColors(List<string> hoverColorList)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            foreach (var color in hoverColorList)
            {
                sb.Append($"\"{color}\", ");
            }
            sb.Append(']');

            return sb.ToString();
        }

        private struct HSVColor
        {
            public double hue;
            public double saturation;
            public double value;
        }

        private static HSVColor ColorToHSV(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.GetHue();
            var saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            var value = max / 255d;

            return new HSVColor()
            {
                hue = hue,
                saturation = saturation,
                value = value,
            };
        }

        private static Color ColorFromHSV(HSVColor hsvColor)
        {
            int hi = Convert.ToInt32(Math.Floor(hsvColor.hue / 60)) % 6;
            double f = hsvColor.hue / 60 - Math.Floor(hsvColor.hue / 60);

            hsvColor.value = hsvColor.value * 255;
            int v = Convert.ToInt32(hsvColor.value);
            int p = Convert.ToInt32(hsvColor.value * (1 - hsvColor.saturation));
            int q = Convert.ToInt32(hsvColor.value * (1 - f * hsvColor.saturation));
            int t = Convert.ToInt32(hsvColor.value * (1 - (1 - f) * hsvColor.saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}