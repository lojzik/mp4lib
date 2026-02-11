using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stsz")]
public class BoxSTSZ(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    private UInt32 _SampleSize { get; set; }
    public UInt32 SampleCount { get; private set; }
    private UInt32[] EntrySizeTable { get; set; } = [];
    public UInt64 TotalSize { get; private set; }
    protected override void ReadData()
    {
        base.ReadData();
        TotalSize = 0;
        _SampleSize = ReadUInt32();
        SampleCount = ReadUInt32();
        if (_SampleSize == 0)
        {
            EntrySizeTable = new UInt32[SampleCount];
            for (int i = 0; i < SampleCount; i++)
            {
                EntrySizeTable[i] = ReadUInt32();
                TotalSize += EntrySizeTable[i];
            }
        }
        else
        {
            TotalSize = SampleCount * _SampleSize;
        }
    }
    public override string ToString() => $"{base.ToString()}: SampleSize:{_SampleSize}, SampleCount:{SampleCount}";
    public UInt32 SampleSize(uint sample) => _SampleSize == 0 ? EntrySizeTable[sample] : _SampleSize;
}
