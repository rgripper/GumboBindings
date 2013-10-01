using System;
using System.Runtime.InteropServices;

namespace Gumbo.Bindings
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboErrorContainer
    {
        // The type of error.
        public GumboErrorType type;

        // The position within the source file where the error occurred.
        public GumboSourcePosition position;

        // A pointer to the byte within the original source file text where the error
        // occurred (note that this is not the same as position.offset, as that gives
        // character-based instead of byte-based offsets).
        [MarshalAsAttribute(UnmanagedType.LPStr)]
        public string original_text;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboCodepointErrorContainer : GumboErrorContainer
    {
        // The code point we encountered, for:
        // * GUMBO_ERR_UTF8_INVALID
        // * GUMBO_ERR_UTF8_TRUNCATED
        // * GUMBO_ERR_NUMERIC_CHAR_REF_WITHOUT_SEMICOLON
        // * GUMBO_ERR_NUMERIC_CHAR_REF_INVALID
        public ulong codepoint;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboTokenizerErrorContainer : GumboErrorContainer
    {
        // Tokenizer errors.
        public GumboTokenizerError tokenizer;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboNamedCharErrorContainer : GumboErrorContainer
    {
        // Short textual data, for:
        // * GUMBO_ERR_NAMED_CHAR_REF_WITHOUT_SEMICOLON
        // * GUMBO_ERR_NAMED_CHAR_REF_INVALID
        public GumboStringPiece text;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboDuplicateAttrErrorContainer : GumboErrorContainer
    {
        // Duplicate attribute data, for GUMBO_ERR_DUPLICATE_ATTR.
        public GumboDuplicateAttrError duplicate_attr;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class GumboParserErrorContainer : GumboErrorContainer
    {
        // Parser state, for GUMBO_ERR_PARSER and
        // GUMBO_ERR_UNACKNOWLEDGE_SELF_CLOSING_TAG.
        public GumboParserError parser;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboTokenizerError
    {

        /// int
        public int codepoint;

        /// GumboTokenizerErrorState->Anonymous_55a5f164_2631_451b_91c9_865f5f94eb35
        public GumboTokenizerErrorState state;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboStringPiece
    {
        /** A pointer to the beginning of the string.  NULL iff length == 0. */
        public IntPtr data;

        /** The length of the string fragment, in bytes.  May be zero. */
        public uint length;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct GumboParserError
    {
        // The type of input token that resulted in this error.
        public GumboTokenType input_type;

        // The HTML tag of the input token.  TAG_UNKNOWN if this was not a tag token.
        public GumboTag input_tag;

        // The insertion mode that the parser was in at the time.
        public GumboInsertionMode parser_state;

        // The tag stack at the point of the error.  Note that this is an GumboVector
        // of GumboTag's *stored by value* - cast the void* to an GumboTag directly to
        // get at the tag.
        public GumboVector /* GumboTag */ tag_stack;
    }

    public enum GumboInsertionMode
    {
        GUMBO_INSERTION_MODE_INITIAL,
        GUMBO_INSERTION_MODE_BEFORE_HTML,
        GUMBO_INSERTION_MODE_BEFORE_HEAD,
        GUMBO_INSERTION_MODE_IN_HEAD,
        GUMBO_INSERTION_MODE_IN_HEAD_NOSCRIPT,
        GUMBO_INSERTION_MODE_AFTER_HEAD,
        GUMBO_INSERTION_MODE_IN_BODY,
        GUMBO_INSERTION_MODE_TEXT,
        GUMBO_INSERTION_MODE_IN_TABLE,
        GUMBO_INSERTION_MODE_IN_TABLE_TEXT,
        GUMBO_INSERTION_MODE_IN_CAPTION,
        GUMBO_INSERTION_MODE_IN_COLUMN_GROUP,
        GUMBO_INSERTION_MODE_IN_TABLE_BODY,
        GUMBO_INSERTION_MODE_IN_ROW,
        GUMBO_INSERTION_MODE_IN_CELL,
        GUMBO_INSERTION_MODE_IN_SELECT,
        GUMBO_INSERTION_MODE_IN_SELECT_IN_TABLE,
        GUMBO_INSERTION_MODE_AFTER_BODY,
        GUMBO_INSERTION_MODE_IN_FRAMESET,
        GUMBO_INSERTION_MODE_AFTER_FRAMESET,
        GUMBO_INSERTION_MODE_AFTER_AFTER_BODY,
        GUMBO_INSERTION_MODE_AFTER_AFTER_FRAMESET,
    }

    public enum GumboTokenType
    {
        GUMBO_TOKEN_DOCTYPE,

        GUMBO_TOKEN_START_TAG,

        GUMBO_TOKEN_END_TAG,

        GUMBO_TOKEN_COMMENT,

        GUMBO_TOKEN_WHITESPACE,

        GUMBO_TOKEN_CHARACTER,

        GUMBO_TOKEN_NULL,

        GUMBO_TOKEN_EOF,
    }

    public enum GumboErrorType
    {
        GUMBO_ERR_UTF8_INVALID,

        GUMBO_ERR_UTF8_TRUNCATED,

        GUMBO_ERR_UTF8_NULL,

        GUMBO_ERR_NUMERIC_CHAR_REF_NO_DIGITS,

        GUMBO_ERR_NUMERIC_CHAR_REF_WITHOUT_SEMICOLON,

        GUMBO_ERR_NUMERIC_CHAR_REF_INVALID,

        GUMBO_ERR_NAMED_CHAR_REF_WITHOUT_SEMICOLON,

        GUMBO_ERR_NAMED_CHAR_REF_INVALID,

        GUMBO_ERR_TAG_STARTS_WITH_QUESTION,

        GUMBO_ERR_TAG_EOF,

        GUMBO_ERR_TAG_INVALID,

        GUMBO_ERR_CLOSE_TAG_EMPTY,

        GUMBO_ERR_CLOSE_TAG_EOF,

        GUMBO_ERR_CLOSE_TAG_INVALID,

        GUMBO_ERR_SCRIPT_EOF,

        GUMBO_ERR_ATTR_NAME_EOF,

        GUMBO_ERR_ATTR_NAME_INVALID,

        GUMBO_ERR_ATTR_DOUBLE_QUOTE_EOF,

        GUMBO_ERR_ATTR_SINGLE_QUOTE_EOF,

        GUMBO_ERR_ATTR_UNQUOTED_EOF,

        GUMBO_ERR_ATTR_UNQUOTED_RIGHT_BRACKET,

        GUMBO_ERR_ATTR_UNQUOTED_EQUALS,

        GUMBO_ERR_ATTR_AFTER_EOF,

        GUMBO_ERR_ATTR_AFTER_INVALID,

        GUMBO_ERR_DUPLICATE_ATTR,

        GUMBO_ERR_SOLIDUS_EOF,

        GUMBO_ERR_SOLIDUS_INVALID,

        GUMBO_ERR_DASHES_OR_DOCTYPE,

        GUMBO_ERR_COMMENT_EOF,

        GUMBO_ERR_COMMENT_INVALID,

        GUMBO_ERR_COMMENT_BANG_AFTER_DOUBLE_DASH,

        GUMBO_ERR_COMMENT_DASH_AFTER_DOUBLE_DASH,

        GUMBO_ERR_COMMENT_SPACE_AFTER_DOUBLE_DASH,

        GUMBO_ERR_COMMENT_END_BANG_EOF,

        GUMBO_ERR_DOCTYPE_EOF,

        GUMBO_ERR_DOCTYPE_INVALID,

        GUMBO_ERR_DOCTYPE_SPACE,

        GUMBO_ERR_DOCTYPE_RIGHT_BRACKET,

        GUMBO_ERR_DOCTYPE_SPACE_OR_RIGHT_BRACKET,

        GUMBO_ERR_DOCTYPE_END,

        GUMBO_ERR_PARSER,

        GUMBO_ERR_UNACKNOWLEDGED_SELF_CLOSING_TAG,
    }

    public enum GumboTokenizerErrorState
    {

        GUMBO_ERR_TOKENIZER_DATA,

        GUMBO_ERR_TOKENIZER_CHAR_REF,

        GUMBO_ERR_TOKENIZER_RCDATA,

        GUMBO_ERR_TOKENIZER_RAWTEXT,

        GUMBO_ERR_TOKENIZER_PLAINTEXT,

        GUMBO_ERR_TOKENIZER_SCRIPT,

        GUMBO_ERR_TOKENIZER_TAG,

        GUMBO_ERR_TOKENIZER_SELF_CLOSING_TAG,

        GUMBO_ERR_TOKENIZER_ATTR_NAME,

        GUMBO_ERR_TOKENIZER_ATTR_VALUE,

        GUMBO_ERR_TOKENIZER_MARKUP_DECLARATION,

        GUMBO_ERR_TOKENIZER_COMMENT,

        GUMBO_ERR_TOKENIZER_DOCTYPE,

        GUMBO_ERR_TOKENIZER_CDATA,
    }
}
