
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("trex")]
public class BoxTREX(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt32 TrackID { get; private set; }
    public UInt32 DefaultSampleDescriptionIndex { get; private set; }
    public UInt32 DefaultSampleDuration { get; private set; }
    public UInt32 DefaultSampleSize { get; private set; }
    public UInt32 DefaultSampleFlags { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        TrackID = ReadUInt32();
        DefaultSampleDescriptionIndex = ReadUInt32();
        DefaultSampleDuration = ReadUInt32();
        DefaultSampleSize = ReadUInt32();
        DefaultSampleFlags = ReadUInt32();
    }

    public override string ToString() => $"{base.ToString()}: TrackID={TrackID}, DefaultSampleDescriptionIndex={DefaultSampleDescriptionIndex}";
}
