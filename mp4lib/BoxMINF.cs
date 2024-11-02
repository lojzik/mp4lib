using System.IO;

namespace JHa.MP4
{
    public class BoxMINF : BoxNested
    {
        public BoxMINF(SubStream stream) : base(stream)
        {
            STBL = FindBox<BoxSTBL>("stbl");
        }
        public BoxSTBL STBL { get; }
    }
}
