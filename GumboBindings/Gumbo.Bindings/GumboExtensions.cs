using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gumbo.Bindings
{
    public static class GumboExtensions
    {
        public static IEnumerable<GumboNode> GetChildren(this GumboElement element)
        {
            return MarshalToPtrArray(element.children).Select(MarshalToSpecificNode);
        }

        public static IEnumerable<GumboNode> GetChildren(this GumboDocument element)
        {
            return MarshalToPtrArray(element.children).Select(MarshalToSpecificNode);
        }

        public static IEnumerable<GumboAttribute> GetAttributes(this GumboElement element)
        {
            return MarshalToPtrArray(element.attributes).Select(MarshalTo<GumboAttribute>);
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

        public static GumboErrorContainer MarshalToSpecificErrorContainer(IntPtr errorPointer)
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
        public static GumboNode MarshalToSpecificNode(IntPtr nodePointer)
        {
            GumboNode node = (GumboNode)Marshal.PtrToStructure(nodePointer, typeof(GumboNode));
            switch (node.type)
            {
                case GumboNodeType.GUMBO_NODE_DOCUMENT:
                    return (GumboDocumentNode)Marshal.PtrToStructure(nodePointer, typeof(GumboDocumentNode));
                case GumboNodeType.GUMBO_NODE_ELEMENT:
                    return (GumboElementNode)Marshal.PtrToStructure(nodePointer, typeof(GumboElementNode));
                case GumboNodeType.GUMBO_NODE_TEXT:
                case GumboNodeType.GUMBO_NODE_CDATA:
                case GumboNodeType.GUMBO_NODE_COMMENT:
                case GumboNodeType.GUMBO_NODE_WHITESPACE:
                    return (GumboTextNode)Marshal.PtrToStructure(nodePointer, typeof(GumboTextNode));
                default:
                    throw new NotSupportedException(String.Format("Unknown node type '{0}'", (int)node.type));
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr LoadLibrary(String dllName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr module, string name);

        //public static IntPtr MarshalProcAddress(string name)
        //{
        //    IntPtr gumboModulePtr = GetModuleHandle("gumbo.dll");
        //    return GetProcAddress(gumboModulePtr, name);
        //}

        public static T MarshalProcAddress<T>(string name)
        {
            IntPtr gumboModulePtr = LoadLibrary("gumbo.dll");
            IntPtr ptr = GetProcAddress(gumboModulePtr, name);
            return MarshalTo<T>(ptr);
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
