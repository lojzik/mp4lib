using JHa.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JHa.MP4;

[Box("edts")]
[Box("udta")]
[Box("dinf")]
public class BoxNested(SubStream stream) : Box(stream)
{
    public List<Box> Nested { get; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        var currentPosition = Stream.Position;
        do
        {
            var box = BoxFactory.Instance.CreateBox(Stream, currentPosition);
            Nested.Add(box);
            currentPosition += box.Size;

        } while (currentPosition < Size);
    }
    protected T? FindBox<T>(String4 type) where T : Box => Nested.Where(x => x.Type.Equals(type)).Cast<T>().SingleOrDefault();
    protected T FindMandatoryBox<T>(String4 type) where T : Box => Nested.Where(x => x.Type.Equals(type)).Cast<T>().SingleOrDefault() ?? throw new BoxNotFoundException(type);
    protected List<T> FindBoxMulti<T>(String4 type) where T : Box => [.. Nested.Where(x => x.Type.Equals(type)).Cast<T>()];
}
