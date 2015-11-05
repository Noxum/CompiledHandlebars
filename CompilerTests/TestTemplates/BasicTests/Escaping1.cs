﻿using System.Linq;
using System.Text;
using System.Net;
using System;

/*11/4/2015 4:50:21 PM | parsing: 0ms; init: 2; codeGeneration: 0!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class Escaping1
  {
    public static string Render(CompiledHandlebars.CompilerTests.HandlebarsJsSpec.FooModel viewModel)
    {
      var sb = new StringBuilder();
      sb.Append("{{Foo}}");
      return sb.ToString();
    }

    private static bool IsTruthy(bool b)
    {
      return b;
    }

    private static bool IsTruthy(string s)
    {
      return !string.IsNullOrEmpty(s);
    }

    private static bool IsTruthy(object o)
    {
      return o != null;
    }

    private class CompiledHandlebarsTemplateAttribute : Attribute
    {
    }
  }
}