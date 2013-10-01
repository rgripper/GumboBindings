using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class DocumentWrapper : NodeWrapper
    {
        private readonly Lazy<ElementWrapper> _Root;

        public ElementWrapper Root { get { return _Root.Value; } }

        public bool HasDocType { get; private set; }

        public string Name { get; private set; }

        public string PublicIdentifier { get; private set; }

        public string SystemIdentifier { get; private set; }

        public GumboQuirksModeEnum DocTypeQuirksMode { get; private set; }

        public DocumentWrapper(GumboWrapper disposableOwner, GumboDocumentNode node, NodeWrapper parent)
            : base(disposableOwner, node, parent)
        {
            _Root = new Lazy<ElementWrapper>(() => CreateRoot(node));
            HasDocType = node.document.has_doctype;
            Name = NativeUtf8Helper.StringFromNativeUtf8(node.document.name);
            PublicIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.public_identifier);
            SystemIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.system_identifier);
            DocTypeQuirksMode = node.document.doc_type_quirks_mode;
        }

        private ElementWrapper CreateRoot(GumboDocumentNode node)
        {
            ThrowIfOwnerDisposed();
            return new ElementWrapper(this.DisposableOwner, (GumboElementNode)node.document.GetChildren().First(), this);
        }
    }
}
