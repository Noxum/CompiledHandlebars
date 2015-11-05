﻿using System;
using System.Linq;
using System.Net;
using System.Text;

/*05.11.2015 10:55:35 | parsing: 0ms; init: 1; codeGeneration: 0!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class SelfReferencingPartialTest1
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.SelfReferencingViewModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Name))
      {
        sb.Append(WebUtility.HtmlEncode(viewModel.Name));
      }

      if (IsTruthy(viewModel) && IsTruthy(viewModel.Child))
      {
        sb.Append(Render(viewModel.Child));
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
}/**/