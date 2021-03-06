﻿# Using the HandlebarsCompiler.ViewEngine.Core in a asp.net core mvc project

1. Add the CompiledHandlebars.ViewEngine.Core and CompiledHandlebars.RuntimeUtils Nuget Package
2. Add the CompiledHandlebarsViewEngine to the MVC-Services and configure the ViewLocationFormats.
```csharp
var ve_options = new CompiledHandlebarsViewEngineOptions()
{
	ViewLocationFormats = new string[] { "~/Views/{1}/{0}.hbs", "~/Views/{0}.hbs", "~/{0}.hbs" 
};

services.AddMvc().AddViewOptions(options =>
{
		options.ViewEngines.Add(new CompiledHandlebarsViewEngine(typeof(Startup).GetTypeInfo().Assembly, ve_options));
});
```	
3. Call the View in the Controller by Name and pass any ViewModel
```csharp
	return View("ViewName", viewModel);
```

# Using the HandlebarsCompiler in an asp.net core (.net core) project

Because of a [bug](https://github.com/dotnet/roslyn/issues/20808), the HandlebarsCompiler does not work correctly for asp.net core projects that target the .net core framework.

Fortunately, there is a workaround for this bug.
The asp.net core project must have a project reference to a .net core project (not asp.net core).

Handlebars-Templates **must** have their `BuildAction` set to **`C# analyzer additional file`** in order to be found by the compiler!

So: here is how to use the HandlebarsCompiler with asp.net core projects:
1. Put the HandlebarsCompiler.exe in your PATH
2. Add the following to your .csproj file. Adding a PreBuild-Event is possible, too, but Macros are not supported there (https://github.com/dotnet/sdk/issues/677).
Pay attention to the `-c` flag that tells the compiler to not add compiled files to the project file.
```
<Target Name="PreBuild" BeforeTargets="PreBuildEvent">
	<Exec Command="HandlebarsCompiler.exe -c $(ProjectPath)" />
</Target>
```
3. Create an empty dummy project that targets .net core and create a project reference to it
4. Create a Handlebars-Template (any file with '.hbs' extension) and make sure its `BuildAction` is set to `C# analyzer additional file`
