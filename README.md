Gumbo Bindings for C#
=============

Google [Gumbo HTML5 parsing library](https://github.com/google/gumbo-parser) bindings.

Basics
=============

            string testHtml = "<html><body class=\"gumbo\">Boo!</body></html>";
            using (GumboWrapper gumbo = new GumboWrapper(testHtml))
            {
                Console.WriteLine(gumbo.Document.Root.Elements().ElementAt(1).Children.OfType<TextWrapper>().First().Text);
                Console.WriteLine(gumbo.ToXDocument()); // to XDocument
            }
            Console.ReadLine();

