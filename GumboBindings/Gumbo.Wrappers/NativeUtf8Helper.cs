using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class NativeUtf8Helper
    {
        /// <summary>
        /// Allocates pointer and puts null terminated UTF-8 string bytes.
        /// </summary>
        /// <param name="managedString"></param>
        /// <returns></returns>
        public static IntPtr NativeUtf8FromString(string value) 
        {
            int length = Encoding.UTF8.GetByteCount(value);
            byte[] buffer = new byte[length + 1];
            Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }

        /// <summary>
        /// Reads all bytes as a null terminated UTF-8 string.
        /// </summary>
        /// <param name="nativeUtf8"></param>
        /// <returns></returns>
        public static string StringFromNativeUtf8(IntPtr nativeUtf8) 
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
            int length = 0;
            while (Marshal.ReadByte(nativeUtf8, length) != 0) length++;
            if (length == 0) return string.Empty;
            byte[] buffer = new byte[length];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Reads all bytes as a null terminated UTF-8 string.
        /// </summary>
        /// <param name="nativeUtf8"></param>
        /// <returns></returns>
        public static string StringFromNativeUtf8(IntPtr nativeUtf8, int count)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
            byte[] buffer = new byte[count];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
