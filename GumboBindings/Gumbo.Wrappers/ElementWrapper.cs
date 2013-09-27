using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers
{
    public class ElementWrapper : NodeWrapper
    {
        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public IEnumerable<NodeWrapper> Children { get; private set; }

        /// GumboTag->Anonymous_18731001_1c98_4ec7_9213_761374ef910c
        public GumboTag Tag { get; private set; }

        /// GumboNamespaceEnum->Anonymous_3f067a31_b9c5_4f93_a7c3_4199fa665800
        public GumboNamespaceEnum TagNamespace { get; private set; }

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition StartPosition { get; private set; }

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition EndPosition { get; private set; }

        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public IEnumerable<AttributeWrapper> Attributes { get; private set; }

        public ElementWrapper(GumboElementNode node, NodeWrapper parent)
            : base(node, parent)
        {
            Children = node.element.GetChildren().Select(x => x is GumboElementNode
                ? (NodeWrapper)new ElementWrapper((GumboElementNode)x, this)
                : (NodeWrapper)new TextWrapper((GumboTextNode)x, this)).ToList();
            Tag = node.element.tag;
            TagNamespace = node.element.tag_namespace;
            StartPosition = node.element.start_pos;
            EndPosition = node.element.end_pos;
            Attributes = node.element.GetAttributes().Select(x => new AttributeWrapper(x)).ToList();
        }

        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public IEnumerable<ElementWrapper> Elements() 
        { 
            return Children.OfType<ElementWrapper>(); 
        }
    }
}
