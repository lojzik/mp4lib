using System.IO;

namespace JHa.MP4
{
    public class FullBox : Box
    {
        public FullBox(SubStream stream) : base(stream)
        {
        }
        public byte Version { get; private set; }
        public byte[] Flags { get; } = new byte[3];
        protected override void ReadData()
        {
            base.ReadData();
            Version = (byte)Stream.ReadByte();
            Stream.Read(Flags, 0, 3);
        }
        public override string ToString() => $"{base.ToString()}: version: {Version}";
    }
}
