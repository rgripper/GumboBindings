C# bindings for Gumbo - a pure-C HTML5 parser
=============

### Note! Package was moved from `Gumbo.Wrappers` to `Gumbo.Bindings`. Please, readd the package.

[Gumbo parser](https://github.com/google/gumbo-parser) bindings using P/Invokes and marshalling.

You can get it via Nuget: `Install-Package Gumbo.Bindings`

You will also need to place a native `gumbo.dll` along the wrapper library. You can either take one from this repo or compile yourself.

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

### GumboNative (C++ Project)

This project compiles C sources into DLL using .DEF file containing exports used by P/Invokes.

### Gumbo.DefGen (C# Console Project)

Uses dumpbin.exe to generate exports from static library .LIB file and outputs .DEF file, consumed by C compiler.
Static library is compiled by simply switching an option in project settings for Gumbo (C++ Project).
Library name and path to VS bin folder containing dumpbin.exe are set in App.config.
