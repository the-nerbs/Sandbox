using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEResEnum
{
    internal static class ExtensionMethods
    {
        public static T Read<T>(this Stream stream)
        {
            int len = Marshal.SizeOf(typeof(T));

            var bytes = new byte[len];
            if (stream.Read(bytes, 0, len) != len)
            {
                throw new IOException("Not enough data in the stream");
            }

            var hBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(hBytes.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                hBytes.Free();
            }
        }
    }
}
