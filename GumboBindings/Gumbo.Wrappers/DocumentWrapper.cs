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
        public ElementWrapper Root { get { return _Children.Value.FirstOrDefault(); } }

        public bool HasDocType { get; private set; }

        public string Name { get; private set; }

        public string PublicIdentifier { get; private set; }

        public string SystemIdentifier { get; private set; }

        public GumboQuirksModeEnum DocTypeQuirksMode { get; private set; }

        public override IEnumerable<NodeWrapper> Children
        {
            get 
            {
                return _Children.Value;
            }
        }

        private readonly Lazy<IEnumerable<ElementWrapper>> _Children;

        internal DocumentWrapper(GumboDocumentNode node, DisposalAwareLazyFactory lazyFactory, 
            Action<string, ElementWrapper> addElementWithId)
            : base(node, null)
        {
            _Children = lazyFactory.Create<IEnumerable<ElementWrapper>>(() =>
            {
                return node.GetChildren().Select(x => new ElementWrapper((GumboElementNode)x, this,
                    lazyFactory, addElementWithId)).ToList().AsReadOnly();
            });

            HasDocType = node.document.has_doctype;
            Name = NativeUtf8Helper.StringFromNativeUtf8(node.document.name);
            PublicIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.public_identifier);
            SystemIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.system_identifier);
            DocTypeQuirksMode = node.document.doc_type_quirks_mode;
        }
    }
}
