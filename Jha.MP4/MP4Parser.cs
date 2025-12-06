using Jha.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jha.MP4
{
    public class MP4Parser
    {
        Dictionary<String4, Type> BoxTypes { get; } = [];
        public void RegisterDefaultBoxTypes()
        {
            var boxType = typeof(Box);
            var boxTypes =  AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(boxType) && !type.IsAbstract);
            foreach (var type in boxTypes)
            {
                var boxAttr = (BoxAttribute?)Attribute.GetCustomAttribute(type, typeof(BoxAttribute));
                if (boxAttr is not null)
                {
                    BoxTypes[boxAttr.Token] = type;
                }
            }
        }
        public Type GetBoxType(String4 boxName)
        {
            if (BoxTypes.TryGetValue(boxName, out var type))
            {
                return type;
            }
            return typeof(Box);
        }
        public MP4Parser()
        {
            RegisterDefaultBoxTypes();
        }
        public Box CreateBox(SubStream stream)
        {
            var position = stream.Position;
            try
            {
                stream.Position = position + 4;
                String4 xtype;
                xtype.Token = 0;

//                unsafe
                {
                    throw new NotImplementedException();
//                    var x = new Span<byte>(xtype.ByteBuffer, 4);
//                    stream.ReadExactly(x);
                }
                var boxType = GetBoxType(xtype);
                var box = (Box)Activator.CreateInstance(boxType, stream)!;
                return box;
            }
            finally
            {
                stream.Position = position;
            }
        }
        public MP4File Parse(Stream stream)
        {
            return null;
//            return new MP4File(stream, this);
        }

    }
}
