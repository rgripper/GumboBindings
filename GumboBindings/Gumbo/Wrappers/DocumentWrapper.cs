using System;
using System.Collections.Immutable;
using System.Linq;

namespace Gumbo.Wrappers
{
    public class DocumentWrapper : NodeWrapper
    {
        public ElementWrapper Root => (ElementWrapper)Children.FirstOrDefault();

        public bool HasDocType { get; private set; }

        public string Name { get; private set; }

        public string PublicIdentifier { get; private set; }

        public string SystemIdentifier { get; private set; }

        public GumboQuirksModeEnum DocTypeQuirksMode { get; private set; }

        public override ImmutableArray<NodeWrapper> Children => _Children.Value;

        private readonly Lazy<ImmutableArray<NodeWrapper>> _Children;

        internal DocumentWrapper(GumboDocumentNode node, WrapperFactory factory)
            : base(node, null)
        {
            _Children = factory.CreateDisposalAwareLazy(() => 
                ImmutableArray.CreateRange(node.GetChildren().OrderBy(x => x.index_within_parent).Select(x => factory.CreateNodeWrapper(x))));

            HasDocType = node.document.has_doctype;
            Name = NativeUtf8Helper.StringFromNativeUtf8(node.document.name);
            PublicIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.public_identifier);
            SystemIdentifier = NativeUtf8Helper.StringFromNativeUtf8(node.document.system_identifier);
            DocTypeQuirksMode = node.document.doc_type_quirks_mode;
        }
    }
}
