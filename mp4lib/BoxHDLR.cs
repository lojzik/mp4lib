using System;
using System.IO;
using System.Linq;
using System.Text;

namespace JHa.MP4
{
    public class BoxHDLR : FullBox
    {
//        public string ComponentType { get; private set; } = "XXXX";
//        public string ComponentSubtype { get; private set; } = "XXXX";
        public String4 ComponentType;
        public String4 ComponentSubtype;
        public string ComponentName { get; private set; } = "";
        public BoxHDLR(SubStream stream) : base(stream)
        {
        }
        protected override void ReadData()
        {
            base.ReadData();
            Read(ref ComponentType);// = ReadString4();
            Read(ref ComponentSubtype);// = ReadString4();
            //var buffer = new byte[Stream.Length - 20];
            int length = (int)Stream.Length - 20;
            Span<byte> buff = stackalloc byte[length];
            Stream.Read(buff);
            int start = 0;
            while(buff[start] <= 32 && start < length)
              start++;
            length -= start;
            while(buff[start+length-1] <= 32 && length > 0)
              length--;
            ComponentName = Encoding.ASCII.GetString(buff.Slice(start,length));
            //Stream.Read(buffer, 0, buffer.Length);
            //ComponentName = Encoding.ASCII.GetString(buffer.Where(x => x != 0).ToArray()).Trim();
        }
        public override string ToString() => $"{base.ToString()}: {ComponentType}/{ComponentSubtype}/{ComponentName}";
    }
}
