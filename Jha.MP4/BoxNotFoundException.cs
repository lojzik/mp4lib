using System;

namespace JHa.MP4;

[Serializable]
internal class BoxNotFoundException : Exception
{
    public BoxNotFoundException()
    {
    }

    public BoxNotFoundException(string? message) : base(message)
    {
    }

    public BoxNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}