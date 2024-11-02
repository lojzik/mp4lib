using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JHa.MP4
{
    public class BoxNested : Box
    {
        public List<Box> Nested { get; } = new List<Box>();

        public BoxNested(SubStream stream) : base(stream)
        {
        }
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
        protected T FindBox<T>(string type) where T : Box => Nested.Where(x => x.Type == type).Cast<T>().SingleOrDefault();
        protected List<T> FindBoxMulti<T>(string type) where T : Box => Nested.Where(x => x.Type == type).Cast<T>().ToList();
    }
}
