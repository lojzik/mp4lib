using System;
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
        public void TestSmallm4v()
        {
            using var mp4file = MP4File.Parse("small.m4v");
            Assert.Equal<ulong>(123551, mp4file.MOOV.TRAKS[0].TotalSize);
            var memoryStream = new MemoryStream();
            mp4file.MOOV.TRAKS[0].Write(memoryStream);
            var expected = File.ReadAllBytes("small.m4v.stream1");
            var actual = memoryStream.ToArray();
            Assert.Equal(expected, actual);
        }
    }
}
