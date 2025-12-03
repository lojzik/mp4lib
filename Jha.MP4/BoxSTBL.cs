
using System;
using System.IO;
using Jha.Common;

namespace Jha.MP4;

public class BoxSTBL : BoxNested
{
    public struct ChunkToSample
    {
        public UInt32 SamplesCount { get; set; }
        public UInt32 SampleDescriptionIndex { get; set; }
        public UInt32 FirstSampleIndex { get; set; }
        public override readonly string ToString() => $"samples: {SamplesCount}, sid{SampleDescriptionIndex}, first sample: {FirstSampleIndex}";

    }
    public struct SampleEntry
    {
        public UInt32 ChunkIndex { get; set; }
        public UInt32 IndexInChunk { get; set; }
        public override readonly string ToString() => $"chunk index: {ChunkIndex}, index in chunk: {IndexInChunk}";
    }

    public BoxSTBL(SubStream stream) : base(stream)
    {
        STTS = FindBox<BoxSTTS>(BoxName.stts) ?? throw new BoxNotFoundException("stts");
        STSC = FindBox<BoxSTSC>(BoxName.stsc) ?? throw new BoxNotFoundException("stsc");
        STSD = FindBox<BoxSTSD>(BoxName.stsd) ?? throw new BoxNotFoundException("stsd");
        STSZ = FindBox<BoxSTSZ>(BoxName.stsz) ?? throw new BoxNotFoundException("stsz");
        STCO = FindBox<BoxSTCO>(BoxName.stco);
        CO64 = FindBox<BoxCO64>(BoxName.co64);
        if(STCO == null && CO64 == null)
            throw new BoxNotFoundException("stco/co64");
        RestoreTables();
    }
    public BoxSTTS STTS { get; }
    public BoxSTSC STSC { get; }
    public BoxSTSD STSD { get; }
    public BoxSTSZ STSZ { get; }
    public BoxSTCO? STCO { get; }
    public BoxCO64? CO64 { get; }
    public ChunkToSample[] ChunksToSamples { get; private set; } = [];
    public SampleEntry[] SampleEntries { get; private set; } = [];
    public void ReadSample(UInt32 index, byte[] buffer, int offset)
    {
        var sampleEntry = SampleEntries[index];
        UInt32 seekInChunk = 0;
        for (uint i = 0; i < sampleEntry.IndexInChunk; i++)
        {
            seekInChunk += STSZ.SampleSize(index - i - 1);
        }
        ulong dataIndex =  CoDataIndex(sampleEntry.ChunkIndex) + seekInChunk;
        var rootStream = Stream.RootStream();
        rootStream.Position = (long)dataIndex;
        rootStream.ReadExactly(buffer, offset, (int)STSZ.SampleSize(index));
    }
    public void ReadSample(UInt32 index, Span<byte> buffer)
    {
        var sampleEntry = SampleEntries[index];
        UInt32 seekInChunk = 0;
        for (uint i = 0; i < sampleEntry.IndexInChunk; i++)
        {
            seekInChunk += STSZ.SampleSize(index - i - 1);
        }
        ulong dataIndex = CoDataIndex(sampleEntry.ChunkIndex) + seekInChunk;
        var rootStream = Stream.RootStream();
        rootStream.Position = (long)dataIndex;
        rootStream.ReadExactly(buffer);
    }

    private uint CoEntryCount()
    {
        return STCO?.EntryCount ?? CO64?.EntryCount ?? throw new InvalidOperationException("STCO/CO64 not exists");
    }
    private ulong CoDataIndex(uint index)
    {
        return STCO?.Items[index] ?? CO64?.Items[index] ?? throw new InvalidOperationException("STCO/CO64 not exists");
    }
    private void RestoreTables()
    {
        uint lastRealChunkNumber = CoEntryCount() + 1;
        ChunksToSamples = new ChunkToSample[CoEntryCount()];
        var items = STSC.Items;
        for (int i = items.Length - 1; i >= 0; i--)
        {
            var item = STSC.Items[i];
            uint begRealChunkNumer = item.FirstChunk;
            for (uint chki = begRealChunkNumer - 1; chki < lastRealChunkNumber - 1; ++chki)
            {
                ChunksToSamples[chki] = new ChunkToSample()
                {
                    SamplesCount = item.SamplesPerChunk,
                    SampleDescriptionIndex = item.SampleDescriptionIndex
                };
            }
            lastRealChunkNumber = begRealChunkNumer;
        }
        UInt32 sampleIndex = 0;
        SampleEntries = new SampleEntry[STSZ.SampleCount];
        for (uint i = 0; i < ChunksToSamples.Length; i++)
        {
            ChunksToSamples[i].FirstSampleIndex = sampleIndex;
            UInt32 indexInChunk = 0;
            for (uint sami = 0; sami != ChunksToSamples[i].SamplesCount; sami++)
            {
                SampleEntries[sampleIndex] = new SampleEntry()
                {
                    ChunkIndex = i,
                    IndexInChunk = indexInChunk,
                };
                sampleIndex++;
                indexInChunk++;
            }
        }
    }
}
