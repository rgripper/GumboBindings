using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Gumbo.Wrappers
{
    public class GumboWrapper : IDisposable, IXPathNavigable
    {
        public DocumentWrapper Document { get; private set; }

        public IEnumerable<GumboErrorContainer> Errors { get; private set; }

        private bool _Disposed;

        private bool _Marshalled;

        private GumboOptions _Options;

        private readonly GumboDocumentNode _GumboDocumentNode;

        private readonly IntPtr _OutputPtr;

        private readonly IntPtr _Html;

        private readonly Dictionary<string, List<ElementWrapper>> ElementsWithIds = 
            new Dictionary<string, List<ElementWrapper>>(StringComparer.OrdinalIgnoreCase);

        public static List<Tuple<string, TimeSpan>> Values = new List<Tuple<string, TimeSpan>>();

        public static Tuple<string, System.Diagnostics.Stopwatch> StartLog(string text)
        {
            return Tuple.Create(text, System.Diagnostics.Stopwatch.StartNew());
        }

        public static void EndLog(Tuple<string, System.Diagnostics.Stopwatch> startState)
        {
            Console.WriteLine("{0} {1}", startState.Item2.Elapsed, startState.Item1);
        }

        public GumboWrapper(string html, bool stopOnFirstError = false, int maxErrors = -1, int tabStopSize = 8)
        {
            _Options = new GumboOptions();
            NativeMethods.gumbo_set_options_defaults(ref _Options);
            _Options.max_errors = maxErrors;
            _Options.stop_on_first_error = stopOnFirstError;
            _Html = NativeUtf8Helper.NativeUtf8FromString(html);

            _OutputPtr = NativeMethods.gumbo_parse(_Html);
            var output = (GumboOutput)Marshal.PtrToStructure(_OutputPtr, typeof(GumboOutput));
            _GumboDocumentNode = output.GetDocument();
            Errors = output.GetErrors();

            var lazyFactory = new DisposalAwareLazyFactory(() => this._Disposed, typeof(GumboWrapper).Name);
            Document = new DocumentWrapper(_GumboDocumentNode, lazyFactory, AddElementWithId);
        }

        public XDocument ToXDocument()
        {
            return GumboToXmlExtensions.ToXDocument(_GumboDocumentNode);
        }

        public XPathNavigator CreateNavigator()
        {
            return new GumboNavigator(this, this.Document);
        }

        public ElementWrapper GetElementById(string id)
        {
            MarshalAll();

            List<ElementWrapper> elements;

            if (ElementsWithIds.TryGetValue(id, out elements))
            {
                return elements.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Disposes all unmanaged data. Any subsequent calls to get nodes' children
        /// not previously marshalled will result in exception.
        /// </summary>
        public void Dispose()
        {
            if (_Disposed)
            {
                return;
            }

            Marshal.FreeHGlobal(_Html);
            NativeMethods.gumbo_destroy_output(ref _Options, _OutputPtr);
            _Disposed = true;
        }

        /// <summary>
        /// Marshals all nodes. Does nothing if has already been called.
        /// </summary>
        public void MarshalAll()
        {
            if (_Marshalled)
            {
                return;
            }

            MarshalElementAndDescendants(this.Document.Root);
            _Marshalled = true;
        }

        ~GumboWrapper()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        private void AddElementWithId(string id, ElementWrapper element)
        {
            List<ElementWrapper> elements;
            if (!ElementsWithIds.TryGetValue(id, out elements))
            {
                elements = new List<ElementWrapper>();
                ElementsWithIds.Add(id, elements);
            }

            elements.Add(element);
        }

        private static void MarshalElementAndDescendants(ElementWrapper element)
        {
            element.Attributes.Any();
            foreach (var child in element.Children.OfType<ElementWrapper>())
            {
                MarshalElementAndDescendants(child);
            }
        }
    }
}
