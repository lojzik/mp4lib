using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Jha;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public unsafe struct String4
{
    [FieldOffset(0)]
    public uint Token;
    [FieldOffset(0)]
    public fixed byte ByteBuffer[4];
    public string AsString
    {
        readonly get
        {
            Span<char> buff = stackalloc char[4];
            for (int i = 0; i < 4; i++)
                buff[i] = (char)ByteBuffer[i];
            return string.Intern(new string(buff));
        }
        set
        {
            if (value.Length != 4)
                throw new ArgumentException($"invalid length {value.Length}");
            for (int i = 0; i < 4; i++)
            {
                if (value[i] > '\xFF')
                    throw new ArgumentException($"invalid value '{value}'");
            }
            for (int i = 0; i < 4; i++)
            {
                ByteBuffer[i] = (byte)(value[i]);
            }
        }
    }
    public void From(ReadOnlySpan<byte> value)
    {
        if (value.Length != 4)
            throw new ArgumentException($"invalid length {value.Length}");
        for(int i = 0 ; i < 4 ; i++)
        ByteBuffer[i] = value[i];
    }
    public override readonly string ToString() => AsString;
    public override readonly int GetHashCode() => (int)Token;
    public static implicit operator String4(string str) => new() { AsString = str };
    public static implicit operator uint(String4 box) => box.Token;
    public static implicit operator string(String4 box) => box.AsString;
    public override readonly bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (obj is String4 str4)
            return Token == str4.Token;
        if (obj is string str)
        {
            return this == str;
        }
        return false;
    }
    public static bool operator ==(String4 string4, string str)
    {
        if (str.Length != 4)
            return false;
        for (int i = 0; i < 4; i++)
        {
            if ((byte)(string4.ByteBuffer[i]) != str[i])
                return false;
        }
        return true;
    }
    public static bool operator !=(String4 string4, string str)
    {
        if (str.Length != 4)
            return true;
        for (int i = 0; i < 4; i++)
        {
            if ((byte)(string4.ByteBuffer[i]) != str[i])
                return true;
        }
        return false;
    }
}