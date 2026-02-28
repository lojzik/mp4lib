using JHa.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JHa.MP4;

[Box("moov")]
public class BoxMOOV : BoxNested
{
    public BoxMOOV(SubStream stream) : base(stream)
    {
        MVHD = FindMandatoryBox<BoxMVHD>(BoxName.mvhd);
        TRAKS = [.. FindBoxMulti<BoxTRAK>(BoxName.trak)];
        UDTA = FindBox<BoxNested>(BoxName.udta);
        MVEX = FindBox<BoxMVEX>(BoxName.mvex);
    }
    public BoxMVHD MVHD { get; }
    public IList<BoxTRAK> TRAKS { get; }
    public BoxNested? UDTA { get; }
    public BoxMVEX? MVEX { get; }
}
