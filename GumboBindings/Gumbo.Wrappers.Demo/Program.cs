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
            string testHtml = "<html><body class=\"gumbo\">Boo!</body></html>";
            using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            {
                Console.WriteLine(gumbo.Document.Root.Elements().ElementAt(1).Children.OfType<TextWrapper>().First().Text);
                Console.WriteLine(gumbo.ToXDocument());
            }
            Console.ReadLine();
        }
    }





   
}
