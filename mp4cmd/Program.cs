using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JHa.MP4;

namespace mp4cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = String.Empty;
            if (args.Length == 0)
            {
                Console.WriteLine("parametry: file.mp4 [-e] [-s]");
                return;
            }
            else
            {
                filename = args[0];
            }
            using var mp4file = MP4File.Parse(filename);
            if (args.Contains("-s"))
                Display(mp4file.Nested, "");
            if (args.Contains("-e"))
            {
                foreach (var trak in mp4file.MOOV.TRAKS)
                {
                    var trackname = filename + $".track.{trak.TrackId}.bin";
                    using var filewrite = File.OpenWrite(trackname);
                    trak.WriteAllTo(filewrite);
                }
            }
        }
        static void Display(Box box, string padding)
        {
            Console.WriteLine(padding + box);
            if (box is BoxNested n)
                Display(n.Nested, padding + "  ");
        }
        static void Display(IEnumerable<Box> boxes, string padding)
        {
            foreach (var box in boxes)
                Display(box, padding);
        }

    }
}
