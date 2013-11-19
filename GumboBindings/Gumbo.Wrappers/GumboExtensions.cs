using Gumbo.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gumbo.Wrappers
{
    public static class GumboExtensions
    {
        public static string MarshalToString(this GumboStringPiece stringPiece)
        {
            return NativeUtf8Helper.StringFromNativeUtf8(stringPiece.data, (int)stringPiece.length);
        }

        public static IEnumerable<GumboNode> GetChildren(this GumboElementNode node)
        {
            return MarshalToPtrArray(node.element.children).Select(MarshalToSpecificNode);
        }

        public static IEnumerable<GumboNode> GetChildren(this GumboDocumentNode node)
        {
            return MarshalToPtrArray(node.document.children).Select(MarshalToSpecificNode);
        }

        public static IEnumerable<GumboAttribute> GetAttributes(this GumboElementNode node)
        {
            return MarshalToPtrArray(node.element.attributes).Select(MarshalTo<GumboAttribute>);
        }

        public static GumboDocumentNode GetDocument(this GumboOutput output)
        {
            return MarshalTo<GumboDocumentNode>(output.document);
        }

        public static GumboElementNode GetRoot(this GumboOutput output)
        {
            return MarshalTo<GumboElementNode>(output.root);
        }

        public static IEnumerable<GumboErrorContainer> GetErrors(this GumboOutput output)
        {
            return MarshalToPtrArray(output.errors).Select(MarshalToSpecificErrorContainer);
        }

        private static GumboErrorContainer MarshalToSpecificErrorContainer(IntPtr errorPointer)
        {
            var error = MarshalTo<GumboErrorContainer>(errorPointer);
            switch (error.type)
            {
                case GumboErrorType.GUMBO_ERR_UTF8_INVALID:
                case GumboErrorType.GUMBO_ERR_UTF8_TRUNCATED:
                case GumboErrorType.GUMBO_ERR_NUMERIC_CHAR_REF_WITHOUT_SEMICOLON:
                case GumboErrorType.GUMBO_ERR_NUMERIC_CHAR_REF_INVALID:
                    return MarshalTo<GumboCodepointErrorContainer>(errorPointer);
                case GumboErrorType.GUMBO_ERR_NAMED_CHAR_REF_WITHOUT_SEMICOLON:
                case GumboErrorType.GUMBO_ERR_NAMED_CHAR_REF_INVALID:
                    return MarshalTo<GumboNamedCharErrorContainer>(errorPointer);
                case GumboErrorType.GUMBO_ERR_DUPLICATE_ATTR:
                    return MarshalTo<GumboDuplicateAttrErrorContainer>(errorPointer);
                case GumboErrorType.GUMBO_ERR_PARSER:
                case GumboErrorType.GUMBO_ERR_UNACKNOWLEDGED_SELF_CLOSING_TAG:
                    return MarshalTo<GumboParserErrorContainer>(errorPointer);
                default:
                    return MarshalTo<GumboTokenizerErrorContainer>(errorPointer);
            }

        }

        /// <summary>
        /// Dealing with C unions, we need a two-step marshalling to get the actual instance.
        /// </summary>
        /// <param name="nodePointer"></param>
        /// <returns></returns>
        private static GumboNode MarshalToSpecificNode(IntPtr nodePointer)
        {
            GumboNode node = (GumboNode)Marshal.PtrToStructure(nodePointer, typeof(GumboNode));
            switch (node.type)
            {
                case GumboNodeType.GUMBO_NODE_DOCUMENT:
                    return MarshalTo<GumboDocumentNode>(nodePointer);
                case GumboNodeType.GUMBO_NODE_ELEMENT:
                    return MarshalTo<GumboElementNode>(nodePointer);
                case GumboNodeType.GUMBO_NODE_TEXT:
                case GumboNodeType.GUMBO_NODE_CDATA:
                case GumboNodeType.GUMBO_NODE_COMMENT:
                case GumboNodeType.GUMBO_NODE_WHITESPACE:
                    return MarshalTo<GumboTextNode>(nodePointer);
                default:
                    throw new NotSupportedException(String.Format("Unknown node type '{0}'", (int)node.type));
            }
        }

        private static T MarshalTo<T>(IntPtr pointer)
        {
            return (T)Marshal.PtrToStructure(pointer, typeof(T));
        }

        private static IntPtr[] MarshalToPtrArray(GumboVector vector)
        {
            if (vector.data == IntPtr.Zero)
            {
                return new IntPtr[0];
            }

            IntPtr[] ptrs = new IntPtr[vector.length];
            Marshal.Copy(vector.data, ptrs, 0, ptrs.Length);
            return ptrs;
        }
    }
}
