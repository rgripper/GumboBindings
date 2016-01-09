using System.Collections.Immutable;
using System.Diagnostics;

namespace Gumbo.Wrappers
{
    [DebuggerDisplay("Type = {Type}, Value = {Value}")]
    public class TextWrapper : NodeWrapper
    {
        public string Value { get; private set; }

        public GumboSourcePosition StartPosition { get; private set; }

        public override ImmutableArray<NodeWrapper> Children => ImmutableArray.Create<NodeWrapper>();

        internal TextWrapper(GumboTextNode node, NodeWrapper parent)
            : base(node, parent)
        {
            Value = NativeUtf8Helper.StringFromNativeUtf8(node.text.text);
            StartPosition = node.text.start_pos;
        }

    }
}
