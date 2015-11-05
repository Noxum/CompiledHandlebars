﻿using System.Linq;
using System.Text;
using System.Net;
using System;

/*11/4/2015 4:50:19 PM | parsing: 0ms; init: 2; codeGeneration: 0!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class FirstTest4
  {
    public static string Render(CompiledHandlebars.CompilerTests.TestViewModels.MarsModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Plains))
      {
        bool first1 = true;
        foreach (var loopItem0 in viewModel.Plains)
        {
          if (!IsTruthy(first1))
          {
          }
          else
          {
            sb.Append(WebUtility.HtmlEncode(loopItem0.Name));
          }

          first1 = false;
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
}