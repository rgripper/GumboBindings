using Gumbo.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Gumbo.Wrappers
{
    public struct GumboWrapperOptions
    {
        public bool StopOnFirstError { get; set; }

        public int MaxErrors { get; set; }

        public int TabStopSize { get; set; }

        public GumboTag FragmentContext { get; set; }

        public GumboNamespaceEnum FragmentNamespace { get; set; }
    }

    public sealed class GumboWrapper : IDisposable, IXPathNavigable
    {
        public DocumentWrapper Document { get; private set; }

        public IEnumerable<GumboErrorContainer> Errors { get; private set; }

        private bool _IsDisposed;

        private bool _IsMarshalled;

        private GumboOptions _Options;

        private readonly GumboDocumentNode _GumboDocumentNode;

        private readonly IntPtr _OutputPtr;

        private readonly IntPtr _Html;

        private readonly Dictionary<string, List<ElementWrapper>> ElementsWithIds = 
            new Dictionary<string, List<ElementWrapper>>(StringComparer.OrdinalIgnoreCase);

        private readonly WrapperFactory _WrapperFactory;

        private readonly IUnmanagedLibrary _GumboLibrary = UnmanagedLibraryHelper.Create(NativeMethods.LibraryName);

        public GumboWrapper(string html, GumboWrapperOptions? options = null)
        {
            _Options = CreateOptions(options);

            _Html = NativeUtf8Helper.NativeUtf8FromString(html);

            _OutputPtr = NativeMethods.gumbo_parse(_Html);
            var output = Marshal.PtrToStructure<GumboOutput>(_OutputPtr);
            _GumboDocumentNode = output.GetDocument();
            Errors = output.GetErrors();

            var lazyFactory = new DisposalAwareLazyFactory(() => _IsDisposed, typeof(GumboWrapper).Name);
            _WrapperFactory = new WrapperFactory(lazyFactory);
            Document = (DocumentWrapper)_WrapperFactory.CreateNodeWrapper(_GumboDocumentNode);
        }

        private GumboOptions CreateOptions(GumboWrapperOptions? options)
        {
            var defaultOptionsCopy = _GumboLibrary.MarshalStructure<GumboOptions>("kGumboDefaultOptions");

            if (options != null)
            {
                defaultOptionsCopy.max_errors = options.Value.MaxErrors;
                defaultOptionsCopy.stop_on_first_error = options.Value.StopOnFirstError;
                defaultOptionsCopy.tab_stop = options.Value.TabStopSize;
                defaultOptionsCopy.fragment_context = options.Value.FragmentContext;
                defaultOptionsCopy.fragment_namespace = options.Value.FragmentNamespace;
            }

            return defaultOptionsCopy;
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
            if (_IsDisposed)
            {
                return;
            }

            Marshal.FreeHGlobal(_Html);
            NativeMethods.gumbo_destroy_output(ref _Options, _OutputPtr);
            _GumboLibrary.Dispose();
            _IsDisposed = true;
        }

        /// <summary>
        /// Marshals all nodes. Does nothing if has already been called.
        /// </summary>
        public void MarshalAll()
        {
            if (_IsMarshalled)
            {
                return;
            }

            MarshalElementAndDescendants(this.Document.Root);
            _IsMarshalled = true;
        }

        ~GumboWrapper()
        {
            Dispose();
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
            GC.KeepAlive(element.Attributes);
            foreach (var child in element.Children.OfType<ElementWrapper>())
            {
                MarshalElementAndDescendants(child);
            }
        }
    }
}
