
using System;
using System.Collections.Generic;
using System.IO;

namespace JHa.MP4
{
    public class BoxFTYP : Box
    {
//        public string MajorBrand { get; private set; } = "XXXX";
        public String4 MajorBrand;
        public UInt32 MinorVersion { get; private set; }
        public List<String4> CompatibleBrands { get; } = new List<String4>();
        public BoxFTYP(SubStream stream) : base(stream)
        {
        }
        protected override void ReadData()
        {
            base.ReadData();
            Read(ref MajorBrand);// = ReadString4();
            MinorVersion = ReadUInt32();
            while (Stream.Position < Stream.Length)
            {
                String4 string4 = new String4();
                Read(ref string4);
                CompatibleBrands.Add(string4);
//                CompatibleBrands.Add(ReadString4().Trim());
            }
        }
        public override string ToString() => $"{base.ToString()}: {MajorBrand}, {MinorVersion}, {string.Join(", ",CompatibleBrands)}";
    }
}
