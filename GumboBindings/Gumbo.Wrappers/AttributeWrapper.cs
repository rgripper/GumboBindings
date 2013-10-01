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

        public string OriginalName { get; private set; }

        public GumboSourcePosition NameStart { get; private set; }

        public GumboSourcePosition NameEnd { get; private set; }

        public GumboSourcePosition ValueStart { get; private set; }

        public GumboSourcePosition ValueEnd { get; private set; }

        public GumboAttributeNamespaceEnum Namespace { get; private set; }

        public AttributeWrapper(GumboAttribute attribute)
        {
            Name = NativeUtf8Helper.StringFromNativeUtf8(attribute.name);
            Value = NativeUtf8Helper.StringFromNativeUtf8(attribute.value);
            OriginalName = NativeUtf8Helper.StringFromNativeUtf8(attribute.original_name.data, (int)attribute.original_name.length);
            NameStart = attribute.name_start;
            NameEnd = attribute.name_end;
            ValueStart = attribute.value_start;
            ValueEnd = attribute.value_end;
            Namespace = attribute.attr_namespace;
        }
    }
}
