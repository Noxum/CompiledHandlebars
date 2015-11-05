﻿using System;
using System.Linq;
using System.Net;
using System.Text;

/*11/4/2015 10:20:02 PM | parsing: 3ms; init: 7; codeGeneration: 3!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class RedundantTruthynessCheckTest1
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Phobos))
      {
        if (IsTruthy(viewModel.Phobos.Name))
        {
          sb.Append(WebUtility.HtmlEncode(viewModel.Phobos.Name));
        }
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