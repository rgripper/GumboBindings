using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class TextWrapper : NodeWrapper
    {
        public string Text;

        public GumboSourcePosition StartPosition;

        public TextWrapper(GumboTextNode node, NodeWrapper parent)
            : base (node, parent)
        {
            Text = NativeUtf8Helper.StringFromNativeUtf8(node.text.text);
            StartPosition = node.text.start_pos;
        }
    }
}
