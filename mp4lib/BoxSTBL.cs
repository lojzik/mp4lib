
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTBL : BoxNested
    {
        public class ChunkToSample
        {
            public UInt32 SamplesCount { get; set; }
            public UInt32 SampleDescriptionIndex { get; set; }
            public UInt32 FirstSampleIndex { get; set; }
            public override string ToString() => $"samples: {SamplesCount}, sid{SampleDescriptionIndex}, first sample: {FirstSampleIndex}";

        }
        public class SampleEntry
        {
            public UInt32 ChunkIndex { get; set; }
            public UInt32 IndexInChunk { get; set; }
            public override string ToString() => $"chunk index: {ChunkIndex}, index in chunk: {IndexInChunk}";
        }

        public BoxSTBL(SubStream stream) : base(stream)
        {
            STTS = FindBox<BoxSTTS>("stts");
            STSC = FindBox<BoxSTSC>("stsc");
            STSD = FindBox<BoxSTSD>("stsd");
            STSZ = FindBox<BoxSTSZ>("stsz");
            STCO = FindBox<BoxSTCO>("stco");
            RestoreTables();
        }
        public BoxSTTS STTS { get; }
        public BoxSTSC STSC { get; }
        public BoxSTSD STSD { get; }
        public BoxSTSZ STSZ { get; }
        public BoxSTCO STCO { get; }
        public ChunkToSample[] ChunksToSamples { get; private set; } = Array.Empty<ChunkToSample>();
        public SampleEntry[] SampleEntries { get; private set; } = Array.Empty<SampleEntry>();
        public void ReadSample(UInt32 index, byte[] buffer, int offset)
        {
            var sampleEntry = SampleEntries[index];
            UInt32 seekInChunk = 0;
            for (uint i = 0; i < sampleEntry.IndexInChunk; i++)
            {
                seekInChunk += STSZ.SampleSize(index - i - 1);
            }
            uint dataIndex = STCO.Items[sampleEntry.ChunkIndex] + seekInChunk;
            var rootStream = Stream.RootStream();
            rootStream.Position = dataIndex;
            rootStream.Read(buffer, offset, (int)STSZ.SampleSize(index));
        }
        public void ReadSample(UInt32 index, Span<byte> buffer)
        {
            var sampleEntry = SampleEntries[index];
            UInt32 seekInChunk = 0;
            for (uint i = 0; i < sampleEntry.IndexInChunk; i++)
            {
                seekInChunk += STSZ.SampleSize(index - i - 1);
            }
            uint dataIndex = STCO.Items[sampleEntry.ChunkIndex] + seekInChunk;
            var rootStream = Stream.RootStream();
            rootStream.Position = dataIndex;
            rootStream.Read(buffer);
        }

        private void RestoreTables()
        {
            uint lastRealChunkNumber = STCO.EntryCount + 1;
            ChunksToSamples = new ChunkToSample[STCO.EntryCount];
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
}
