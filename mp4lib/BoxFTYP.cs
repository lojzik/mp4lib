
using System;
using System.Collections.Generic;
using System.IO;

namespace JHa.MP4
{
    public class BoxFTYP : Box
    {
        public string MajorBrand { get; private set; }
        public UInt32 MinorVersion { get; private set; }
        public List<string> CompatibleBrands { get; } = new List<string>();
        public BoxFTYP(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        protected override void ReadData()
        {
            base.ReadData();
            MajorBrand = ReadString4();
            MinorVersion = ReadUInt32();
            while (Stream.Position < StartIndex + Size)
            {
                CompatibleBrands.Add(ReadString4().Trim());
            }
        }
        public override string ToString() => $"{base.ToString()}: {MajorBrand}, {MinorVersion}, {string.Join(", ",CompatibleBrands)}";
    }
}
