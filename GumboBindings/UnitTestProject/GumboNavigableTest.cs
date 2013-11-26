using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gumbo.Wrappers;
using System.Linq;
using System.Xml.XPath;
using Gumbo.Bindings;

namespace UnitTestProject
{
    [TestClass]
    public class GumboNavigableTest
    {
        public static readonly string TestHtml = "<html><body class=\"gumbo\">boo!<span>Pillz here!</span><p id=\"tag123\"></p></body></html>";

        [TestMethod]
        public void TestSelectSingleNodeAttribute()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                var node = nav.SelectSingleNode("/html/body/@class");
                Assert.AreEqual("gumbo", node.Value);
                Assert.AreEqual("class", node.Name);
                Assert.AreEqual("class", node.LocalName);
            }
        }

        [TestMethod]
        public void TestSelectSingleNodeForElement()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                var node = nav.SelectSingleNode("/html/body/span");
                Assert.AreEqual("Pillz here!", node.Value);
                Assert.AreEqual("span", node.Name);
                Assert.AreEqual("span", node.LocalName);
            }
        }

        [TestMethod]
        public void TestMoveToId()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                nav.MoveToId("tag123");
                Assert.AreEqual("p", nav.Name);
            }
        }

    }
}
