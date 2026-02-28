using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using JHa.MP4;
using Xunit;
using Xunit.Abstractions;

namespace mp4test
{
    public class MP4Test(ITestOutputHelper output)
    {
        //protected readonly ITestOutputHelper output = output;

        [Theory]
        [InlineData("Mpeg4.mp4", "isom", 512, 0, 4)]
        [InlineData("small.m4v", "M4V ", 512, 2, 3)]
        public void ParseHeaders(string filename, string majorBrand, UInt32 minorVersion, int trakCount, int compatibleBrandsCount)
        {
            output.WriteLine($"filename: {filename}");
            using var mp4file = MP4File.Parse(filename);
            Assert.Equal(majorBrand, mp4file.FTYP.MajorBrand);
            Assert.Equal(minorVersion, mp4file.FTYP.MinorVersion);
            Assert.Equal(compatibleBrandsCount, mp4file.FTYP.CompatibleBrands.Count);
            Assert.Equal(trakCount, mp4file.MOOV.TRAKS.Count);
            output.WriteLine($"majorbrand: {mp4file.FTYP.MajorBrand}");
            output.WriteLine($"minorVersion: {mp4file.FTYP.MinorVersion}");
            output.WriteLine($"tracks: {mp4file.MOOV.TRAKS.Count}");
            foreach (var trak in mp4file.MOOV.TRAKS)
            {
                output.WriteLine($"trak: {trak.TrackId}, {trak.StreamName}, samples: {trak.SampleCount}, hdlr: {trak.MDIA.HDLR.ComponentType}, {trak.MDIA.HDLR.ComponentSubtype},{trak.MDIA.HDLR.ComponentName} ");
            }
        }
        [Fact]
        public void Smallm4v_expectedTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            Assert.Equal<ulong>(123551, mp4file.MOOV.TRAKS[0].TotalSize);
            var memoryStream = new MemoryStream();
            mp4file.MOOV.TRAKS[0].Write(memoryStream);
            var expected = File.ReadAllBytes("small.m4v.stream1");
            var actual = memoryStream.ToArray();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Moof10Fragments_verifyStructure()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            Assert.Equal(10, mp4file.MOOFs.Count);
            Assert.Equal(2, mp4file.MOOFs[0].TRAFs.Count);
            Assert.Equal(2, mp4file.MOOFs[9].TRAFs.Count);
            Assert.Equal(2, mp4file.MOOFs[0].TRAFs[0].TRUNs.Count);
            Assert.Single(mp4file.MOOFs[9].TRAFs[1].TRUNs);
            Assert.Equal((UInt32)1, mp4file.MOOFs[0].MFHD.SequenceNumber);
            Assert.Equal((UInt32)25, mp4file.MOOFs[0].TRAFs[0].TRUNs[0].SampleCount);
            Assert.Equal((UInt32)1919, mp4file.MOOFs[0].TRAFs[0].TRUNs[0].Samples[0].Size);
            Assert.Equal((UInt32)171, mp4file.MOOFs[0].TRAFs[0].TRUNs[0].Samples[2].Size);
            Assert.Equal((UInt32)1000, mp4file.MOOFs[0].TRAFs[0].TRUNs[0].Samples[0].UCompositionTimeOffset);
            Assert.Equal((UInt32)0, mp4file.MOOFs[0].TRAFs[0].TRUNs[0].Samples[2].UCompositionTimeOffset);


        }
        [Theory]
        [InlineData("Mpeg4.mp4")]
        [InlineData("small.m4v")]
        [InlineData("10_fragments.mp4")]
        [InlineData("sample_mpeg4.mp4")]
        public void TestParse(string filename)
        {
            using var mp4file = MP4File.Parse(filename);
            display(mp4file.Nested, "");
        }

        // --- Testy pro BoxMVHD ---
        [Theory]
        [InlineData("small.m4v", 1000U, 5599UL, 3U)]
        [InlineData("10_fragments.mp4", 600U, 600UL, 202U)]
        [InlineData("sample_mpeg4.mp4", 1000U, 13347UL, 2U)]
        public void Mvhd_parsesCorrectly(string filename, uint expectedTimescale, ulong expectedDuration, uint expectedNextTrackID)
        {
            using var mp4file = MP4File.Parse(filename);
            var mvhd = mp4file.MOOV.MVHD;
            Assert.Equal(expectedTimescale, mvhd.Timescale);
            Assert.Equal(expectedDuration, mvhd.Duration);
            Assert.Equal(expectedNextTrackID, mvhd.NextTrackID);
            Assert.Equal(0x00010000U, mvhd.Rate);
            Assert.Equal((ushort)0x0100, mvhd.Volume);
        }

        // --- Testy pro BoxMDHD ---
        [Fact]
        public void Mdhd_smallm4v_videoTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var mdhd = mp4file.MOOV.TRAKS[0].MDIA.MDHD;
            Assert.Equal(15360U, mdhd.Timescale);
            Assert.Equal(84992UL, mdhd.Duration);
            Assert.Equal("und", mdhd.Language);
        }

        [Fact]
        public void Mdhd_smallm4v_audioTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var mdhd = mp4file.MOOV.TRAKS[1].MDIA.MDHD;
            Assert.Equal(48000U, mdhd.Timescale);
            Assert.Equal(268720UL, mdhd.Duration);
            Assert.Equal("eng", mdhd.Language);
        }

        // --- Testy pro BoxVMHD ---
        [Theory]
        [InlineData("small.m4v")]
        [InlineData("sample_mpeg4.mp4")]
        public void Vmhd_videoTrackPresent(string filename)
        {
            using var mp4file = MP4File.Parse(filename);
            var videoTrack = mp4file.MOOV.TRAKS.First(t => t.MDIA.HDLR.ComponentSubtype.Equals("vide"));
            var vmhd = videoTrack.MDIA.MINF.VMHD;
            Assert.NotNull(vmhd);
            Assert.Equal((ushort)0, vmhd.GraphicsMode);
        }

        // --- Testy pro BoxSMHD ---
        [Fact]
        public void Smhd_audioTrackPresent()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var audioTrack = mp4file.MOOV.TRAKS.First(t => t.MDIA.HDLR.ComponentSubtype.Equals("soun"));
            var smhd = audioTrack.MDIA.MINF.SMHD;
            Assert.NotNull(smhd);
            Assert.Equal((short)0, smhd.Balance);
        }

        // --- Testy pro BoxNMHD ---
        [Fact]
        public void Nmhd_presentInSystemsTrack()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            // First track is BIFS handler (sdsm) → nmhd
            var nmhd = mp4file.MOOV.TRAKS[0].MDIA.MINF.NMHD;
            Assert.NotNull(nmhd);
        }

        // --- Testy pro BoxDREF ---
        [Theory]
        [InlineData("small.m4v")]
        [InlineData("10_fragments.mp4")]
        public void Dref_parsesEntries(string filename)
        {
            using var mp4file = MP4File.Parse(filename);
            foreach (var trak in mp4file.MOOV.TRAKS)
            {
                var dinf = trak.MDIA.MINF.STBL; // access stbl to ensure parsing
                // dref is inside dinf — traverse via Nested
                var minfNested = (BoxNested)trak.MDIA.MINF;
                var dinfBox = minfNested.Nested.FirstOrDefault(b => b.Type.Equals(BoxName.dinf));
                Assert.NotNull(dinfBox);
                var dinf2 = (BoxNested)dinfBox;
                var drefBox = dinf2.Nested.FirstOrDefault(b => b.Type.Equals(BoxName.dref));
                Assert.NotNull(drefBox);
                var dref = (BoxDREF)drefBox;
                Assert.True(dref.EntryCount > 0);
                Assert.Equal((int)dref.EntryCount, dref.Entries.Count);
            }
        }

        // --- Testy pro BoxCTTS ---
        [Fact]
        public void Ctts_smallm4v_videoTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stbl = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL;
            var cttsBox = stbl.Nested.FirstOrDefault(b => b.Type.Equals(BoxName.ctts));
            Assert.NotNull(cttsBox);
            var ctts = (BoxCTTS)cttsBox;
            Assert.Equal(163U, ctts.EntryCount);
            Assert.True(ctts.Items.Length > 0);
            // First entry should have valid values
            Assert.True(ctts.Items[0].SampleCount > 0);
        }

        [Fact]
        public void Ctts_sampleMpeg4_videoTrack()
        {
            using var mp4file = MP4File.Parse("sample_mpeg4.mp4");
            var stbl = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL;
            var cttsBox = stbl.Nested.FirstOrDefault(b => b.Type.Equals(BoxName.ctts));
            Assert.NotNull(cttsBox);
            var ctts = (BoxCTTS)cttsBox;
            Assert.Equal(392U, ctts.EntryCount);
        }

        // --- Testy pro BoxMVEX, BoxTREX ---
        [Fact]
        public void Mvex_10fragments_hasTrexEntries()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            var mvex = mp4file.MOOV.MVEX;
            Assert.NotNull(mvex);
            Assert.Equal(2, mvex.TREXs.Count);
            Assert.Equal(201U, mvex.TREXs[0].TrackID);
            Assert.Equal(101U, mvex.TREXs[1].TrackID);
            Assert.Equal(1U, mvex.TREXs[0].DefaultSampleDescriptionIndex);
        }

        // --- Testy pro BoxMP4A, BoxMP4V, BoxMP4S, BoxESDS ---
        [Fact]
        public void SampleEntries_10fragments_mp4sPresent()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            // Track 0 (BIFS) has mp4s
            var stsd = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL.STSD;
            Assert.Equal(1U, stsd.EntryCount);
            Assert.IsType<BoxMP4S>(stsd.Items[0]);
            var mp4s = (BoxMP4S)stsd.Items[0];
            Assert.NotNull(mp4s.ESDS);
            Assert.True(mp4s.ESDS.ESDescriptorData.Length > 0);
        }

        [Fact]
        public void SampleEntries_10fragments_mp4vPresent()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            // Track 2 (ISO Video Handler, TrackID=201)
            var videoTrack = mp4file.MOOV.TRAKS.First(t => t.TrackId == 201);
            var stsd = videoTrack.MDIA.MINF.STBL.STSD;
            Assert.Equal(1U, stsd.EntryCount);
            Assert.IsType<BoxMP4V>(stsd.Items[0]);
            var mp4v = (BoxMP4V)stsd.Items[0];
            Assert.NotNull(mp4v.ESDS);
            Assert.True(mp4v.ESDS.ESDescriptorData.Length > 0);
        }

        [Fact]
        public void SampleEntries_10fragments_mp4aPresent()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            // Track 3 (ISO Audio Handler, TrackID=101)
            var audioTrack = mp4file.MOOV.TRAKS.First(t => t.TrackId == 101);
            var stsd = audioTrack.MDIA.MINF.STBL.STSD;
            Assert.Equal(1U, stsd.EntryCount);
            Assert.IsType<BoxMP4A>(stsd.Items[0]);
            var mp4a = (BoxMP4A)stsd.Items[0];
            Assert.True(mp4a.ChannelCount > 0);
            Assert.True(mp4a.SampleRate > 0);
            Assert.NotNull(mp4a.ESDS);
        }

        // --- Testy pro existující netestované boxy ---

        // BoxTKHD
        [Fact]
        public void Tkhd_smallm4v_trackProperties()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var tkhd = mp4file.MOOV.TRAKS[0].TKHD;
            Assert.Equal(1U, tkhd.TrackId);
            Assert.Equal(5534UL, tkhd.Duration);
            Assert.True(tkhd.Flags.HasFlag(BoxTKHD.TkhdFlags.TrackEnabled));
            Assert.True(tkhd.Flags.HasFlag(BoxTKHD.TkhdFlags.TrackIsMovie));
        }

        // BoxTFHD
        [Fact]
        public void Tfhd_10fragments_trackId()
        {
            using var mp4file = MP4File.Parse("10_fragments.mp4");
            var tfhd = mp4file.MOOFs[0].TRAFs[0].Nested
                .First(b => b.Type.Equals(BoxName.tfhd));
            Assert.IsType<BoxTFHD>(tfhd);
            Assert.Equal(201U, ((BoxTFHD)tfhd).TrackId);
        }

        // BoxSTTS
        [Fact]
        public void Stts_smallm4v_videoTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stts = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL.STTS;
            Assert.Equal(1U, stts.EntryCount);
            Assert.Equal(166U, stts.Items[0].Count);
            Assert.Equal(512U, stts.Items[0].Delta);
        }

        [Fact]
        public void Stts_smallm4v_audioTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stts = mp4file.MOOV.TRAKS[1].MDIA.MINF.STBL.STTS;
            Assert.Equal(45U, stts.EntryCount);
            Assert.True(stts.Items[0].Count > 0);
            Assert.True(stts.Items[0].Delta > 0);
        }

        // BoxSTSC
        [Fact]
        public void Stsc_smallm4v_videoTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stsc = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL.STSC;
            Assert.Equal(4U, stsc.EntryCount);
            Assert.Equal(1U, stsc.Items[0].FirstChunk);
            Assert.True(stsc.Items[0].SamplesPerChunk > 0);
            Assert.Equal(1U, stsc.Items[0].SampleDescriptionIndex);
        }

        // BoxSTSD
        [Fact]
        public void Stsd_smallm4v_entryCount()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            foreach (var trak in mp4file.MOOV.TRAKS)
            {
                var stsd = trak.MDIA.MINF.STBL.STSD;
                Assert.Equal(1U, stsd.EntryCount);
                Assert.Single(stsd.Items);
            }
        }

        // BoxSTSZ
        [Fact]
        public void Stsz_smallm4v_videoTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stsz = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL.STSZ;
            Assert.Equal(166U, stsz.SampleCount);
            Assert.Equal(123551UL, stsz.TotalSize);
            // Individual sample sizes
            Assert.True(stsz.SampleSize(0) > 0);
        }

        // BoxSTCO
        [Fact]
        public void Stco_smallm4v_videoTrack()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stco = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL.STCO;
            Assert.NotNull(stco);
            Assert.Equal(164U, stco.EntryCount);
            Assert.True(stco.Items[0] > 0);
        }

        // BoxSTSS
        [Fact]
        public void Stss_smallm4v_syncSamples()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            var stbl = mp4file.MOOV.TRAKS[0].MDIA.MINF.STBL;
            var stssBox = stbl.Nested.FirstOrDefault(b => b.Type.Equals(BoxName.stss));
            Assert.NotNull(stssBox);
            var stss = (BoxSTSS)stssBox;
            Assert.Equal(1U, stss.EntryCount);
            Assert.True(stss.Items[0] > 0);
        }

        // BoxELST
        [Fact]
        public void Elst_smallm4v_editList()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            foreach (var trak in mp4file.MOOV.TRAKS)
            {
                var edts = (BoxNested)trak.MDIA; // navigate to trak level
                var edtsBox = ((BoxNested)(Box)trak).Nested.FirstOrDefault(b => b.Type.Equals(BoxName.edts));
                Assert.NotNull(edtsBox);
                var elstBox = ((BoxNested)edtsBox).Nested.FirstOrDefault(b => b.Type.Equals(BoxName.elst));
                Assert.NotNull(elstBox);
                var elst = (BoxELST)elstBox;
                Assert.Equal(1U, elst.EntryCount);
                Assert.True(elst.Items[0].SegmentDuration > 0);
            }
        }


        private void display(List<Box> nested, string odsazeni)
        {
            foreach (var box in nested)
            {
                display(box, odsazeni);
            }
        }

        private void display(Box box, string odsazeni)
        {
            output.WriteLine(odsazeni + box.ToString());
            if (box is BoxNested n)
                display(n.Nested, odsazeni + "  ");
        }
    }
}
