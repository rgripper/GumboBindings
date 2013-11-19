using System;
using System.Runtime.InteropServices;

namespace Gumbo.Bindings
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboNode
    {
        public GumboNodeType type;

        public IntPtr parent;

        /// <summary>
        /// The index within the parent's children vector of this node.
        /// </summary>
        public uint index_within_parent;

        /// <summary>
        /// Flags containing information about why this element was
        /// inserted into the parse tree, including a variety of special parse
        /// situations.
        /// </summary>
        public GumboParseFlags parse_flags;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboElementNode : GumboNode
    {
        public GumboElement element;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboDocumentNode : GumboNode
    {
        public GumboDocument document;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboTextNode : GumboNode
    {
        public GumboText text;
    }

    /// <summary>
    /// Input struct containing configuration options for the parser.
    /// These let you specify alternate memory managers, provide different error
    /// handling, etc.
    /// Use kGumboDefaultOptions for sensible defaults, and only set what you need.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboOptions
    {
        /// <summary>
        /// A memory allocator function.  Default: malloc.
        /// </summary>
        IntPtr allocator;

        /// <summary>
        /// A memory deallocator function. Default: free.
        /// </summary>
        IntPtr deallocator;

        /// <summary>
        /// An opaque object that's passed in as the first argument to all callbacks
        /// used by this library.  Default: NULL.
        /// </summary>
        IntPtr userdata;

        /// <summary>
        /// The tab-stop size, for computing positions in source code that uses tabs.
        /// Default: 8.
        /// </summary>
        public int tab_stop;

        /// <summary>
        /// Whether or not to stop parsing when the first error is encountered.
        /// Default: false.
        /// </summary>
        public bool stop_on_first_error;

        /// <summary>
        /// The maximum number of errors before the parser stops recording them.  This
        /// is provided so that if the page is totally borked, we don't completely fill
        /// up the errors vector and exhaust memory with useless redundant errors.  Set
        /// to -1 to disable the limit.
        /// Default: -1
        /// </summary>
        public int max_errors;
    }

    /// <summary>
    /// The output struct containing the results of the parse.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboOutput
    {
        /// <summary>
        /// Pointer to the document node.  This is a GumboNode of type NODE_DOCUMENT
        /// that contains the entire document as its child.
        /// </summary>
        public IntPtr document;

        /// <summary>
        /// Pointer to the root node.  This the <html> tag that forms the root of the
        /// document.
        /// </summary>
        public IntPtr root;

        /// <summary>
        /// A list of errors that occurred during the parse.
        /// NOTE: In version 1.0 of this library, the API for errors hasn't been fully
        /// fleshed out and may change in the future.  For this reason, the GumboError
        /// header isn't part of the public API.  Contact us if you need errors
        /// reported so we can work out something appropriate for your use-case.
        /// </summary>
        public GumboVector errors;
    }

    public enum GumboNodeType
    {
        GUMBO_NODE_DOCUMENT,

        GUMBO_NODE_ELEMENT,

        GUMBO_NODE_TEXT,

        GUMBO_NODE_CDATA,

        GUMBO_NODE_COMMENT,

        /// <summary>
        /// Text node, where all contents is whitespace.
        /// </summary>
        GUMBO_NODE_WHITESPACE,
    }

    /// <summary>
    /// Parse flags.
    /// We track the reasons for parser insertion of nodes and store them in a
    /// bitvector in the node itself.  This lets client code optimize out nodes that
    /// are implied by the HTML structure of the document, or flag constructs that
    /// may not be allowed by a style guide, or track the prevalence of incorrect or
    /// tricky HTML code.
    /// </summary>
    [Flags]
    public enum GumboParseFlags
    {
        /// <summary>
        /// A normal node - both start and end tags appear in the source, nothing has
        /// been reparented.
        /// </summary>
        GUMBO_INSERTION_NORMAL = 0,

        /// <summary>
        /// A node inserted by the parser to fulfill some implicit insertion rule.
        /// This is usually set in addition to some other flag giving a more specific
        /// insertion reason; it's a generic catch-all term meaning "The start tag for
        /// this node did not appear in the document source".
        /// </summary>
        GUMBO_INSERTION_BY_PARSER = (1) << (0),

        /// <summary>
        /// A flag indicating that the end tag for this node did not appear in the
        /// document source.  Note that in some cases, you can still have
        /// parser-inserted nodes with an explicit end tag: for example, "Text&lt;/html&gt;"
        /// has GUMBO_INSERTED_BY_PARSER set on the &lt;html&gt; node, but
        /// GUMBO_INSERTED_END_TAG_IMPLICITLY is unset, as the &lt;/html&gt; tag actually
        /// exists.  This flag will be set only if the end tag is completely missing;
        /// in some cases, the end tag may be misplaced (eg. a &lt;/body&gt; tag with text
        /// afterwards), which will leave this flag unset and require clients to
        /// inspect the parse errors for that case.
        /// </summary>
        GUMBO_INSERTION_IMPLICIT_END_TAG = (1) << (1),

        /// <summary>
        /// A flag for nodes that are inserted because their presence is implied by
        /// other tags, eg. &lt;html&gt;, &lt;head&gt;, &lt;body&gt;, &lt;tbody&gt;, etc.
        /// </summary>
        GUMBO_INSERTION_IMPLIED = (1) << (3),

        /// <summary>
        /// A flag for nodes that are converted from their end tag equivalents.  For
        /// example, &lt;/p&gt; when no paragraph is open implies that the parser should
        /// create a &lt;p&gt; tag and immediately close it, while &lt;/br&gt; means the same thing
        /// as &lt;br&gt;.
        /// </summary>
        GUMBO_INSERTION_CONVERTED_FROM_END_TAG = (1) << (4),

        /// <summary>
        /// A flag for nodes that are converted from the parse of an &lt;isindex&gt; tag.
        /// </summary>
        GUMBO_INSERTION_FROM_ISINDEX = (1) << (5),

        /// <summary>
        /// A flag for &lt;image&gt; tags that are rewritten as &lt;img&gt;.
        /// </summary>
        GUMBO_INSERTION_FROM_IMAGE = (1) << (6),

        /// <summary>
        /// A flag for nodes that are cloned as a result of the reconstruction of
        /// active formatting elements.  This is set only on the clone; the initial
        /// portion of the formatting run is a NORMAL node with an IMPLICIT_END_TAG.
        /// </summary>
        GUMBO_INSERTION_RECONSTRUCTED_FORMATTING_ELEMENT = (1) << (7),

        /// <summary>
        /// A flag for nodes that are cloned by the adoption agency algorithm.
        /// </summary>
        GUMBO_INSERTION_ADOPTION_AGENCY_CLONED = (1) << (8),

        /// <summary>
        /// A flag for nodes that are moved by the adoption agency algorithm.
        /// </summary>
        GUMBO_INSERTION_ADOPTION_AGENCY_MOVED = (1) << (9),

        /// <summary>
        /// A flag for nodes that have been foster-parented out of a table (or
        /// should've been foster-parented, if verbatim mode is set).
        /// </summary>
        GUMBO_INSERTION_FOSTER_PARENTED = (1) << (10),
    }

    /// <summary>
    /// A simple vector implementation.  This stores a pointer to a data array and a
    /// length.  All elements are stored as void///; client code must cast to the
    /// appropriate type.  Overflows upon addition result in reallocation of the data
    /// array, with the size doubling to maintain O(1) amortized cost.  There is no
    /// removal function, as this isn't needed for any of the operations within this
    /// library.  Iteration can be done through inspecting the structure directly in
    /// a for-loop.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboVector
    {
        /// <summary>
        /// Data elements.  This points to a dynamically-allocated array of capacity
        /// elements, each a void* to the element itself.
        /// </summary>
        public IntPtr data;

        /// <summary>
        /// Number of elements currently in the vector.
        /// </summary>
        public uint length;

        /// <summary>
        /// Current array capacity.
        /// </summary>
        public uint capacity;
    }

    /// <summary>
    /// An enum for all the tags defined in the HTML5 standard.  These correspond to
    /// the tag names themselves.  Enum constants exist only for tags which appear in
    /// the spec itself (or for tags with special handling in the SVG and MathML
    /// namespaces); any other tags appear as GUMBO_TAG_UNKNOWN and the actual tag
    /// name can be obtained through original_tag.
    /// </summary>
    /// <remarks>
    /// This is mostly for API convenience, so that clients of this library don't
    /// need to perform a strcasecmp to find the normalized tag name.  It also has
    /// efficiency benefits, by letting the parser work with enums instead of
    /// strings.
    /// </remarks>
    public enum GumboTag
    {
        /// <summary>
        ///  http://www.whatwg.org/specs/web-apps/current-work/multipage/semantics.html#the-root-element
        /// </summary>
        GUMBO_TAG_HTML,

        /// <summary>
        ///  http://www.whatwg.org/specs/web-apps/current-work/multipage/semantics.html#document-metadata
        /// </summary>
        GUMBO_TAG_HEAD,
        GUMBO_TAG_TITLE,
        GUMBO_TAG_BASE,
        GUMBO_TAG_LINK,
        GUMBO_TAG_META,
        GUMBO_TAG_STYLE,

        /// <summary>
        ///  http://www.whatwg.org/specs/web-apps/current-work/multipage/scripting-1.html#scripting-1
        /// </summary>
        GUMBO_TAG_SCRIPT,
        GUMBO_TAG_NOSCRIPT,
        GUMBO_TAG_TEMPLATE,

        /// <summary>
        ///  http://www.whatwg.org/specs/web-apps/current-work/multipage/sections.html#sections
        /// </summary>
        GUMBO_TAG_BODY,
        GUMBO_TAG_ARTICLE,
        GUMBO_TAG_SECTION,
        GUMBO_TAG_NAV,
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

        /// <summary>
        ///  http://www.whatwg.org/specs/web-apps/current-work/multipage/grouping-content.html#grouping-content
        /// </summary>
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
        GUMBO_TAG_MAIN,
        GUMBO_TAG_DIV,

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/text-level-semantics.html#text-level-semantics
        /// </summary>
        GUMBO_TAG_A,
        GUMBO_TAG_EM,
        GUMBO_TAG_STRONG,
        GUMBO_TAG_SMALL,
        GUMBO_TAG_S,
        GUMBO_TAG_CITE,
        GUMBO_TAG_Q,
        GUMBO_TAG_DFN,
        GUMBO_TAG_ABBR,
        GUMBO_TAG_DATA,
        GUMBO_TAG_TIME,
        GUMBO_TAG_CODE,
        GUMBO_TAG_VAR,
        GUMBO_TAG_SAMP,
        GUMBO_TAG_KBD,
        GUMBO_TAG_SUB,
        GUMBO_TAG_SUP,
        GUMBO_TAG_I,
        GUMBO_TAG_B,
        GUMBO_TAG_U,
        GUMBO_TAG_MARK,
        GUMBO_TAG_RUBY,
        GUMBO_TAG_RT,
        GUMBO_TAG_RP,
        GUMBO_TAG_BDI,
        GUMBO_TAG_BDO,
        GUMBO_TAG_SPAN,
        GUMBO_TAG_BR,
        GUMBO_TAG_WBR,

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/edits.html#edits
        /// </summary>
        GUMBO_TAG_INS,
        GUMBO_TAG_DEL,

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/embedded-content-1.html#embedded-content-1
        /// </summary>
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

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/the-map-element.html#mathml
        /// </summary>
        GUMBO_TAG_MATH,
        GUMBO_TAG_MI,
        GUMBO_TAG_MO,
        GUMBO_TAG_MN,
        GUMBO_TAG_MS,
        GUMBO_TAG_MTEXT,
        GUMBO_TAG_MGLYPH,
        GUMBO_TAG_MALIGNMARK,
        GUMBO_TAG_ANNOTATION_XML,

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/the-map-element.html#svg-0
        /// </summary>
        GUMBO_TAG_SVG,
        GUMBO_TAG_FOREIGNOBJECT,
        GUMBO_TAG_DESC,

        /// <summary>
        /// SVG title tags will have GUMBO_TAG_TITLE as with HTML.
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/tabular-data.html#tabular-data
        /// </summary>
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

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/forms.html#forms
        /// </summary>
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

        /// <summary>
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/interactive-elements.html#interactive-elements
        /// </summary>
        GUMBO_TAG_DETAILS,
        GUMBO_TAG_SUMMARY,
        GUMBO_TAG_MENU,
        GUMBO_TAG_MENUITEM,

        /// <summary>
        /// Non-conforming elements that nonetheless appear in the HTML5 spec.
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/obsolete.html#non-conforming-features
        /// </summary>
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

        /// <summary>
        /// Used for all tags that don't have special handling in HTML.
        /// </summary>
        GUMBO_TAG_UNKNOWN,

        /// <summary>
        /// A marker value to indicate the end of the enum, for iterating over it.
        /// Also used as the terminator for varargs functions that take tags.
        /// </summary>
        GUMBO_TAG_LAST,
    }

    /// <summary>
    /// A struct representing a single attribute on an HTML tag.  This is a
    /// name-value pair, but also includes information about source locations and
    /// original source text.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboAttribute
    {
        /// <summary>
        /// The namespace for the attribute.  This will usually be
        /// GUMBO_ATTR_NAMESPACE_NONE, but some XLink/XMLNS/XML attributes take special
        /// values, per:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/tree-construction.html#adjust-foreign-attributes
        /// </summary>
        public GumboAttributeNamespaceEnum attr_namespace;

        /// <summary>
        /// The name of the attribute.  This is in a freshly-allocated buffer to deal
        /// with case-normalization, and is null-terminated.
        /// </summary>
        public IntPtr name;

        /// <summary>
        /// The original text of the attribute name, as a pointer into the original
        /// source buffer.
        /// </summary>
        public GumboStringPiece original_name;

        /// <summary>
        /// The value of the attribute.  This is in a freshly-allocated buffer to deal
        /// with unescaping, and is null-terminated.  It does not include any quotes
        /// that surround the attribute.  If the attribute has no value (for example,
        /// 'selected' on a checkbox), this will be an empty string.
        /// </summary>
        public IntPtr value;

        /// <summary>
        /// The original text of the value of the attribute.  This points into the
        /// original source buffer.  It includes any quotes that surround the
        /// attribute, and you can look at original_value.data[0] and
        /// original_value.data[original_value.length - 1] to determine what the quote
        /// characters were.  If the attribute has no value, this will be a 0-length
        /// string.
        /// </summary>
        public GumboStringPiece original_value;

        /// <summary>
        /// The starting position of the attribute name.
        /// </summary>
        public GumboSourcePosition name_start;

        /// <summary>
        /// The ending position of the attribute name.  This is not always derivable
        /// from the starting position of the value because of the possibility of
        /// whitespace around the = sign.
        /// </summary>
        public GumboSourcePosition name_end;

        /// <summary>
        /// The starting position of the attribute value.
        /// </summary>
        public GumboSourcePosition value_start;

        /// <summary>
        /// The ending position of the attribute value.
        /// </summary>
        public GumboSourcePosition value_end;
    }

    /// <summary>
    /// Information specific to document nodes.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboDocument
    {
        /// <summary>
        /// An array of GumboNodes, containing the children of this element.  This will
        /// normally consist of the <html> element and any comment nodes found.
        /// Pointers are owned.
        /// </summary>
        public GumboVector children;

        /// <summary>
        /// True if there was an explicit doctype token as opposed to it being omitted.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool has_doctype;

        /// <summary>
        /// A field from the doctype token, copied verbatim.
        /// </summary>
        public IntPtr name;

        /// <summary>
        /// A field from the doctype token, copied verbatim.
        /// </summary>
        public IntPtr public_identifier;

        /// <summary>
        /// A field from the doctype token, copied verbatim.
        /// </summary>
        public IntPtr system_identifier;

        /// <summary>
        /// Whether or not the document is in QuirksMode, as determined by the values
        /// in the GumboTokenDocType template.
        /// </summary>
        public GumboQuirksModeEnum doc_type_quirks_mode;
    }

    /// <summary>
    /// The struct used to represent all HTML elements.  This contains information
    /// about the tag, attributes, and child nodes.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboElement
    {
        /// <summary>
        /// An array of GumboNodes, containing the children of this element.  Pointers
        /// are owned.
        /// </summary>
        public GumboVector children;

        public GumboTag tag;

        public GumboNamespaceEnum tag_namespace;

        /// <summary>
        /// A GumboStringPiece pointing to the original tag text for this element,
        /// pointing directly into the source buffer.  If the tag was inserted
        /// algorithmically (for example, &lt;head&gt; or &lt;tbodygt; insertion), this will be a
        /// zero-length string.
        /// </summary>
        public GumboStringPiece original_tag;

        /// <summary>
        /// A GumboStringPiece pointing to the original end tag text for this element.
        /// If the end tag was inserted algorithmically, (for example, closing a
        /// self-closing tag), this will be a zero-length string.
        /// </summary>
        public GumboStringPiece original_end_tag;

        /// <summary>
        /// The source position for the start of the start tag.
        /// </summary>
        public GumboSourcePosition start_pos;

        /// <summary>
        /// An array of GumboAttributes, containing the attributes for this tag in the
        /// order that they were parsed.  Pointers are owned.
        /// </summary>
        public GumboSourcePosition end_pos;

        public GumboVector attributes;
    }

    /// <summary>
    /// The struct used to represent TEXT, CDATA, COMMENT, and WHITESPACE elements.
    /// This contains just a block of text and its position.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboText
    {
        /// <summary>
        /// The text of this node, after entities have been parsed and decoded.  For
        /// comment/cdata nodes, this does not include the comment delimiters.
        /// </summary>
        public IntPtr text;

        /// <summary>
        /// The original text of this node, as a pointer into the original buffer.  For
        /// comment/cdata nodes, this includes the comment delimiters.
        /// </summary>
        public GumboStringPiece original_text;

        /// <summary>
        /// The starting position of this node.  This corresponds to the position of
        /// original_text, before entities are decoded.
        /// </summary>
        public GumboSourcePosition start_pos;
    }

    /// <summary>
    /// Attribute namespaces.
    /// HTML includes special handling for XLink, XML, and XMLNS namespaces on
    /// attributes.  Everything else goes in the generatic "NONE" namespace.
    /// </summary>
    public enum GumboAttributeNamespaceEnum
    {
        GUMBO_ATTR_NAMESPACE_NONE,

        GUMBO_ATTR_NAMESPACE_XLINK,

        GUMBO_ATTR_NAMESPACE_XML,

        GUMBO_ATTR_NAMESPACE_XMLNS,
    }

    /// <summary>
    /// A struct representing a character position within the original text buffer.
    /// Line and column numbers are 1-based and offsets are 0-based, which matches
    /// how most editors and command-line tools work.  Also, columns measure
    /// positions in terms of characters while offsets measure by bytes; this is
    /// because the offset field is often used to pull out a particular region of
    /// text (which in most languages that bind to C implies pointer arithmetic on a
    /// buffer of bytes), while the column field is often used to reference a
    /// particular column on a printable display, which nowadays is usually UTF-8.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboSourcePosition
    {
        public uint line;

        public uint column;

        public uint offset;
    }

    /// <summary>
    /// http://www.whatwg.org/specs/web-apps/current-work/complete/dom.html#quirks-mode
    /// </summary>
    public enum GumboQuirksModeEnum
    {
        GUMBO_DOCTYPE_NO_QUIRKS,

        GUMBO_DOCTYPE_QUIRKS,

        GUMBO_DOCTYPE_LIMITED_QUIRKS,
    }

    /// <summary>
    /// Namespaces.
    /// Unlike in X(HT)ML, namespaces in HTML5 are not denoted by a prefix.  Rather,
    /// anything inside an &lt;svg&gt; tag is in the SVG namespace, anything inside the
    /// &lt;math&gt; tag is in the MathML namespace, and anything else is inside the HTML
    /// namespace.  No other namespaces are supported, so this can be an enum only.
    /// </summary>
    public enum GumboNamespaceEnum
    {
        GUMBO_NAMESPACE_HTML,

        GUMBO_NAMESPACE_SVG,

        GUMBO_NAMESPACE_MATHML,
    }

    public partial class NativeMethods
    {
        private const string LibraryName = "gumbo.dll";

        /// <summary>
        /// Extracts the tag name from the original_text field of an element or token by
        /// stripping off &lt;/&gt; characters and attributes and adjusting the passed-in
        /// GumboStringPiece appropriately.  The tag name is in the original case and
        /// shares a buffer with the original text, to simplify memory management.
        /// Behavior is undefined if a string-piece that doesn't represent an HTML tag
        /// (&lt;tagname&gt; or &lt;/tagname&gt;) is passed in. If the string piece is completely
        /// empty (NULL data pointer), then this function will exit successfully as a
        /// no-op.
        /// </summary>
        /// <param name="text"></param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gumbo_tag_from_original_text(ref GumboStringPiece text);

        /// <summary>
        /// Returns the normalized (usually all-lowercased, except for foreign content)
        /// tag name for an GumboTag enum.  Return value is static data owned by the
        /// library.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gumbo_normalized_tagname(GumboTag tag);

        /// <summary>
        /// Parses a buffer of UTF8 text into an GumboNode parse tree.  The buffer must
        /// live at least as long as the parse tree, as some fields (eg. original_text)
        /// point directly into the original buffer.
        /// </summary>
        /// <remarks>
        /// This doesn't support buffers longer than 4 gigabytes.
        /// </remarks>
        /// <param name="buffer"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gumbo_parse(IntPtr buffer);

        /// <summary>
        /// Extended version of <see cref="gumbo_parse"/> that takes an explicit options structure,
        /// buffer, and length.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="buffer"></param>
        /// <param name="buffer_length"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gumbo_parse_with_options(ref GumboOptions options, IntPtr buffer, [MarshalAs(UnmanagedType.SysUInt)] uint buffer_length);

        /// <summary>
        /// Release the memory used for the parse tree and parse errors.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="output"></param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gumbo_destroy_output(ref GumboOptions options, IntPtr output);

        /// <summary>
        /// Create options for method <see cref="gumbo_parse_with_options"/>.
        /// </summary>
        /// <param name="tab_stop">The tab-stop size, for computing positions in source code that uses tabs.</param>
        /// <param name="stop_on_first_error"></param>
        /// <param name="max_errors"></param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gumbo_set_options_defaults(ref GumboOptions options);
    }
}