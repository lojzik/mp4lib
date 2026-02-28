
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("nmhd")]
public class BoxNMHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
}
