using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Gumbo.Wrappers
{
    [DebuggerDisplay("Type = {Type}, Tag = {Tag}")]
    public class ElementWrapper : NodeWrapper
    {
        public override ImmutableArray<NodeWrapper> Children => _Children.Value;

        public ImmutableArray<AttributeWrapper> Attributes => _Attributes.Value;

        public string Value => _Value.Value;

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

        private readonly Lazy<ImmutableArray<NodeWrapper>> _Children;

        private readonly Lazy<ImmutableArray<AttributeWrapper>> _Attributes;

        private readonly Lazy<string> _Value;

        internal ElementWrapper(GumboElementNode node, NodeWrapper parent, WrapperFactory factory)
            : base(node, parent)
        {
            _Children = factory.CreateDisposalAwareLazy(() =>
                ImmutableArray.CreateRange(node.GetChildren().OrderBy(x => x.index_within_parent).Select(x => factory.CreateNodeWrapper(x, this))));

            _Attributes = factory.CreateDisposalAwareLazy(() =>
                ImmutableArray.CreateRange(node.GetAttributes().Select(x => factory.CreateAttributeWrapper(x, this))));

            _Value = factory.CreateDisposalAwareLazy(() => string.Concat(Children.Select(x => x is ElementWrapper
                    ? ((ElementWrapper)x).Value
                    : ((TextWrapper)x).Value)));

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
