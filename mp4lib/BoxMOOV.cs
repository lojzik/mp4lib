using System.Collections.Generic;
using System.IO;

namespace JHa.MP4
{
    public class BoxMOOV : BoxNested
    {
        public BoxMOOV(SubStream stream) : base(stream)
        {
            TRAKS = FindBoxMulti<BoxTRAK>("trak");
        }
        public IList<BoxTRAK> TRAKS { get; }
    }
}
