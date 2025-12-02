using System;
using System.IO;

namespace Jha.MP4;

public class BoxMDIA : BoxNested
{
    public BoxMDIA(SubStream stream) : base(stream)
    {
        HDLR = FindBox<BoxHDLR>(BoxName.hdlr) ?? throw new BoxNotFoundException(BoxName.hdlr);
        MINF = FindBox<BoxMINF>(BoxName.minf) ?? throw new BoxNotFoundException(BoxName.minf); ;
    }
    public BoxHDLR HDLR { get; }
    public BoxMINF MINF { get; }
}
