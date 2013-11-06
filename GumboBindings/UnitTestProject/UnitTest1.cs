using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gumbo.Wrappers;
using System.Linq;
using Gumbo.Bindings;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFirstAndLastTagsInEnum()
        {
            string testHtml = "<html><head><head><body><title></title><base></base><tt></tt><unknown123></unknown123></body></html>";
            using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            {
                var list = gumbo.Document.Root.Children.OfType<ElementWrapper>().ToList();
                Assert.AreEqual(GumboTag.GUMBO_TAG_HEAD, list[0].Tag);
                Assert.AreEqual("<head>", list[0].OriginalTag);
                var body = list[1].Children.OfType<ElementWrapper>().ToList();
                Assert.AreEqual(GumboTag.GUMBO_TAG_TITLE, body[0].Tag);
                Assert.AreEqual(GumboTag.GUMBO_TAG_BASE, body[1].Tag);
                Assert.AreEqual(GumboTag.GUMBO_TAG_TT, body[2].Tag);
                Assert.AreEqual(GumboTag.GUMBO_TAG_UNKNOWN, body[3].Tag);
            }
        }

        [TestMethod]
        public void TestHeadBody()
        {
            string testHtml = "<html><body class=\"gumbo\">привет!</body></html>";
            using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            {
                var list = gumbo.Document.Root.Children.OfType<ElementWrapper>().ToList();
                Assert.AreEqual(GumboTag.GUMBO_TAG_HEAD, list[0].Tag);
                Assert.AreEqual(null, list[0].OriginalTag);
                Assert.AreEqual(GumboTag.GUMBO_TAG_BODY, list[1].Tag);
            }
        }

        [TestMethod]
        public void TestAtributes()
        {
            string testHtml = "<html><body class=\"gumbo\">привет!</body></html>";
            using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            {
                var list = gumbo.Document.Root.Children.OfType<ElementWrapper>().ToList();

                Assert.AreEqual("class", list[1].Attributes.First().Name);
                Assert.AreEqual("gumbo", list[1].Attributes.First().Value);
            }
        }
    }
}
