using System.IO;

namespace Jha.MP4;

public class BoxMINF : BoxNested
{
    public BoxMINF(SubStream stream) : base(stream)
    {
        STBL = FindBox<BoxSTBL>(BoxName.stbl) ?? throw new BoxNotFoundException(BoxName.stbl);
    }
    public BoxSTBL STBL { get; }
}
