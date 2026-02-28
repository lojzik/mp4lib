
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mdhd")]
public class BoxMDHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt64 CreationTimeUnix { get; private set; }
    public UInt64 ModificationTimeUnix { get; private set; }
    public DateTimeOffset CreationTime => DateTimeOffset.FromUnixTimeSeconds((long)CreationTimeUnix);
    public DateTimeOffset ModificationTime => DateTimeOffset.FromUnixTimeSeconds((long)ModificationTimeUnix);
    public UInt32 Timescale { get; private set; }
    public UInt64 Duration { get; private set; }
    public UInt16 LanguagePacked { get; private set; }
    public string Language => DecodeLang(LanguagePacked);

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
        LanguagePacked = (UInt16)ReadInt16();
        ReadInt16(); // pre_defined
    }

    private static string DecodeLang(UInt16 packed)
    {
        char c1 = (char)(((packed >> 10) & 0x1F) + 0x60);
        char c2 = (char)(((packed >> 5) & 0x1F) + 0x60);
        char c3 = (char)((packed & 0x1F) + 0x60);
        return new string(new[] { c1, c2, c3 });
    }

    public override string ToString() => $"{base.ToString()}: Timescale={Timescale}, Duration={Duration}, Language={Language}";
}
