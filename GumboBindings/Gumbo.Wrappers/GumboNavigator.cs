using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Gumbo.Wrappers
{
    class GumboNavigator : XPathNavigator
    {
        private DocumentWrapper _Document;

        private ElementWrapper _Element;

        private TextWrapper _Text;

        private AttributeWrapper _Attribute;

        private readonly GumboWrapper _Gumbo;

        public override string BaseURI
        {
            get { throw new NotImplementedException(); }
        }

        public override XPathNavigator Clone()
        {
            if (_Document != null)
            {
                return new GumboNavigator(_Gumbo, _Document);
            }

            if (_Element != null)
            {
                return new GumboNavigator(_Gumbo, _Element);
            }

            if (_Text != null)
            {
                return new GumboNavigator(_Gumbo, _Text);
            }

            if (_Attribute != null)
            {
                return new GumboNavigator(_Gumbo, _Attribute);
            }

            throw new InvalidOperationException();
        }

        public GumboNavigator(GumboWrapper gumbo, NodeWrapper node)
        {
            _Gumbo = gumbo;
            SetCurrentNode(node);
        }

        public GumboNavigator(GumboWrapper gumbo, AttributeWrapper attribute)
        {
            _Gumbo = gumbo;
            SetCurrentAttribute(attribute);
        }

        public override bool IsEmptyElement
        {
            get 
            {
                if (_Element == null)
                {
                    return false;
                }

                return !_Element.Children.Any();
            }
        }

        public override bool IsSamePosition(XPathNavigator other)
        {
            var otherGumboNav = other as GumboNavigator;
            if (otherGumboNav == null)
            {
                return false;
            }

            return this._Document == otherGumboNav._Document
                && this._Element == otherGumboNav._Element
                && this._Text == otherGumboNav._Text
                && this._Attribute == otherGumboNav._Attribute;
        }

        public override string LocalName
        {
            get
            {
                if (_Element != null)
                {
                    if (!String.IsNullOrEmpty(_Element.OriginalTagName))
                    {
                        return _Element.OriginalTagName.Split(':').Last();
                    }
                    else
                    {
                        return _Element.NormalizedTagName;
                    }
                }

                if (_Attribute != null)
                {
                    return _Attribute.Name;
                }

                return String.Empty;
            }
        }

        public override bool MoveTo(XPathNavigator other)
        {
            var state = GumboWrapper.StartLog("MoveTo");

            var otherGumboNav = other as GumboNavigator;
            if (otherGumboNav == null)
            {
                return false;
            }

            this._Document = otherGumboNav._Document;
            this._Element = otherGumboNav._Element;
            this._Text = otherGumboNav._Text;
            this._Attribute = otherGumboNav._Attribute;

            GumboWrapper.EndLog(state);
            return true;
        }

        public override bool MoveToFirstAttribute()
        {
            var state = GumboWrapper.StartLog("MoveToFirstAttribute");

            if (_Element == null)
            {
                return false;
            }

            var firstAttr = _Element.Attributes.FirstOrDefault();
            if (firstAttr == null)
            {
                return false;
            }

            SetCurrentAttribute(firstAttr);

            GumboWrapper.EndLog(state);
            return true;
        }

        public override bool MoveToFirstChild()
        {
            var state = GumboWrapper.StartLog("MoveToFirstChild");

            if (_Document != null)
            {
                var child = _Document.Children.SingleOrDefault();
                if (child == null)
                {
                    return false;
                }

                SetCurrentNode(child);

                GumboWrapper.EndLog(state);
                return true;
            }

            if (_Element != null)
            {
                var child = _Element.Children.FirstOrDefault();
                if (child == null)
                {
                    return false;
                }

                SetCurrentNode(child);

                GumboWrapper.EndLog(state);
                return true;
            }

            GumboWrapper.EndLog(state);
            return false;
        }

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            throw new NotImplementedException();
        }

        public override bool MoveToId(string id)
        {
            _Gumbo.MarshalAll();

            var element = _Gumbo.GetElementById(id);
            if (element == null)
            {
                return false;
            }

            SetCurrentNode(element);
            return true;
        }

        public override bool MoveToNext()
        {
            var state = GumboWrapper.StartLog("MoveToNext");

            if (_Element != null)
            {
                if (_Element.Parent == null)
                {
                    GumboWrapper.EndLog(state);
                    return false;
                }

                var nextNode = _Element.Parent.Children.ElementAtOrDefault(_Element.Index + 1);
                if (nextNode == null)
                {
                    GumboWrapper.EndLog(state);
                    return false;
                }

                SetCurrentNode(nextNode);

                GumboWrapper.EndLog(state);
                return true;
            }

            if (_Text != null)
            {
                var nextNode = _Text.Parent.Children.ElementAtOrDefault(_Text.Index + 1);
                if (nextNode == null)
                {
                    GumboWrapper.EndLog(state);
                    return false;
                }

                SetCurrentNode(nextNode);
                GumboWrapper.EndLog(state);
                return true;
            }

            GumboWrapper.EndLog(state);
            return false;
        }

        public override bool MoveToNextAttribute()
        {
            var state = GumboWrapper.StartLog("MoveToNextAttribute");

            if (_Attribute == null)
            {
                return false;
            }

            var nextAttr = _Attribute.Parent.Attributes.ElementAtOrDefault(_Attribute.Index + 1);
            if (nextAttr == null)
            {
                return false;
            }

            SetCurrentAttribute(nextAttr);
            GumboWrapper.EndLog(state);
            return true;
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            throw new NotImplementedException();
        }

        public override bool MoveToParent()
        {
            var state = GumboWrapper.StartLog("MoveToParent");

            if (_Element != null)
            {
                if (_Element.Parent == null)
                {
                    GumboWrapper.EndLog(state);
                    return false;
                }

                SetCurrentNode(_Element.Parent);
                GumboWrapper.EndLog(state);
                return true;
            }

            if (_Text != null)
            {
                SetCurrentNode(_Text.Parent);
                GumboWrapper.EndLog(state);
                return true;
            }

            GumboWrapper.EndLog(state);
            return false;
        }

        public override bool MoveToPrevious()
        {
            var state = GumboWrapper.StartLog("MoveToPrevious");

            if (_Element != null)
            {
                if (_Element.Parent == null)
                {
                    return false;
                }

                var nextNode = _Element.Parent.Children.ElementAtOrDefault(_Element.Index - 1);
                if (nextNode == null)
                {
                    GumboWrapper.EndLog(state);
                    return false;
                }

                SetCurrentNode(nextNode);
                GumboWrapper.EndLog(state);
                return true;
            }

            if (_Text != null)
            {
                var nextNode = _Text.Parent.Children.ElementAtOrDefault(_Text.Index - 1);
                if (nextNode == null)
                {
                    GumboWrapper.EndLog(state);
                    return false;
                }

                SetCurrentNode(_Text.Parent);
                GumboWrapper.EndLog(state);
                return true;
            }
            GumboWrapper.EndLog(state);
            return false;
        }

        public override string Name
        {
            get 
            {
                if (_Element != null)
                {
                    return _Element.OriginalTagName;
                }

                if (_Attribute != null)
                {
                    return _Attribute.OriginalName;
                }

                return String.Empty;
            }
        }

        public override System.Xml.XmlNameTable NameTable
        {
            get { throw new NotImplementedException(); }
        }

        public override string NamespaceURI
        {
            get { return String.Empty; }
        }

        public override XPathNodeType NodeType
        {
            get 
            {
                if (_Document != null)
                {
                    return XPathNodeType.Root;
                }

                if (_Element != null)
                {
                    return XPathNodeType.Element;
                }

                if (_Text != null)
                {
                    switch (_Text.Type)
                    {
                        case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_TEXT:
                        case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_CDATA:
                            return XPathNodeType.Text;
                        case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_COMMENT:
                            return XPathNodeType.Comment;
                        case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_WHITESPACE:
                            return XPathNodeType.Whitespace;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (_Attribute != null)
                {
                    return XPathNodeType.Attribute;
                }

                // REVIEW: XPathNodeType.Namespace support?
                throw new Exception();
            }
        }

        public override string Prefix
        {
            get 
            {
                if (_Element != null)
                {
                    return String.Empty; // namespaces are implicit in html/svg/mathml
                }

                if (_Attribute != null)
                {
                    switch (_Attribute.Namespace)
                    {
                        case Gumbo.Bindings.GumboAttributeNamespaceEnum.GUMBO_ATTR_NAMESPACE_NONE:
                            return String.Empty;
                        case Gumbo.Bindings.GumboAttributeNamespaceEnum.GUMBO_ATTR_NAMESPACE_XLINK:
                            return "xlink";
                        case Gumbo.Bindings.GumboAttributeNamespaceEnum.GUMBO_ATTR_NAMESPACE_XML:
                            return "xml";
                        case Gumbo.Bindings.GumboAttributeNamespaceEnum.GUMBO_ATTR_NAMESPACE_XMLNS:
                            return "xmlns";
                        default:
                            throw new Exception();
                    } 
                }

                return String.Empty;
            }
        }

        public override string Value
        {
            get 
            {
                if (_Element != null)
                {
                    return _Element.Value;
                }

                if (_Attribute != null)
                {
                    return _Attribute.Value;
                }

                return String.Empty;
            }
        }

        private void ClearCurrent()
        {
            _Document = null;
            _Element = null;
            _Text = null;
            _Attribute = null;
        }

        private void SetCurrentAttribute(AttributeWrapper attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            ClearCurrent();
            _Attribute = attribute;
        }

        private void SetCurrentNode(NodeWrapper node)
        {
            

            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            ClearCurrent();
            var state = GumboWrapper.StartLog("SetCurrentNode");
            switch (node.Type)
            {
                case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_DOCUMENT:
                    _Document = (DocumentWrapper)node;
                    break;
                case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_ELEMENT:
                    _Element = (ElementWrapper)node;
                    break;
                case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_TEXT:
                case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_CDATA:
                case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_COMMENT:
                case Gumbo.Bindings.GumboNodeType.GUMBO_NODE_WHITESPACE:
                    _Text = (TextWrapper)node;
                    break;
                default:
                    throw new Exception();
            }
            GumboWrapper.EndLog(state);
            
        }
    }
}
