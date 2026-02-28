
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mp4v")]
public class BoxMP4V(SubStream stream) : Box(stream)
{
    // SampleEntry fields
    public UInt16 DataReferenceIndex { get; private set; }
    // VisualSampleEntry fields
    public UInt16 Width { get; private set; }
    public UInt16 Height { get; private set; }
    public UInt32 HorizResolution { get; private set; }
    public UInt32 VertResolution { get; private set; }
    public string CompressorName { get; private set; } = string.Empty;

    public BoxESDS? ESDS { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        // 6 bytes reserved
        for (int i = 0; i < 3; i++) ReadInt16();
        DataReferenceIndex = (UInt16)ReadInt16();
        // VisualSampleEntry: pre_defined(2) + reserved(2) + pre_defined[3](12)
        ReadInt16(); ReadInt16();
        ReadUInt32(); ReadUInt32(); ReadUInt32();
        Width = (UInt16)ReadInt16();
        Height = (UInt16)ReadInt16();
        HorizResolution = ReadUInt32(); // 16.16 fixed-point
        VertResolution = ReadUInt32(); // 16.16 fixed-point
        ReadUInt32(); // reserved
        ReadInt16(); // frame_count
        // compressorname: 1 byte length + 31 bytes string
        int nameLen = Stream.ReadByte();
        if (nameLen > 31) nameLen = 31;
        CompressorName = nameLen > 0 ? ReadString(nameLen) : string.Empty;
        int pad = 31 - nameLen;
        for (int i = 0; i < pad; i++) Stream.ReadByte();
        ReadInt16(); // depth
        ReadInt16(); // pre_defined

        // Parse child boxes (esds)
        if (Stream.Position < Size)
        {
            var child = BoxFactory.Instance.CreateBox(Stream, Stream.Position);
            if (child is BoxESDS esds)
                ESDS = esds;
        }
    }

    public override string ToString() => $"{base.ToString()}: {Width}x{Height}, Compressor={CompressorName}";
}
