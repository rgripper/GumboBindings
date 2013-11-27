using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Gumbo.Wrappers
{
    public class UnmanagedLibrary : IDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr LoadLibrary(String dllName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr dllPointer);

        private static bool _Disposed = false;

        private readonly IntPtr _DllPointer;

        private readonly string _TempDllPath;

        public UnmanagedLibrary(Assembly assembly, string dllName)
        {
            _TempDllPath = ExtractEmbeddedLibrary(assembly, dllName);
            _DllPointer = LoadLibrary(_TempDllPath);
        }

        private static string ExtractEmbeddedLibrary(Assembly assembly, string dllName)
        {
            string tempPath = Path.GetTempFileName();
            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dllName))
            using (Stream fileStream = File.Open(tempPath, FileMode.Open))
            {
                resourceStream.CopyTo(fileStream);
            }
            return tempPath;
        }

        public void Dispose()
        {
            if (_Disposed)
            {
                return;
            }

            FreeLibrary(_DllPointer);
            File.Delete(_TempDllPath);
            _Disposed = true;
        }

        ~UnmanagedLibrary()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}