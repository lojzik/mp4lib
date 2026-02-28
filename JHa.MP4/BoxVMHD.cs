
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("vmhd")]
public class BoxVMHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt16 GraphicsMode { get; private set; }
    public UInt16[] OpColor { get; private set; } = new UInt16[3];

    protected override void ReadData()
    {
        base.ReadData();
        GraphicsMode = (UInt16)ReadInt16();
        OpColor[0] = (UInt16)ReadInt16();
        OpColor[1] = (UInt16)ReadInt16();
        OpColor[2] = (UInt16)ReadInt16();
    }

    public override string ToString() => $"{base.ToString()}: GraphicsMode={GraphicsMode}, OpColor=[{OpColor[0]},{OpColor[1]},{OpColor[2]}]";
}
