using Gumbo;
using Gumbo.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gumbo.Wrappers.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //WebClient client = new WebClient();
            //string testHtml = client.DownloadString("http://google.ru/");
            string testHtml = "<html><body class=\"gumbo\">привет!</body></html>";
            using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            {
                Console.WriteLine(gumbo.Document.Root.Elements().ElementAt(1).Children.OfType<TextWrapper>().First().OriginalText);
                Console.WriteLine(gumbo.Document.Root.Elements().ElementAt(1).OriginalTag);
                Console.WriteLine(gumbo.Document.Root.Elements().ElementAt(1).OriginalEndTag);
    //            Console.WriteLine("Attribute name:{0} attribute value:{1}",
    //gumbo.Document.Root.Elements().ElementAt(1).Attributes.First().Name,
    //gumbo.Document.Root.Elements().ElementAt(1).Attributes.First().Value);
    //            Console.WriteLine(gumbo.ToXDocument());
            }
            Console.ReadLine();
        }
    }





   
}
