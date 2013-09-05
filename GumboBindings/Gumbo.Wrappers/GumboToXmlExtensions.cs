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
                        elementNode.element.GetAttributes().Select(x => new XAttribute(x.name, x.value)));
                case GumboNodeType.GUMBO_NODE_TEXT:
                    var textNode = (GumboTextNode)node;
                    return new XText(textNode.text.text);
                case GumboNodeType.GUMBO_NODE_CDATA:
                    var cDataNode = (GumboTextNode)node;
                    return new XCData(cDataNode.text.text);
                case GumboNodeType.GUMBO_NODE_COMMENT:
                    var commentNode = (GumboTextNode)node;
                    return new XComment(commentNode.text.text);
                case GumboNodeType.GUMBO_NODE_WHITESPACE:
                    var spaceNode = (GumboTextNode)node;
                    return new XText(spaceNode.text.text);
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
