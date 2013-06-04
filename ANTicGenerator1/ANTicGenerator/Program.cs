using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace ANTicGenerator
{
    class Program
    {
        static void Work(string fileName)
        {
            var background = new Bitmap("background.bmp");
            var head = new Bitmap("head.bmp");
            var body0 = new Bitmap("body0.bmp");
            var body1 = new Bitmap("body1.bmp");
            var antenna0 = new Bitmap("antenna0.bmp");
            var antenna1 = new Bitmap("antenna1.bmp");
            var eye = new Bitmap("eye.bmp");
            var hair = new Bitmap("hair.bmp");
            var legs = (from i in Enumerable.Range(0, 6)
                        let name = string.Format("leg{0}.bmp", i)
                        select new Bitmap(name)).ToArray();

            List<Bitmap> parts = new[] { background, head, body0, body1, antenna0, antenna1, eye, hair }.ToList();
            parts.AddRange(legs);

            Bitmap canvas = new Bitmap(background.Width, background.Height);

            List<Color> partColors = new List<Color>();

            Random rand = new Random();

            for (int i = 0; i < 14; i++)
            {
                var baseColor = Color.FromArgb(rand.Next());
                var color = Color.FromArgb(255, baseColor);
                partColors.Add(color);
            }


            int count = 0;

            foreach (var part in parts)
            {
                List<Point> pixels = new List<Point>();

                for (int i = 0; i < part.Height; i++)
                {
                    for (int j = 0; j < part.Width; j++)
                    {
                        var pixel = part.GetPixel(j, i);
                        var target0 = canvas.GetPixel(j, i);

                        if (pixel.GetBrightness() != 0)
                        {
                            var a = (byte)(partColors[count].A * pixel.A / 255.0);
                            var r = (byte)(partColors[count].R * pixel.R / 255.0);
                            var g = (byte)(partColors[count].G * pixel.G / 255.0);
                            var b = (byte)(partColors[count].B * pixel.B / 255.0);

                            var target1 = Color.FromArgb(a, r, g, b);

                            canvas.SetPixel(j, i, target1);
                        }
                    }
                }

                count++;
            }

            canvas.Save(fileName);
        }

        static void Main(string[] args)
        {
            System.Threading.Tasks.Parallel.For(0, 1000, (i) =>
                {
                    var path = System.IO.Path.Combine(Environment.CurrentDirectory,
                        "ANTic", string.Format("ANTic{0}.png", i));

                    Work(path);
                });
        }
    }
}
