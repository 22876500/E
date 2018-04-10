using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace AASDataServer.Helper
{
    public class ProtoBufHelper
    {
        public static byte[] toByteArray<T>(T o)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, o);
                byte[] tmp = stream.ToArray();
                return tmp;
            }
        }

        public static T fromByteArray<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                T tmp = Serializer.Deserialize<T>(stream);
                return tmp;
            }
        }
    }
}
