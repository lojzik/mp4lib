using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("tkhd")]
public class BoxTKHD(SubStream stream) : FullBox<BoxTKHD.TkhdFlags>(stream)
{
    [Flags]
    public enum  TkhdFlags:uint
    {
        None = 0x000000,
        TrackEnabled = 0x000001,
        TrackIsMovie = 0x000002,
        TrackisPreview = 0x000004,
    }

    public UInt64 CreationTimeUnix { get; private set; }
    public UInt64 ModificationTimeUnix { get; private set; }
    public DateTimeOffset CreationTime => DateTimeOffset.FromUnixTimeSeconds((long)CreationTimeUnix);
    public DateTimeOffset ModificationTime => DateTimeOffset.FromUnixTimeSeconds((long)ModificationTimeUnix);
    public UInt32 TrackId { get; private set; }
    public UInt64 Duration { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        if (Version == 1)
        {
            CreationTimeUnix = ReadUInt64();
            ModificationTimeUnix = ReadUInt64();
            TrackId = ReadUInt32();
            ReadUInt32(); //reserved
            Duration = ReadUInt64();
        }
        else
        {
            CreationTimeUnix = ReadUInt32();
            ModificationTimeUnix = ReadUInt32();
            TrackId = ReadUInt32();
            ReadUInt32();//reserved
            Duration = ReadUInt32();
        }
        //todo: read next params
    }
    public override string ToString() => $"{base.ToString()}: TrackId:{TrackId}, Duration: {Duration}, CT: {CreationTime}, MT:{ModificationTime}";
}
