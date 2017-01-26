using Gumbo.Wrappers;
using Xunit;

namespace UnitTestProject
{

    public class GumboNavigableTest
    {
        public static readonly string TestHtml = "<html><body class=\"gumbo\">boo!<span>Pillz here!</span><p id=\"tag123\"></p></body></html>";

        [Fact]
        public void TestSelectSingleNodeAttribute()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                var node = nav.SelectSingleNode("/html/body/@class");
                Assert.NotNull(node);
                Assert.Equal("gumbo", node.Value);
                Assert.Equal("class", node.Name);
                Assert.Equal("class", node.LocalName);
            }
        }

        [Fact]
        public void TestSelectSingleNodeForElement()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                var node = nav.SelectSingleNode("/html/body/span");
                Assert.NotNull(node);
                Assert.Equal("Pillz here!", node.Value);
                Assert.Equal("span", node.Name);
                Assert.Equal("span", node.LocalName);
            }
        }

        [Fact]
        public void TestMoveToId()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                nav.MoveToId("tag123");
                Assert.Equal("p", nav.Name);
            }
        }

    }
}
