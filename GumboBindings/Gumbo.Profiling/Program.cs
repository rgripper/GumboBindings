using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using System.Text;
using System.Threading.Tasks;
using Gumbo.Wrappers;
using System.Diagnostics;

namespace Gumbo.Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1;

            Stopwatch watch;
                
            //watch = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
                {
                    gumbo.MarshalAll();
                    var nav = gumbo.CreateNavigator();
                    watch = Stopwatch.StartNew();
                    nav.SelectSingleNode("/html/body/@class");
                    Console.WriteLine("***** " + watch.Elapsed);
                    watch = Stopwatch.StartNew();
                    nav.SelectSingleNode("/html/body/p");
                    Console.WriteLine("***** " + watch.Elapsed);
                    watch = Stopwatch.StartNew();
                    nav.SelectSingleNode("/html/body/@class");
                    Console.WriteLine("***** " + watch.Elapsed);
                    watch = Stopwatch.StartNew();
                    nav.SelectSingleNode("/html/body/p");
                    Console.WriteLine("***** " + watch.Elapsed);
                }
            }

            

            //watch = Stopwatch.StartNew();


            //Console.WriteLine(watch.Elapsed);

            Console.ReadLine();
        }

        public static readonly string TestHtml = "<html><body class=\"gumbo\">boo!<span>Pillz here!</span><p id=\"tag123\"></p></body></html>";

        public static void TestSelectSingleNodeAttribute()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                var node = nav.SelectSingleNode("/html/body/@class");
            }
        }

        public static void TestSelectSingleNodeAttribute_XLinq()
        {
            var document = System.Xml.Linq.XDocument.Parse(TestHtml);

            var nav = document.CreateNavigator();
            var node = nav.SelectSingleNode("/html/body/@class");

        }

        public static void TestSelectSingleNodeForElement()
        {
            using (GumboWrapper gumbo = new GumboWrapper(TestHtml))
            {
                var nav = gumbo.CreateNavigator();
                var node = nav.SelectSingleNode("/html/body/p");
            }
        }
    }
}
