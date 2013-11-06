using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class UnmanagedLibrary : IDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr LoadLibrary(String dllName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr dllPointer);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr dllPointer, string name);

        private static bool _Disposed = false;

        private readonly IntPtr _DllPointer;

        public UnmanagedLibrary(string moduleName)
        {
            _DllPointer = LoadLibrary(moduleName);
        }

        public T MarshalProcAddress<T>(string name)
        {
            IntPtr ptr = GetProcAddress(_DllPointer, name);
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }

        public void Dispose()
        {
            if (_Disposed)
            {
                return;
            }

            FreeLibrary(_DllPointer);
            _Disposed = true;
        }

        ~UnmanagedLibrary()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
