using Gumbo;
using Gumbo.Wrappers;
using System;
using System.Linq;
using Xunit;

namespace UnitTestProject
{
    public class GumboWrapperTest
    {
        [Fact]
        public void TestFirstAndLastTagsInEnum()
        {
            string testHtml = "<html><head><head><body><title></title><base></base><tt></tt><unknown123></unknown123></body></html>";
            using (var gumbo = new GumboWrapper(testHtml))
            {
                var list = gumbo.Document.Root.Children.OfType<ElementWrapper>().ToList();
                Assert.Equal(GumboTag.GUMBO_TAG_HEAD, list[0].Tag);
                Assert.Equal("<head>", list[0].OriginalTag);
                var body = list[1].Children.OfType<ElementWrapper>().ToList();
                Assert.Equal(GumboTag.GUMBO_TAG_TITLE, body[0].Tag);
                Assert.Equal(GumboTag.GUMBO_TAG_BASE, body[1].Tag);
                Assert.Equal(GumboTag.GUMBO_TAG_TT, body[2].Tag);
                Assert.Equal(GumboTag.GUMBO_TAG_UNKNOWN, body[3].Tag);
            }
        }

        [Fact]
        public void TestHeadBody()
        {
            string testHtml = "<html><body class=\"gumbo\">привет!</body></html>";
            using (var gumbo = new GumboWrapper(testHtml))
            {
                var list = gumbo.Document.Root.Children.OfType<ElementWrapper>().ToList();
                Assert.Equal(GumboTag.GUMBO_TAG_HEAD, list[0].Tag);
                Assert.Equal(null, list[0].OriginalTag);
                Assert.Equal(GumboTag.GUMBO_TAG_BODY, list[1].Tag);
            }
        }

        [Fact]
        public void TestAttributes()
        {
            string testHtml = "<html><body class=\"gumbo\">привет!</body></html>";
            using (var gumbo = new GumboWrapper(testHtml))
            {
                var list = gumbo.Document.Root.Children.OfType<ElementWrapper>().ToList();

                Assert.Equal("class", list[1].Attributes.First().Name);
                Assert.Equal("gumbo", list[1].Attributes.First().Value);
            }
        }
    }
}
