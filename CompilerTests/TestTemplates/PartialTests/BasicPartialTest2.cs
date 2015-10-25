﻿using System.Text;
using System.Net;
using System;

/*10/24/2015 9:43:29 PM | parsing: 5ms; init: 16; codeGeneration: 31!*/
namespace TestTemplates
{
  [CompiledHandlebarsTemplate]
  public static class BasicPartialTest2
  {
    public static string Render(System.String viewModel)
    {
      var sb = new StringBuilder();
      sb.Append(BasicPartialTest1.Render(viewModel));
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