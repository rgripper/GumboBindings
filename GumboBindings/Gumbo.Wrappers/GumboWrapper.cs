using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gumbo.Wrappers
{
    public class GumboWrapper : IDisposable
    {
        private GumboOptions _Options;

        private readonly GumboDocumentNode _GumboDocumentNode;

        private readonly IntPtr _OutputPtr;

        private readonly string _Html;

        public DocumentWrapper Document { get; private set; }

        public IEnumerable<GumboErrorContainer> Errors { get; private set; }

        public GumboWrapper(string html, bool stopOnFirstError = false, int maxErrors = -1)
        {
            _Html = html;

            _Options = GumboExtensions.MarshalProcAddress<GumboOptions>("kGumboDefaultOptions");
            _Options.max_errors = maxErrors;
            _Options.stop_on_first_error = stopOnFirstError;

            _OutputPtr = NativeMethods.gumbo_parse(_Html);
            var output = (GumboOutput)Marshal.PtrToStructure(_OutputPtr, typeof(GumboOutput));
            _GumboDocumentNode = output.GetDocument();
            Errors = output.GetErrors();
            Document = new DocumentWrapper(_GumboDocumentNode, null);
        }

        public XDocument ToXDocument()
        {
            return GumboToXmlExtensions.ToXDocument(_GumboDocumentNode);
        }

        public void Dispose()
        {
            NativeMethods.gumbo_destroy_output(ref _Options, _OutputPtr);
        }
    }
}
