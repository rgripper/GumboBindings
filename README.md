C# bindings for Gumbo - a pure-C HTML5 parser
=============

[Gumbo parser](https://github.com/google/gumbo-parser) bindings using P/Invokes and marshalling.

You can get it via Nuget (https://www.nuget.org/packages/Gumbo.Wrappers/).

Types member names are preserved the way they appear in the original code to make API change tracking easier.
To simplify usage there are wrappers for the main classes.

## Basic usage of the wrapper classes

    string testHtml = "<html><body class=\"gumbo\">Boo!</body></html>";
    using (GumboWrapper gumbo = new GumboWrapper(testHtml))
    {
        Console.WriteLine(gumbo.Document.Root.Children.OfType<ElementWrapper>().ElementAt(1).Children.OfType<TextWrapper>().First().Text); // Boo!
        Console.WriteLine(gumbo.ToXDocument()); // to XDocument
    }
    Console.ReadLine();

It produces the following output:

    Boo!
    <html>
      <head />
      <body class="gumbo">Boo!</body>
    </html>

## Utility projects

### Gumbo (C++ Project)

This project compiles C sources into DLL using .DEF file containing exports used by P/Invokes.

### Gumbo.DefGen (C# Console Project)

Uses dumpbin.exe to generate exports from static library .LIB file and outputs .DEF file, consumed by C compiler.
Static library is compiled by simply switching an option in project settings for Gumbo (C++ Project).
Library name and path to VS bin folder containing dumpbin.exe are set in App.config.
