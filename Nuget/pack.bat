nuget pack ..\GumboBindings\Gumbo.Wrappers\Gumbo.Wrappers.csproj -IncludeReferencedProjects -Prop Configuration=Release
nuget push Gumbo.Wrappers.*.nupkg
