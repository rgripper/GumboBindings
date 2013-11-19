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
        public ElementWrapper Root { get { return _Root.Value; } }

        public bool HasDocType { get; private set; }

        public string Name { get; private set; }

        public string PublicIdentifier { get; private set; }

        public string SystemIdentifier { get; private set; }

        public GumboQuirksModeEnum DocTypeQuirksMode { get; private set; }

        public override IEnumerable<NodeWrapper> Children
        {
            get 
            {
                yield return Root;
            }
        }

        private readonly Lazy<ElementWrapper> _Root;

        internal DocumentWrapper(GumboDocumentNode node, DisposalAwareLazyFactory lazyFactory, 
            Action<string, ElementWrapper> addElementWithId)
            : base(node, null)
        {
            _Root = lazyFactory.Create<ElementWrapper>(() =>
            {
                return new ElementWrapper((GumboElementNode)node.GetChildren().Single(), this,
                    lazyFactory, addElementWithId);
            });

            HasDocType = node.document.has_doctype;
            Name = NativeUtf8Helper.StringFromNativeUtf8(node.document.name);
            PublicIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.public_identifier);
            SystemIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.system_identifier);
            DocTypeQuirksMode = node.document.doc_type_quirks_mode;
        }
    }
}
