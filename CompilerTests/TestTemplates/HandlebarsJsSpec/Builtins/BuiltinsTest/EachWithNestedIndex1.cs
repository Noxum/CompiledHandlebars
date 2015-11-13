﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Builtins
{
  [CompiledHandlebarsTemplate]
  public static class EachWithNestedIndex1
  {
    public static string Render(CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Builtins.TextListModel viewModel)
    {
      var sb = new StringBuilder();
      if (IsTruthy(viewModel) && IsTruthy(viewModel.Goodbyes))
      {
        int index1 = 0;
        foreach (var loopItem0 in viewModel.Goodbyes)
        {
          sb.Append(WebUtility.HtmlEncode(index1.ToString()));
          sb.Append(". ");
          sb.Append(WebUtility.HtmlEncode(loopItem0.Text));
          sb.Append("! ");
          int index2 = 0;
          foreach (var loopItem1 in viewModel.Goodbyes)
          {
            sb.Append(WebUtility.HtmlEncode(index2.ToString()));
            sb.Append(" ");
            index2++;
          }

          sb.Append("After ");
          sb.Append(WebUtility.HtmlEncode(index1.ToString()));
          sb.Append(" ");
          index1++;
        }
      }

      sb.Append("cruel ");
      sb.Append(WebUtility.HtmlEncode(viewModel.World));
      sb.Append("!");
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
  }
}/*Line: 1; Column 192: SpecialExpressions can only exist inside EachBlocks
Line: 1; Column 192: Could not find Helper Method 'CompiledHandlebars.Compiler.AST.Expressions.IndexExpression'*/