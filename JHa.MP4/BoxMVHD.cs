
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mvhd")]
public class BoxMVHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt64 CreationTimeUnix { get; private set; }
    public UInt64 ModificationTimeUnix { get; private set; }
    public DateTimeOffset CreationTime => DateTimeOffset.FromUnixTimeSeconds((long)CreationTimeUnix);
    public DateTimeOffset ModificationTime => DateTimeOffset.FromUnixTimeSeconds((long)ModificationTimeUnix);
    public UInt32 Timescale { get; private set; }
    public UInt64 Duration { get; private set; }
    public UInt32 Rate { get; private set; }
    public UInt16 Volume { get; private set; }
    public UInt32 NextTrackID { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        if (Version == 1)
        {
            CreationTimeUnix = ReadUInt64();
            ModificationTimeUnix = ReadUInt64();
            Timescale = ReadUInt32();
            Duration = ReadUInt64();
        }
        else
        {
            CreationTimeUnix = ReadUInt32();
            ModificationTimeUnix = ReadUInt32();
            Timescale = ReadUInt32();
            Duration = ReadUInt32();
        }
        Rate = ReadUInt32();
        Volume = (UInt16)ReadInt16();
        ReadInt16(); // reserved
        ReadUInt32(); ReadUInt32(); // reserved
        // matrix[9] - 9 × Int32
        for (int i = 0; i < 9; i++) ReadInt32();
        // pre_defined[6] - 6 × UInt32
        for (int i = 0; i < 6; i++) ReadUInt32();
        NextTrackID = ReadUInt32();
    }

    public override string ToString() => $"{base.ToString()}: Timescale={Timescale}, Duration={Duration}, Rate=0x{Rate:X8}, Volume=0x{Volume:X4}, NextTrackID={NextTrackID}";
}
