
using Jha.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jha.MP4;

[Box("ftyp")]
public class BoxFTYP(SubStream stream) : Box(stream)
{
    private String4 majorBrand;
    public String4 MajorBrand => majorBrand;
    public UInt32 MinorVersion { get; private set; }
    public List<String4> CompatibleBrands { get; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        Read(ref majorBrand);// = ReadString4();
        MinorVersion = ReadUInt32();
        while (Stream.Position < Stream.Length)
        {
            String4 string4 = new();
            Read(ref string4);
            CompatibleBrands.Add(string4);
            //                CompatibleBrands.Add(ReadString4().Trim());
        }
    }
    public override string ToString() => $"{base.ToString()}: {MajorBrand}, {MinorVersion}, {string.Join(", ", CompatibleBrands)}";
}
