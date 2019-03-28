using System.Collections.Generic;
using System.IO;

namespace JHa.MP4
{
    public class BoxMOOV : BoxNested
    {
        public BoxMOOV(Stream stream, long startIndex) : base(stream, startIndex)
        {
            TRAKS = FindBoxMulti<BoxTRAK>("trak");
        }
        public IList<BoxTRAK> TRAKS { get; }
    }
}
