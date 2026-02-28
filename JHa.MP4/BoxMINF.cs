using JHa.Common;
using System.IO;

namespace JHa.MP4;

[Box("minf")]
public class BoxMINF : BoxNested
{
    public BoxMINF(SubStream stream) : base(stream)
    {
        STBL = FindMandatoryBox<BoxSTBL>(BoxName.stbl);
        VMHD = FindBox<BoxVMHD>(BoxName.vmhd);
        SMHD = FindBox<BoxSMHD>(BoxName.smhd);
        NMHD = FindBox<BoxNMHD>(BoxName.nmhd);
    }
    public BoxSTBL STBL { get; }
    public BoxVMHD? VMHD { get; }
    public BoxSMHD? SMHD { get; }
    public BoxNMHD? NMHD { get; }
}
