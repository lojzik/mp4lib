using Jha.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jha.MP4;

[Box("moof")]
public class BoxMOOF(SubStream stream) : BoxNested(stream)
{
    public BoxMFHD MFHD => FindMandatoryBox<BoxMFHD>(BoxName.mfhd);

    public IList<BoxTRAF> TRAFs => FindBoxMulti<BoxTRAF>(BoxName.traf);
}
