using JHa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHa.MP4;

[Box("trun")]
public class BoxTRUN(SubStream stream) : FullBox<BoxTRUN.TrunFlags>(stream)
{
    [Flags]
    public enum TrunFlags : uint
    {
        None = 0,
        DataOffsetPresent = 0x000001,
        FirstSampleFlagsPresent = 0x000004,
        SampleDurationPresent = 0x000100,
        SampleSizePresent = 0x000200,
        SampleFlagsPresent = 0x000400,
        SampleCompositionTimeOffsetsPresent = 0x000800,
    }
    public struct SampleEntry
    {
        public UInt32? Duration;
        public UInt32? Size;
        public UInt32? Flags;
        public Int32? CompositionTimeOffset;
        public UInt32? UCompositionTimeOffset;
    }
    public UInt32 SampleCount { get; private set; }
    public Int32 DataOffset { get; private set; }
    public UInt32 FirstSampleCount { get; private set; }
    public SampleEntry[] Samples { get; private set; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        SampleCount = ReadUInt32();
        if (Flags.HasFlag(TrunFlags.DataOffsetPresent))
        {
            DataOffset = ReadInt32();
        }
        if (Flags.HasFlag(TrunFlags.FirstSampleFlagsPresent))
        {
            FirstSampleCount = ReadUInt32();
        }
        
        Samples = new SampleEntry[SampleCount];
        for (int i = 0; i < SampleCount; i++) {
            var sample = new SampleEntry();
            if (Flags.HasFlag(TrunFlags.SampleDurationPresent))
            {
                sample.Duration = ReadUInt32();
            }
            if (Flags.HasFlag(TrunFlags.SampleSizePresent))
            {
                sample.Size = ReadUInt32();
            }
            if (Flags.HasFlag(TrunFlags.SampleFlagsPresent))
            {
                sample.Flags = ReadUInt32();
            }
            if(Flags.HasFlag(TrunFlags.SampleCompositionTimeOffsetsPresent))
            {
                if (Version == 0)
                {
                    sample.UCompositionTimeOffset = ReadUInt32();
                }
                else
                {
                    sample.CompositionTimeOffset = ReadInt32();
                }
            }
            Samples[i] = sample;
        }

    }
    public override string ToString() => $"{base.ToString()}: SC={SampleCount},DO={DataOffset}, FSC={FirstSampleCount}";
}
