using System.Collections.Immutable;

namespace Gumbo.Wrappers
{
    public abstract class NodeWrapper
    {
        public GumboNodeType Type { get; private set; }

        public GumboParseFlags ParseFlags { get; private set; }

        public NodeWrapper Parent { get; private set; }

        public abstract ImmutableArray<NodeWrapper> Children { get; }

        public NodeWrapper(GumboNode node, NodeWrapper parent)
        {
            Type = node.type;
            ParseFlags = node.parse_flags;
            Parent = parent;
        }
    }
}
