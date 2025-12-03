using Jha.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jha.MP4;

public class BoxNested(SubStream stream) : Box(stream)
{
    public List<Box> Nested { get; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        var currentPosition = Stream.Position;
        do
        {
            var box = CreateBox(Stream, currentPosition);
            Nested.Add(box);
            currentPosition += box.Size;

        } while (currentPosition < Size);
    }
    protected T? FindBox<T>(String4 type) where T : Box => Nested.Where(x => x.Type.Equals(type)).Cast<T>().SingleOrDefault();
    protected List<T> FindBoxMulti<T>(String4 type) where T : Box => [.. Nested.Where(x => x.Type.Equals(type)).Cast<T>()];
}
