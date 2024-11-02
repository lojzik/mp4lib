using System.IO;

namespace JHa.MP4
{
    public class BoxMDIA : BoxNested
    {
        public BoxMDIA(SubStream stream) : base(stream)
        {
            HDLR = FindBox<BoxHDLR>("hdlr");
            MINF = FindBox<BoxMINF>("minf");
        }
        public BoxHDLR HDLR { get; }
        public BoxMINF MINF { get; }
    }
}
