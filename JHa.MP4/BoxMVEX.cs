
using JHa.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace JHa.MP4;

[Box("mvex")]
public class BoxMVEX(SubStream stream) : BoxNested(stream)
{
    public BoxMEHD? MEHD => FindBox<BoxMEHD>(BoxName.mehd);
    public IList<BoxTREX> TREXs => FindBoxMulti<BoxTREX>(BoxName.trex);
}
