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
        public ElementWrapper Root { get; private set; }

        public bool HasDocType { get; private set; }

        public string Name { get; private set; }

        public string PublicIdentifier { get; private set; }

        public string SystemIdentifier { get; private set; }

        public GumboQuirksModeEnum DocTypeQuirksMode { get; private set; }

        public DocumentWrapper(GumboDocumentNode node, NodeWrapper parent)
            : base(node, parent)
        {
            var root = node.document.GetChildren().First();
            Root = new ElementWrapper((GumboElementNode)root, this);
            HasDocType = node.document.has_doctype;
            Name = node.document.name;
            PublicIdentifier = node.document.public_identifier;
            SystemIdentifier = node.document.system_identifier;
            DocTypeQuirksMode = node.document.doc_type_quirks_mode;
        }
    }
}
