
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mp4s")]
public class BoxMP4S(SubStream stream) : Box(stream)
{
    // SampleEntry fields
    public UInt16 DataReferenceIndex { get; private set; }

    public BoxESDS? ESDS { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        // 6 bytes reserved
        for (int i = 0; i < 3; i++) ReadInt16();
        DataReferenceIndex = (UInt16)ReadInt16();

        // Parse child boxes (esds)
        if (Stream.Position < Size)
        {
            var child = BoxFactory.Instance.CreateBox(Stream, Stream.Position);
            if (child is BoxESDS esds)
                ESDS = esds;
        }
    }

    public override string ToString() => $"{base.ToString()}: DataReferenceIndex={DataReferenceIndex}";
}
