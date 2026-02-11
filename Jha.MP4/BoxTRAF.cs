using JHa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHa.MP4;

[Box("traf")]
public class BoxTRAF(SubStream stream) : BoxNested(stream)
{

    public IList<BoxTRUN> TRUNs => [..FindBoxMulti<BoxTRUN>(BoxName.trun)];
}
