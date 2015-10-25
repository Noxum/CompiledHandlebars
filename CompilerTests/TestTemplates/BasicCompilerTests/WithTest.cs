﻿using System.Text;
using System.Net;
using System;

/*10/24/2015 9:43:32 PM | parsing: 0ms; init: 2; codeGeneration: 0!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class WithTest
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Phobos))
      {
        sb.Append("Name:");
        sb.Append(WebUtility.HtmlEncode(viewModel.Phobos.Name));
      }

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