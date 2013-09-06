using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;
using LinqToTwitter;
using System.Threading;

namespace ANTicGenerator
{
    class Program
    {
        void UpdateImage(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Close();
            var buffer = stream.GetBuffer();

            var lines = File.ReadAllLines("auth.txt");

            var auth = new SingleUserAuthorizer
            {
                Credentials = new SingleUserInMemoryCredentials
                {
                    ConsumerKey = lines[0],
                    ConsumerSecret = lines[1],
                    TwitterAccessToken = lines[2],
                    TwitterAccessTokenSecret = lines[3]
                }
            };

            var context = new TwitterContext(auth);

            context.UpdateAccountImage(buffer, "ANTicGeneratorRocks.png", "png", true);
        }

        Bitmap MakeImage(int seed, Bitmap background, List<Bitmap> layers)
        {
            Processor processor = new Processor();
            processor.Background = background;
            processor.Layers = layers;

            return processor.Process(seed);
        }

        void Start()
        {
            string iDirectory = Console.ReadLine();
            int layersCount = int.Parse(Console.ReadLine());

            Random rand = new Random();

            var layers = new List<Bitmap>();

            for (var layer = 1; layer <= layersCount; layer++)
            {
                layers.Add(new Bitmap(System.IO.Path.Combine(iDirectory,
                    string.Format("{0}.png", layer))));
            }

            var background = new Bitmap(System.IO.Path.Combine(iDirectory, "background.png"));

            while (true)
            {
                var bitmap = MakeImage(rand.Next(), background, layers);
                UpdateImage(bitmap);

                Thread.Sleep(5 * 60 * 1000);
            }
        }

        static void Main(string[] args)
        {
            new Program().Start();
        }
    }
}
