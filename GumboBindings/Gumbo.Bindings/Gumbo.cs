using System;
using System.Runtime.InteropServices;

namespace Gumbo.Bindings
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboNode
    {
        /// GumboNodeType->Anonymous_0baae05d_0323_414f_8f41_c58c3a77cea8
        public GumboNodeType type;

        /// GumboNode*
        public IntPtr parent;

        /// size_t->unsigned int
        public uint index_within_parent;

        /// GumboParseFlags->Anonymous_a2031eb3_cd7b_4b09_98eb_e950ead0223f
        public GumboParseFlags parse_flags;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboElementNode : GumboNode
    {
        /// GumboElement->Anonymous_a668b228_b771_4bb6_b6ae_aabf0700f048
        public GumboElement element;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboDocumentNode : GumboNode
    {
        /// GumboElement->Anonymous_a668b228_b771_4bb6_b6ae_aabf0700f048
        public GumboDocument document;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboTextNode : GumboNode
    {
        /// GumboText->Anonymous_965a1696_be8e_41fe_a8d9_4245e3778c62
        public GumboText text;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboOptions
    {
        /// GumboAllocatorFunction
        IntPtr allocator;

        /// GumboDeallocatorFunction
        IntPtr deallocator;

        /// void*
        public IntPtr userdata;

        /// int
        public int tab_stop;

        /// boolean
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool stop_on_first_error;

        /// int
        public int max_errors;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboOutput
    {
        /// GumboNode*
        public IntPtr document;

        /// GumboNode*
        public IntPtr root;

        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public GumboVector errors;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboDuplicateAttrError 
    {
        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string name;
    
        /// unsigned int
        public uint original_index;
    
        /// unsigned int
        public uint new_index;
    }

    public enum GumboNodeType
    {
        GUMBO_NODE_DOCUMENT,

        GUMBO_NODE_ELEMENT,

        GUMBO_NODE_TEXT,

        GUMBO_NODE_CDATA,

        GUMBO_NODE_COMMENT,

        GUMBO_NODE_WHITESPACE,
    }

    [Flags]
    public enum GumboParseFlags
    {
        /// GUMBO_INSERTION_NORMAL -> 0
        GUMBO_INSERTION_NORMAL = 0,

        /// GUMBO_INSERTION_BY_PARSER -> 1<<0
        GUMBO_INSERTION_BY_PARSER = (1) << (0),

        /// GUMBO_INSERTION_IMPLICIT_END_TAG -> 1<<1
        GUMBO_INSERTION_IMPLICIT_END_TAG = (1) << (1),

        /// GUMBO_INSERTION_IMPLIED -> 1<<3
        GUMBO_INSERTION_IMPLIED = (1) << (3),

        /// GUMBO_INSERTION_CONVERTED_FROM_END_TAG -> 1<<4
        GUMBO_INSERTION_CONVERTED_FROM_END_TAG = (1) << (4),

        /// GUMBO_INSERTION_FROM_ISINDEX -> 1<<5
        GUMBO_INSERTION_FROM_ISINDEX = (1) << (5),

        /// GUMBO_INSERTION_FROM_IMAGE -> 1<<6
        GUMBO_INSERTION_FROM_IMAGE = (1) << (6),

        /// GUMBO_INSERTION_RECONSTRUCTED_FORMATTING_ELEMENT -> 1<<7
        GUMBO_INSERTION_RECONSTRUCTED_FORMATTING_ELEMENT = (1) << (7),

        /// GUMBO_INSERTION_ADOPTION_AGENCY_CLONED -> 1<<8
        GUMBO_INSERTION_ADOPTION_AGENCY_CLONED = (1) << (8),

        /// GUMBO_INSERTION_ADOPTION_AGENCY_MOVED -> 1<<9
        GUMBO_INSERTION_ADOPTION_AGENCY_MOVED = (1) << (9),

        /// GUMBO_INSERTION_FOSTER_PARENTED -> 1<<10
        GUMBO_INSERTION_FOSTER_PARENTED = (1) << (10),
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboVector
    {
        /// void**
        public IntPtr data;

        /// unsigned int
        public uint length;

        /// unsigned int
        public uint capacity;
    }

    public enum GumboTag
    {
        GUMBO_TAG_HTML,

        GUMBO_TAG_HEAD,

        GUMBO_TAG_TITLE,

        GUMBO_TAG_BASE,

        GUMBO_TAG_LINK,

        GUMBO_TAG_META,

        GUMBO_TAG_STYLE,

        GUMBO_TAG_SCRIPT,

        GUMBO_TAG_NOSCRIPT,

        GUMBO_TAG_BODY,

        GUMBO_TAG_SECTION,

        GUMBO_TAG_NAV,

        GUMBO_TAG_ARTICLE,

        GUMBO_TAG_ASIDE,

        GUMBO_TAG_H1,

        GUMBO_TAG_H2,

        GUMBO_TAG_H3,

        GUMBO_TAG_H4,

        GUMBO_TAG_H5,

        GUMBO_TAG_H6,

        GUMBO_TAG_HGROUP,

        GUMBO_TAG_HEADER,

        GUMBO_TAG_FOOTER,

        GUMBO_TAG_ADDRESS,

        GUMBO_TAG_P,

        GUMBO_TAG_HR,

        GUMBO_TAG_PRE,

        GUMBO_TAG_BLOCKQUOTE,

        GUMBO_TAG_OL,

        GUMBO_TAG_UL,

        GUMBO_TAG_LI,

        GUMBO_TAG_DL,

        GUMBO_TAG_DT,

        GUMBO_TAG_DD,

        GUMBO_TAG_FIGURE,

        GUMBO_TAG_FIGCAPTION,

        GUMBO_TAG_DIV,

        GUMBO_TAG_A,

        GUMBO_TAG_EM,

        GUMBO_TAG_STRONG,

        GUMBO_TAG_SMALL,

        GUMBO_TAG_S,

        GUMBO_TAG_CITE,

        GUMBO_TAG_Q,

        GUMBO_TAG_DFN,

        GUMBO_TAG_ABBR,

        GUMBO_TAG_TIME,

        GUMBO_TAG_CODE,

        GUMBO_TAG_VAR,

        GUMBO_TAG_SAMP,

        GUMBO_TAG_KBD,

        GUMBO_TAG_SUB,

        GUMBO_TAG_SUP,

        GUMBO_TAG_I,

        GUMBO_TAG_B,

        GUMBO_TAG_MARK,

        GUMBO_TAG_RUBY,

        GUMBO_TAG_RT,

        GUMBO_TAG_RP,

        GUMBO_TAG_BDI,

        GUMBO_TAG_BDO,

        GUMBO_TAG_SPAN,

        GUMBO_TAG_BR,

        GUMBO_TAG_WBR,

        GUMBO_TAG_INS,

        GUMBO_TAG_DEL,

        GUMBO_TAG_IMAGE,

        GUMBO_TAG_IMG,

        GUMBO_TAG_IFRAME,

        GUMBO_TAG_EMBED,

        GUMBO_TAG_OBJECT,

        GUMBO_TAG_PARAM,

        GUMBO_TAG_VIDEO,

        GUMBO_TAG_AUDIO,

        GUMBO_TAG_SOURCE,

        GUMBO_TAG_TRACK,

        GUMBO_TAG_CANVAS,

        GUMBO_TAG_MAP,

        GUMBO_TAG_AREA,

        GUMBO_TAG_MATH,

        GUMBO_TAG_MI,

        GUMBO_TAG_MO,

        GUMBO_TAG_MN,

        GUMBO_TAG_MS,

        GUMBO_TAG_MTEXT,

        GUMBO_TAG_MGLYPH,

        GUMBO_TAG_MALIGNMARK,

        GUMBO_TAG_ANNOTATION_XML,

        GUMBO_TAG_SVG,

        GUMBO_TAG_FOREIGNOBJECT,

        GUMBO_TAG_DESC,

        GUMBO_TAG_TABLE,

        GUMBO_TAG_CAPTION,

        GUMBO_TAG_COLGROUP,

        GUMBO_TAG_COL,

        GUMBO_TAG_TBODY,

        GUMBO_TAG_THEAD,

        GUMBO_TAG_TFOOT,

        GUMBO_TAG_TR,

        GUMBO_TAG_TD,

        GUMBO_TAG_TH,

        GUMBO_TAG_FORM,

        GUMBO_TAG_FIELDSET,

        GUMBO_TAG_LEGEND,

        GUMBO_TAG_LABEL,

        GUMBO_TAG_INPUT,

        GUMBO_TAG_BUTTON,

        GUMBO_TAG_SELECT,

        GUMBO_TAG_DATALIST,

        GUMBO_TAG_OPTGROUP,

        GUMBO_TAG_OPTION,

        GUMBO_TAG_TEXTAREA,

        GUMBO_TAG_KEYGEN,

        GUMBO_TAG_OUTPUT,

        GUMBO_TAG_PROGRESS,

        GUMBO_TAG_METER,

        GUMBO_TAG_DETAILS,

        GUMBO_TAG_SUMMARY,

        GUMBO_TAG_COMMAND,

        GUMBO_TAG_MENU,

        GUMBO_TAG_APPLET,

        GUMBO_TAG_ACRONYM,

        GUMBO_TAG_BGSOUND,

        GUMBO_TAG_DIR,

        GUMBO_TAG_FRAME,

        GUMBO_TAG_FRAMESET,

        GUMBO_TAG_NOFRAMES,

        GUMBO_TAG_ISINDEX,

        GUMBO_TAG_LISTING,

        GUMBO_TAG_XMP,

        GUMBO_TAG_NEXTID,

        GUMBO_TAG_NOEMBED,

        GUMBO_TAG_PLAINTEXT,

        GUMBO_TAG_RB,

        GUMBO_TAG_STRIKE,

        GUMBO_TAG_BASEFONT,

        GUMBO_TAG_BIG,

        GUMBO_TAG_BLINK,

        GUMBO_TAG_CENTER,

        GUMBO_TAG_FONT,

        GUMBO_TAG_MARQUEE,

        GUMBO_TAG_MULTICOL,

        GUMBO_TAG_NOBR,

        GUMBO_TAG_SPACER,

        GUMBO_TAG_TT,

        GUMBO_TAG_U,

        GUMBO_TAG_UNKNOWN,

        GUMBO_TAG_LAST,
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboAttribute
    {
        /// GumboAttributeNamespaceEnum->Anonymous_d0e8f855_186e_4a3b_aaf5_62320d08595d
        public GumboAttributeNamespaceEnum attr_namespace;

        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string name;

        /**
         * The original text of the attribute name, as a pointer into the original
         * source buffer.
         */
        GumboStringPiece original_name;

        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string value;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition name_start;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition name_end;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition value_start;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition value_end;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboDocument
    {
        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public GumboVector children;

        /// boolean
        [MarshalAsAttribute(UnmanagedType.I1)]
        public bool has_doctype;

        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string name;

        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string public_identifier;

        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string system_identifier;

        /// GumboQuirksModeEnum->Anonymous_9634e771_9059_484b_92b0_2f47f39beb2a
        public GumboQuirksModeEnum doc_type_quirks_mode;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboElement
    {
        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public GumboVector children;

        /// GumboTag->Anonymous_18731001_1c98_4ec7_9213_761374ef910c
        public GumboTag tag;

        /// GumboNamespaceEnum->Anonymous_3f067a31_b9c5_4f93_a7c3_4199fa665800
        public GumboNamespaceEnum tag_namespace;

        /**
         * A GumboStringPiece pointing to the original tag text for this element,
         * pointing directly into the source buffer.  If the tag was inserted
         * algorithmically (for example, <head> or <tbody> insertion), this will be a
         * zero-length string.
         */
        GumboStringPiece original_tag;

        /**
         * A GumboStringPiece pointing to the original end tag text for this element.
         * If the end tag was inserted algorithmically, (for example, closing a
         * self-closing tag), this will be a zero-length string.
         */
        GumboStringPiece original_end_tag;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition start_pos;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition end_pos;

        /// GumboVector->Anonymous_9472dab0_f1a2_477f_aed9_f00825401fa7
        public GumboVector attributes;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboText
    {
        /// char*
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string text;

        /**
         * The original text of this node, as a pointer into the original buffer.  For
         * comment/cdata nodes, this includes the comment delimiters.
         */
        GumboStringPiece original_text;

        /// GumboSourcePosition->Anonymous_b1ac5e09_64df_4170_b0b1_3753090b5fc0
        public GumboSourcePosition start_pos;
    }

    public enum GumboAttributeNamespaceEnum
    {
        GUMBO_ATTR_NAMESPACE_NONE,

        GUMBO_ATTR_NAMESPACE_XLINK,

        GUMBO_ATTR_NAMESPACE_XML,

        GUMBO_ATTR_NAMESPACE_XMLNS,
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboSourcePosition
    {
        /// unsigned int
        public uint line;

        /// unsigned int
        public uint column;

        /// unsigned int
        public uint offset;
    }

    public enum GumboQuirksModeEnum
    {
        GUMBO_DOCTYPE_NO_QUIRKS,

        GUMBO_DOCTYPE_QUIRKS,

        GUMBO_DOCTYPE_LIMITED_QUIRKS,
    }

    public enum GumboNamespaceEnum
    {
        GUMBO_NAMESPACE_HTML,

        GUMBO_NAMESPACE_SVG,

        GUMBO_NAMESPACE_MATHML,
    }

    public partial class NativeMethods
    {
        /// Return Type: GumboOutput*
        ///buffer: char*
        [DllImportAttribute("gumbo.dll", EntryPoint = "gumbo_parse", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gumbo_parse([In] [MarshalAsAttribute(UnmanagedType.LPStr)]string buffer);

        /// Return Type: GumboOutput*
        ///options: GumboOptions*
        ///buffer: char*
        ///buffer_length: size_t->unsigned int
        [DllImportAttribute("gumbo.dll", EntryPoint = "gumbo_parse_with_options", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gumbo_parse_with_options(ref GumboOptions options, [In] [MarshalAsAttribute(UnmanagedType.LPStr)] string buffer, [MarshalAsAttribute(UnmanagedType.SysUInt)] uint buffer_length);

        /// Return Type: void
        ///options: GumboOptions*
        ///output: GumboOutput*
        [DllImportAttribute("gumbo.dll", EntryPoint = "gumbo_destroy_output", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gumbo_destroy_output(ref GumboOptions options, ref GumboOutput output);

        [DllImportAttribute("gumbo.dll", EntryPoint = "gumbo_destroy_output", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gumbo_destroy_output(ref GumboOptions options, IntPtr output);


        [DllImportAttribute("gumbo.dll", EntryPoint = "gumbo_parse_with_options", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gumbo_parse_with_options(ref GumboOptions options, [In] [MarshalAsAttribute(UnmanagedType.LPStr)] string buffer, IntPtr buffer_length);

    }
}