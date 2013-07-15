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
        void Start()
        {
            string iDirectory = Console.ReadLine();
            int layersCount = int.Parse(Console.ReadLine());

            int images = int.Parse(Console.ReadLine());

            Random rand = new Random(42);
            List<int> seeds = new List<int>();
            for (int i = 0; i < images; i++)
            {
                seeds.Add(rand.Next());
            }

            System.Threading.Tasks.Parallel.For(0, images, (i) =>
            {
                var layers = new List<Bitmap>();

                for (var layer = 1; layer <= layersCount; layer++)
                {
                    layers.Add(new Bitmap(System.IO.Path.Combine(iDirectory,
                        string.Format("{0}.png", layer))));
                }

                var background = new Bitmap(System.IO.Path.Combine(iDirectory, "background.png"));

                Processor processor = new Processor();
                processor.Background = background;
                processor.Layers = layers;
                var o = processor.Process(seeds[i]);

                var oPath = System.IO.Path.Combine(Environment.CurrentDirectory, "o");
                System.IO.Directory.CreateDirectory(oPath);
                o.Save(System.IO.Path.Combine(oPath, string.Format("o{0}.png", i)));
            });
            Console.WriteLine("ok");
        }

        static void Main(string[] args)
        {
            new Program().Start();
        }
    }
}
