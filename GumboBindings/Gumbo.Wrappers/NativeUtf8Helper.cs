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
        /// Determines the length of the specified string (not including the terminating null character).
        /// </summary>
        /// <param name="nullTerminatedString"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int lstrlenA(IntPtr nullTerminatedString);

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
            int length = lstrlenA(nativeUtf8);
            return StringFromNativeUtf8(nativeUtf8, length);
        }

        /// <summary>
        /// Reads all bytes as a null terminated UTF-8 string.
        /// </summary>
        /// <param name="nativeUtf8"></param>
        /// <returns></returns>
        public static string StringFromNativeUtf8(IntPtr nativeUtf8, int length)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
            byte[] buffer = new byte[length];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
