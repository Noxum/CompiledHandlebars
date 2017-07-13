﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;
using CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Partials.Shared;

namespace CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Partials
{
	[CompiledHandlebarsTemplate]
	public static class PartialsWithSlashPaths1
	{
		public static string Render(CompiledHandlebars.CompilerTests.HandlebarsJsSpec.Partials.AnotherDudeModel viewModel)
		{
			var sb = new StringBuilder(64);
			sb.Append("Dudes: ");
			sb.Append(dude.Render(viewModel));
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