using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace DotNetCore
{
  [CompiledHandlebarsTemplate]
  public static class Template
  {
    public static string Render(DotNetCore.ViewModel viewModel)
    {
      var sb = new StringBuilder(64);
      sb.Append("\r\nHello ");
      sb.Append(WebUtility.HtmlEncode(viewModel.Name));
      sb.Append("!-");
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

    private static bool IsTruthy<T>(IEnumerable<T> ie)
    {
      return ie != null && ie.Any();
    }

    private static bool IsTruthy(int i)
    {
      return i != 0;
    }

    private class CompiledHandlebarsTemplateAttribute : Attribute
    {
    }

    private class CompiledHandlebarsLayoutAttribute : Attribute
    {
    }
  }
}