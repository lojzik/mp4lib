using Jha.Common;
using System.IO;

namespace Jha.MP4;

[Box("minf")]
public class BoxMINF : BoxNested
{
    public BoxMINF(SubStream stream) : base(stream)
    {
        STBL = FindMandatoryBox<BoxSTBL>(BoxName.stbl);
    }
    public BoxSTBL STBL { get; }
}
