C# bindings for Gumbo - a pure-C HTML5 parser
=============

[Gumbo parser](https://github.com/google/gumbo-parser) bindings using P/Invokes and marshalling.
Types member names are preserved the way they appear in the original code to make API change tracking easier.
To simplify usage there are wrappers for the main classes.

Basic usage of the wrapper classes
=============

    string testHtml = "<html><body class=\"gumbo\">Boo!</body></html>";
    using (GumboWrapper gumbo = new GumboWrapper(testHtml))
    {
        Console.WriteLine(gumbo.Document.Root.Elements().ElementAt(1).Children.OfType<TextWrapper>().First().Text); // Boo!
        Console.WriteLine(gumbo.ToXDocument()); // to XDocument
    }
    Console.ReadLine();

It produces the following output:

    Boo!
    <html>
      <head />
      <body class="gumbo">Boo!</body>
    </html>

