using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Jha.MP4;
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
        public void TestParse(string filename)
        {
            using var mp4file = MP4File.Parse(filename);
            display(mp4file.Nested, "");
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
