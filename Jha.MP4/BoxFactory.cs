using JHa.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHa.MP4
{
    public class BoxFactory
    {
        Dictionary<String4, Type> BoxTypes { get; } = [];

        public static readonly BoxFactory Instance = new BoxFactory();
        public BoxFactory()
        {
            RegisterDefaultBoxTypes();
        }
        private void RegisterDefaultBoxTypes()
        {
            var boxType = typeof(Box);
            var boxTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(boxType) && !type.IsAbstract);
            foreach (var type in boxTypes)
            {
                var boxAttrs = Attribute.GetCustomAttributes(type, typeof(BoxAttribute), false).Cast<BoxAttribute>();
                foreach (var boxAttr in boxAttrs)
                {
                    BoxTypes[boxAttr.Token] = type;

                }
            }
        }
        private Type GetBoxType(String4 boxName)
        {
            if (BoxTypes.TryGetValue(boxName, out var type))
            {
                return type;
            }
            return typeof(Box);
        }
        public Box CreateBox(String4 boxname, SubStream stream)
        {
            var boxType = GetBoxType(boxname);
            var box = (Box)Activator.CreateInstance(boxType, stream)!;
            return box;
        }
        public Box CreateBox(Stream stream, long startIndex)
        {
            var position = stream.Position;
            try
            {
                stream.Position = startIndex + 4;
                String4 xtype;
                xtype.Token = 0;
                Span<byte> buffer = stackalloc byte[4];
                stream.ReadExactly(buffer);
                xtype.From(buffer);
                var substream = new SubStream(stream, startIndex);
                var box = CreateBox(xtype, substream);
                return box;
            }
            finally
            {
                stream.Position = position;
            }
        }
    }
}