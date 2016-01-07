using System;
using System.Linq;
using System.Xml.Linq;
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
                    return new XDocument(docNode.GetChildren().Select(x => CreateXNode(x)));
                case GumboNodeType.GUMBO_NODE_ELEMENT:
                case GumboNodeType.GUMBO_NODE_TEMPLATE:
                    var elementNode = (GumboElementNode)node;
                    string elementName = GetName(elementNode.element.tag);
                    var attributes = elementNode.GetAttributes().Select(x => new XAttribute(
                        NativeUtf8Helper.StringFromNativeUtf8(x.name),
                        NativeUtf8Helper.StringFromNativeUtf8(x.value)));
                    var children = elementNode.GetChildren().Select(x => CreateXNode(x));
                    return new XElement(elementName, attributes, children);
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
                    throw new NotImplementedException($"Node type '{node.type}' is not implemented");
            }
        }

        private static string GetName(GumboTag tag)
        {
            return tag.ToString().Substring("GUMBO_TAG_".Length).ToLower().Replace('_', '-');
        }
    }
}
