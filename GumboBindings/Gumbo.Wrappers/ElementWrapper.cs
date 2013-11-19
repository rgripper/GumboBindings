using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    [DebuggerDisplay("Type = {Type}, Tag = {Tag}")]
    public class ElementWrapper : NodeWrapper
    {
        public override IEnumerable<NodeWrapper> Children { get { return _Children.Value; } }

        public IEnumerable<AttributeWrapper> Attributes { get { return _Attributes.Value; } }

        public string Value { get { return _Value.Value; } }

        public GumboTag Tag { get; private set; }

        public GumboNamespaceEnum TagNamespace { get; private set; }

        public GumboSourcePosition StartPosition { get; private set; }

        public GumboSourcePosition EndPosition { get; private set; }

        public string OriginalTag { get; private set; }

        /// <summary>
        /// Returns lower-case name of every known HTML tag (according to the value of property <see cref="Tag"/>).
        /// For unknown tags returns empty string (where <see cref="Tag"/> is GUMBO_TAG_UNKNOWN).
        /// </summary>
        public string NormalizedTagName { get; private set; }

        public string OriginalTagName { get; private set; }

        public string OriginalEndTag { get; private set; }

        private readonly Lazy<IEnumerable<NodeWrapper>> _Children;

        private readonly Lazy<IEnumerable<AttributeWrapper>> _Attributes;

        private readonly Lazy<string> _Value;

        internal ElementWrapper(GumboElementNode node, NodeWrapper parent, DisposalAwareLazyFactory lazyFactory, 
            Action<string, ElementWrapper> addElementWithId)
            : base(node, parent)
        {

            _Children = lazyFactory.Create<IEnumerable<NodeWrapper>>(() => 
            {
                return node.GetChildren().Select(x => x is GumboElementNode
                ? (NodeWrapper)new ElementWrapper((GumboElementNode)x, this, lazyFactory, addElementWithId)
                : (NodeWrapper)new TextWrapper((GumboTextNode)x, this)).ToList();
            });

            _Attributes = lazyFactory.Create<IEnumerable<AttributeWrapper>>(() => 
            {
                return node.GetAttributes().Select((x, i) => 
                    new AttributeWrapper(x, this, i, addElementWithId)).ToList();
            });

            _Value = lazyFactory.Create<string>(() =>
            {
                return String.Concat(this.Children.Select(x => x is ElementWrapper
                    ? ((ElementWrapper)x).Value
                    : ((TextWrapper)x).Text));
            });

            StartPosition = node.element.start_pos;
            EndPosition = node.element.end_pos;

            Tag = node.element.tag;
            TagNamespace = node.element.tag_namespace;
            OriginalTag = NativeUtf8Helper.StringFromNativeUtf8(
                node.element.original_tag.data, (int)node.element.original_tag.length);
            OriginalTagName = GetTagNameFromOriginalTag(node.element);
            OriginalEndTag = NativeUtf8Helper.StringFromNativeUtf8(
                node.element.original_end_tag.data, (int)node.element.original_end_tag.length);
            NormalizedTagName = NativeUtf8Helper.StringFromNativeUtf8(
                NativeMethods.gumbo_normalized_tagname(node.element.tag));
        }

        private static string GetTagNameFromOriginalTag(GumboElement element)
        {
            var temp = element.original_tag;
            NativeMethods.gumbo_tag_from_original_text(ref temp);
            return temp.MarshalToString();
        }
    }
}
