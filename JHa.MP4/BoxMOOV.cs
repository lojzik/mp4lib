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
        TRAKS = [.. FindBoxMulti<BoxTRAK>(BoxName.trak)];
        UDTA = FindBox<BoxNested>(BoxName.udta);
    }
    public IList<BoxTRAK> TRAKS { get; }
    public BoxNested? UDTA { get; }
}
