﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace CompiledHandlebars.CompilerTests.HandlebarsJsSpec
{
	[CompiledHandlebarsTemplate]
	public static class Zeros1
	{
		public static string Render(CompiledHandlebars.CompilerTests.HandlebarsJsSpec.NumModel1 viewModel)
		{
			var sb = new StringBuilder(64);
			sb.Append("num1: ");
			sb.Append(WebUtility.HtmlEncode(viewModel.Num1.ToString()));
			sb.Append(", num2: ");
			sb.Append(WebUtility.HtmlEncode(viewModel.Num2.ToString()));
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
}/**/