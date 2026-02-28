
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mp4a")]
public class BoxMP4A(SubStream stream) : Box(stream)
{
    // SampleEntry fields
    public UInt16 DataReferenceIndex { get; private set; }
    // AudioSampleEntry fields
    public UInt16 ChannelCount { get; private set; }
    public UInt16 SampleSize { get; private set; }
    public UInt32 SampleRate { get; private set; }

    public BoxESDS? ESDS { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        // 6 bytes reserved
        for (int i = 0; i < 3; i++) ReadInt16();
        DataReferenceIndex = (UInt16)ReadInt16();
        // AudioSampleEntry: 8 bytes reserved
        ReadUInt32(); ReadUInt32();
        ChannelCount = (UInt16)ReadInt16();
        SampleSize = (UInt16)ReadInt16();
        ReadUInt32(); // pre_defined + reserved
        SampleRate = ReadUInt32(); // 16.16 fixed-point

        // Parse child boxes (esds)
        if (Stream.Position < Size)
        {
            var child = BoxFactory.Instance.CreateBox(Stream, Stream.Position);
            if (child is BoxESDS esds)
                ESDS = esds;
        }
    }

    public override string ToString() => $"{base.ToString()}: ChannelCount={ChannelCount}, SampleSize={SampleSize}, SampleRate={SampleRate >> 16}";
}
