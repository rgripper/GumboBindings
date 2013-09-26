using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Gumbo.Wrappers;
using Gumbo.Bindings;

namespace Gumbo.Wrappers
{
    public static class GumboToXmlExtensions
    {
        public static XDocument ToXDocument(this GumboDocumentNode docNode)
        {
            return (XDocument)CreateXNode(docNode);
        }

        private static XNode CreateXNode(GumboNode node)
        {
            switch (node.type)
            {
                case GumboNodeType.GUMBO_NODE_DOCUMENT:
                    var docNode = (GumboDocumentNode)node;
                    return new XDocument(docNode.document.GetChildren().Select(x => CreateXNode(x)));
                case GumboNodeType.GUMBO_NODE_ELEMENT:
                    var elementNode = (GumboElementNode)node;
                    return new XElement(GetName(elementNode.element.tag), 
                        elementNode.element.GetChildren().Select(x => CreateXNode(x)),
                        elementNode.element.GetAttributes().Select(x => new XAttribute(
                            NativeUtf8Helper.StringFromNativeUtf8(x.name), 
                            NativeUtf8Helper.StringFromNativeUtf8(x.value))));
                case GumboNodeType.GUMBO_NODE_TEXT:
                    var textNode = (GumboTextNode)node;
                    return new XText(NativeUtf8Helper.StringFromNativeUtf8(textNode.text.text));
                case GumboNodeType.GUMBO_NODE_CDATA:
                    var cDataNode = (GumboTextNode)node;
                    return new XCData(NativeUtf8Helper.StringFromNativeUtf8(cDataNode.text.text));
                case GumboNodeType.GUMBO_NODE_COMMENT:
                    var commentNode = (GumboTextNode)node;
                    return new XComment(NativeUtf8Helper.StringFromNativeUtf8(commentNode.text.text));
                case GumboNodeType.GUMBO_NODE_WHITESPACE:
                    var spaceNode = (GumboTextNode)node;
                    return new XText(NativeUtf8Helper.StringFromNativeUtf8(spaceNode.text.text));
                default:
                    throw new NotSupportedException(String.Format("Unknown node type '{0}'", (int)node.type));
            }
        }

        private static string GetName(GumboTag tag)
        {
            return tag.ToString().Substring("GUMBO_TAG_".Length).ToLower();
        }
    }
}
