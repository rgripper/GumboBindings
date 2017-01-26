using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.PlatformAbstractions;

namespace Gumbo
{

    public static class UnmanagedLibraryHelper
    {
        public static IUnmanagedLibrary Create(string name)
        {
#if CORECLR
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsUnmanagedLibrary(name);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxUnmanagedLibrary(name);
            }
            else
            {
                // TODO
                throw new NotImplementedException("Unmanaged library loading is not implemented on this platform");
            }
#else
            return new WindowsUnmanagedLibrary(name);
#endif
        }
    }

    public interface IUnmanagedLibrary : IDisposable
    {
        T MarshalStructure<T>(string name);
    }

    internal sealed class WindowsUnmanagedLibrary : IUnmanagedLibrary
    {
        private readonly IntPtr _LibarayHandle;

        private bool _IsDisposed = false;

        public WindowsUnmanagedLibrary(string name)
        {
            _LibarayHandle = LoadLibrary(name);
        }

        public void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            FreeLibrary(_LibarayHandle);
            _IsDisposed = true;
        }

        public T MarshalStructure<T>(string name)
        {
            if (_IsDisposed)
            {
                throw new ObjectDisposedException("UnmanagedLibrary");
            }

            var ptr = GetProcAddress(_LibarayHandle, name);
            return Marshal.PtrToStructure<T>(ptr);
        }

        ~WindowsUnmanagedLibrary()
        {
            Dispose();
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll")]
        private static extern int FreeLibrary(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr handle, string procedureName);
    }

    internal sealed class LinuxUnmanagedLibrary : IUnmanagedLibrary
    {
        private readonly IntPtr _LibarayHandle;

        private const int RTLD_NOW = 2;

        private bool _IsDisposed = false;

        public LinuxUnmanagedLibrary(string name)
        {
            _LibarayHandle = dlopen(name, RTLD_NOW);
        }

        public void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            dlclose(_LibarayHandle);
            _IsDisposed = true;
        }

        public T MarshalStructure<T>(string name)
        {
            if (_IsDisposed)
            {
                throw new ObjectDisposedException("UnmanagedLibrary");
            }

            // clear previous errors if any
            dlerror();
            var ptr = dlsym(_LibarayHandle, name);
            var errPtr = dlerror();
            if (errPtr != IntPtr.Zero)
            {
                throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
            }

            return Marshal.PtrToStructure<T>(ptr);
        }

        ~LinuxUnmanagedLibrary()
        {
            Dispose();
        }

        [DllImport("libdl.so")]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl.so")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport("libdl.so")]
        private static extern int dlclose(IntPtr handle);

        [DllImport("libdl.so")]
        private static extern IntPtr dlerror();
    }

}
