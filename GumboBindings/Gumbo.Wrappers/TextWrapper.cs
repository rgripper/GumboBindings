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
        public string Text { get; private set; }

        public GumboSourcePosition StartPosition { get; private set; }

        public string OriginalText { get; private set; }

        public TextWrapper(GumboWrapper disposableOwner, GumboTextNode node, NodeWrapper parent)
            : base(disposableOwner, node, parent)
        {
            Text = NativeUtf8Helper.StringFromNativeUtf8(node.text.text);
            OriginalText = NativeUtf8Helper.StringFromNativeUtf8(node.text.original_text.data, (int)node.text.original_text.length);
            StartPosition = node.text.start_pos;
        }
    }
}
