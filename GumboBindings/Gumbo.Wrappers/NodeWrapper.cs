using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public abstract class NodeWrapper
    {
        public GumboNodeType Type { get; private set; }

        public int Index { get; private set; }

        public GumboParseFlags ParseFlags { get; private set; }

        public NodeWrapper Parent { get; private set; }

        public abstract IEnumerable<NodeWrapper> Children { get; }

        protected readonly GumboWrapper DisposableOwner;
        
        public NodeWrapper(GumboWrapper disposableOwner, GumboNode node, NodeWrapper parent)
	    {
            DisposableOwner = disposableOwner;
            Type = node.type;
            Index = (int)node.index_within_parent;
            ParseFlags = node.parse_flags;
            Parent = parent;
	    }

        public IEnumerable<ElementWrapper> Elements()
        {
            return Children.OfType<ElementWrapper>();
        }

        protected void ThrowIfOwnerDisposed()
        {
            if (DisposableOwner.Disposed)
            {
                throw new ObjectDisposedException("GumboWrapper has already been disposed");
            }
        }
    }
}
