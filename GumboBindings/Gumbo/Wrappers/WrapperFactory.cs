using System;
using System.Linq;
using System.Collections.Generic;

namespace Gumbo.Wrappers
{
    internal class WrapperFactory
    {
        private readonly DisposalAwareLazyFactory _LazyFactory;

        private readonly Dictionary<string, List<ElementWrapper>> MarshalledElementsByIds =
            new Dictionary<string, List<ElementWrapper>>(StringComparer.OrdinalIgnoreCase);

        private bool _IsMarshalled;

        public WrapperFactory(DisposalAwareLazyFactory lazyFactory)
        {
            _LazyFactory = lazyFactory;
        }

        public NodeWrapper CreateNodeWrapper(GumboNode node, NodeWrapper parent = null)
        {
            switch (node.type)
            {
                case GumboNodeType.GUMBO_NODE_DOCUMENT:
                    return new DocumentWrapper((GumboDocumentNode)node, this);
                case GumboNodeType.GUMBO_NODE_ELEMENT:
                case GumboNodeType.GUMBO_NODE_TEMPLATE:
                    return new ElementWrapper((GumboElementNode)node, parent, this);
                case GumboNodeType.GUMBO_NODE_TEXT:
                case GumboNodeType.GUMBO_NODE_CDATA:
                case GumboNodeType.GUMBO_NODE_COMMENT:
                case GumboNodeType.GUMBO_NODE_WHITESPACE:
                    return new TextWrapper((GumboTextNode)node, parent);
                default:
                    throw new NotImplementedException($"Node type '{node.type}' is not implemented");
            }
        }

        public AttributeWrapper CreateAttributeWrapper(GumboAttribute attribute, ElementWrapper parent)
        {
            var attributeWrapper = new AttributeWrapper(attribute, parent);
            if (string.Equals(attributeWrapper.Name, "id", StringComparison.OrdinalIgnoreCase))
            {
                AddElementById(attributeWrapper.Value, parent);
            }
            return attributeWrapper;
        }

        public Lazy<T> CreateDisposalAwareLazy<T>(Func<T> factoryMethod)
        {
            return _LazyFactory.Create(factoryMethod);
        }

        public ElementWrapper GetElementById(string id)
        {
            List<ElementWrapper> elements;

            if (MarshalledElementsByIds.TryGetValue(id, out elements))
            {
                return elements.FirstOrDefault();
            }

            return null;
        }

        private void AddElementById(string id, ElementWrapper element)
        {
            List<ElementWrapper> elements;
            if (!MarshalledElementsByIds.TryGetValue(id, out elements))
            {
                elements = new List<ElementWrapper>();
                MarshalledElementsByIds.Add(id, elements);
            }

            elements.Add(element);
        }
    }
}
