using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANTicGenerator
{
    class Processor
    {
        public Bitmap Background { get; set; }
        public List<Bitmap> Layers { get; set; }

        public Bitmap Process(int seed)
        {
            Random rand = new Random(seed);

            List<Color> partColors = new List<Color>();
            for (int i = 0; i < Layers.Count; i++)
            {
                var baseColor = Color.FromArgb(rand.Next());
                var color = Color.FromArgb(255, baseColor);
                partColors.Add(color);
            }

            Bitmap canvas = new Bitmap(Background.Width, Background.Height);
            for (int i = 0; i < Background.Height; i++)
            {
                for (int j = 0; j < Background.Width; j++)
                {
                    canvas.SetPixel(j, i, Background.GetPixel(j, i));
                }
            }

            for (int index = 0; index < Layers.Count; index++)
            {
                var layer = Layers[index];
                var partColor = partColors[index];

                for (int i = 0; i < Background.Height; i++)
                {
                    for (int j = 0; j < Background.Width; j++)
                    {
                        var pixel = layer.GetPixel(j, i);
                        var target0 = canvas.GetPixel(j, i);

                        double pixelA = pixel.A / 255.0;
                        double pixelR = pixel.R / 255.0;
                        double pixelG = pixel.G / 255.0;
                        double pixelB = pixel.B / 255.0;

                        double target0A = target0.A / 255.0;
                        double target0R = target0.R / 255.0;
                        double target0G = target0.G / 255.0;
                        double target0B = target0.B / 255.0;

                        double a = pixelA + target0A * (1 - pixelA);
                        double r = (partColor.R / 255.0 * pixelR * pixelA + target0R * target0A * (1 - pixelA)) / a;
                        double g = (partColor.G / 255.0 * pixelG * pixelA + target0G * target0A * (1 - pixelA)) / a;
                        double b = (partColor.B / 255.0 * pixelB * pixelA + target0B * target0A * (1 - pixelA)) / a;

                        a *= 255;
                        r *= 255;
                        g *= 255;
                        b *= 255;

                        var target1 = Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
                        canvas.SetPixel(j, i, target1);
                    }
                }
            }

            return canvas;
        }
    }
}
