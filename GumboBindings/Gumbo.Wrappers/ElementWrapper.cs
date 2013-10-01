using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class ElementWrapper : NodeWrapper
    {
        private readonly Lazy<IEnumerable<NodeWrapper>> _Children;

        public IEnumerable<NodeWrapper> Children { get { return _Children.Value; } }

        private readonly Lazy<IEnumerable<AttributeWrapper>> _Attributes;

        public IEnumerable<AttributeWrapper> Attributes { get { return _Attributes.Value; } }

        public GumboTag Tag { get; private set; }

        public GumboNamespaceEnum TagNamespace { get; private set; }

        public GumboSourcePosition StartPosition { get; private set; }

        public GumboSourcePosition EndPosition { get; private set; }

        public string OriginalTag { get; private set; }

        public string OriginalEndTag { get; private set; }

        public ElementWrapper(GumboWrapper disposableOwner, GumboElementNode node, NodeWrapper parent)
            : base(disposableOwner, node, parent)
        {
            _Children = new Lazy<IEnumerable<NodeWrapper>>(() => CreateChildren(node));
            _Attributes = new Lazy<IEnumerable<AttributeWrapper>>(() => CreateAttributes(node));

            Tag = node.element.tag;
            TagNamespace = node.element.tag_namespace;
            StartPosition = node.element.start_pos;
            EndPosition = node.element.end_pos;

            OriginalTag = NativeUtf8Helper.StringFromNativeUtf8(node.element.original_tag.data, (int)node.element.original_tag.length);
            OriginalEndTag = NativeUtf8Helper.StringFromNativeUtf8(node.element.original_end_tag.data, (int)node.element.original_end_tag.length);
        }

        public IEnumerable<ElementWrapper> Elements()
        {
            return Children.OfType<ElementWrapper>();
        }

        private IEnumerable<NodeWrapper> CreateChildren(GumboElementNode node)
        {
            ThrowIfOwnerDisposed();
            return node.element.GetChildren().Select(x => x is GumboElementNode
                ? (NodeWrapper)new ElementWrapper(this.DisposableOwner, (GumboElementNode)x, this)
                : (NodeWrapper)new TextWrapper(this.DisposableOwner, (GumboTextNode)x, this)).ToList();
        }

        private IEnumerable<AttributeWrapper> CreateAttributes(GumboElementNode node)
        {
            ThrowIfOwnerDisposed();
            return node.element.GetAttributes().Select(x => new AttributeWrapper(x)).ToList();
        }
    }
}
