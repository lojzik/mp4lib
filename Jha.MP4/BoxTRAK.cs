using Jha.Common;
using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;

namespace Jha.MP4;

public class BoxTRAK(SubStream stream) : BoxNested(stream)
{
    public BoxMDIA MDIA => FindBox<BoxMDIA>(BoxName.mdia) ?? throw new BoxNotFoundException("mdia");
    public BoxTKHD TKHD => FindBox<BoxTKHD>(BoxName.tkhd) ?? throw new BoxNotFoundException("tkhd");
    public string StreamName => MDIA.HDLR.ComponentName;
    public UInt32 TrackId => TKHD.TrackId;
    public override string ToString() => $"{base.ToString()}: {TrackId} {StreamName}";
    public UInt32 SampleCount => MDIA.MINF.STBL.STSZ.SampleCount;
    public UInt32 SampleSize(uint index) => MDIA.MINF.STBL.STSZ.SampleSize(index);
    public void ReadSample(uint index, byte[] buffer, int offset) => MDIA.MINF.STBL.ReadSample(index, buffer, offset);
    public void ReadSample(uint index, Span<byte> buffer) => MDIA.MINF.STBL.ReadSample(index, buffer);
    public UInt64 TotalSize => MDIA.MINF.STBL.STSZ.TotalSize;
    public void WriteTo(uint index, Stream stream)
    {
        var sampleSize = SampleSize(index);
        var buffer = ArrayPool<byte>.Shared.Rent((int)sampleSize);//   new byte[sampleSize];
        try
        {
            ReadSample(index, buffer, 0);
            stream.Write(buffer, 0, (int)sampleSize);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    public void Write(Stream stream)
    {
        var sampleCount = SampleCount;
        for (uint i = 0; i < sampleCount; i++)
            WriteTo(i, stream);
    }

}
