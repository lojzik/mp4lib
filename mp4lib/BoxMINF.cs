using System.IO;

namespace JHa.MP4
{
    public class BoxMINF : BoxNested
    {
        public BoxMINF(Stream stream, long startIndex) : base(stream, startIndex)
        {
            STBL = FindBox<BoxSTBL>("stbl");
        }
        public BoxSTBL STBL { get; }
    }
}
