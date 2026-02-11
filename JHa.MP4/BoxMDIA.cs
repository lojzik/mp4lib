using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mdia")]
public class BoxMDIA : BoxNested
{
    public BoxMDIA(SubStream stream) : base(stream)
    {
        HDLR = FindMandatoryBox<BoxHDLR>(BoxName.hdlr);
        MINF = FindMandatoryBox<BoxMINF>(BoxName.minf);
    }
    public BoxHDLR HDLR { get; }
    public BoxMINF MINF { get; }
}
