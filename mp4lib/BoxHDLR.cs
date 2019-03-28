using System.IO;
using System.Linq;
using System.Text;

namespace JHa.MP4
{
    public class BoxHDLR : FullBox
    {
        public string ComponentType { get; private set; }
        public string ComponentSubtype { get; private set; }
        public string ComponentName { get; private set; }
        public BoxHDLR(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        protected override void ReadData()
        {
            base.ReadData();
            ComponentType = ReadString4();
            ComponentSubtype = ReadString4();
            var buffer = new byte[Size - 20];
            Stream.Read(buffer, 0, buffer.Length);
            ComponentName = Encoding.ASCII.GetString(buffer.Where(x => x != 0).ToArray()).Trim();
        }
        public override string ToString() => $"{base.ToString()}: {ComponentType}/{ComponentSubtype}/{ComponentName}";
    }
}
