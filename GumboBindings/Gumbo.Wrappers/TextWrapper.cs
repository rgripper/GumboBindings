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

    [DebuggerDisplay("Type = {Type}, Text = {Text}")]
    public class TextWrapper : NodeWrapper
    {
        public string Text { get; private set; }

        public GumboSourcePosition StartPosition { get; private set; }

        ///ilia: думаю, что необходимости в этом поле нет, к тому же в исходном коде есть такой текст:
        ///Note that
        /// because of foster parenting and other strange DOM manipulations, this may
        /// include other non-text HTML tags in it
        /// что кагбе намекает, что информация в полях, основанных на переменной _start_original_text может быть некорректной
        //public string OriginalText { get; private set; }

        public TextWrapper(GumboWrapper disposableOwner, GumboTextNode node, NodeWrapper parent)
            : base(disposableOwner, node, parent)
        {
            Text = NativeUtf8Helper.StringFromNativeUtf8(node.text.text);
            //OriginalText = NativeUtf8Helper.StringFromNativeUtf8(node.text.original_text.data, (int)node.text.original_text.length);
            StartPosition = node.text.start_pos;
        }
    }
}
