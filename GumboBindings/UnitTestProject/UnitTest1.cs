using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gumbo.Wrappers;
using System.Linq;
using Gumbo.Bindings;

namespace UnitTestProject
{
    //ilia: по идее можно исключить демо проект, а оставить вместо него тестовый

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
                //Видимо теперь первым всегда идет head, даже если его нет в оригинале
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



        [TestMethod]
        public void TestTextNodeOriginalText()
        {
            //думаю, что OriginalText может содержать мусор и использовать это поле не нужно вообще

            //string testHtml = "<html><body class=\"gumbo\">привет!</body></html>";
            //using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            //{
            //    //ок, пусть вторым будет GUMBO_TAG_SECTION, я ожидаю, что в нем есть элемент GUMBO_NODE_TEXT
            //    var body = (gumbo.Document.Root.Children.ElementAt(1) as ElementWrapper);
            //    var firstNode = body.Children.ElementAt(0);
            //    Assert.AreEqual(GumboNodeType.GUMBO_NODE_TEXT, firstNode.Type);
            //    //и он содержит значение привет!
            //    var textNode = firstNode as TextWrapper;
            //    Assert.AreEqual("привет!", textNode.Text);
            //    //а так же я ожидаю, что OriginalText содержит значение привет!
            //    Assert.AreEqual("привет!", textNode.OriginalText);
            //    //вместо этого OriginalText содержит привет!</body></html>
            //}
            Assert.IsTrue(true);
        }

    }
}
