using Jha.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Jha.MP4;

[Box("hdlr")]
public class BoxHDLR(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public String4 componentType;
    public String4 componentSubtype;

    public String4 ComponentType => componentType;
    public String4 ComponentSubtype => componentSubtype;
    public string ComponentName { get; private set; } = "";

    protected override void ReadData()
    {
        base.ReadData();
        Read(ref componentType);// = ReadString4();
        Read(ref componentSubtype);// = ReadString4();
        //var buffer = new byte[Stream.Length - 20];
        int length = (int)Stream.Length - 20;
        Span<byte> buff = stackalloc byte[length];
        Stream.ReadExactly(buff);
        //todo: check in doc
        int start = 0;
        while (buff[start] <= 32 && start < length)
            start++;
        length -= start;
        while (buff[start + length - 1] <= 32 && length > 0)
            length--;
        ComponentName = Encoding.ASCII.GetString(buff.Slice(start, length));
        //Stream.Read(buffer, 0, buffer.Length);
        //ComponentName = Encoding.ASCII.GetString(buffer.Where(x => x != 0).ToArray()).Trim();
    }
    public override string ToString() => $"{base.ToString()}: {ComponentType}/{ComponentSubtype}/{ComponentName}";
}
