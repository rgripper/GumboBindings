using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class AttributeWrapper
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        public GumboAttributeNamespaceEnum Namespace { get; private set; }

        public AttributeWrapper(GumboAttribute attribute)
        {
            Name = attribute.name;
            Value = attribute.value;
            Namespace = attribute.attr_namespace;
        }
    }
}
