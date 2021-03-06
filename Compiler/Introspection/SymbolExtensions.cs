﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiledHandlebars.Compiler.Introspection
{
	public static class SymbolExtensions
	{
		public static ITypeSymbol FindMember(this ISymbol symbol, string name)
		{
			if (symbol == null)
				return null;
			if (symbol.Kind == SymbolKind.NamedType)
				return (symbol as INamedTypeSymbol).FindMemberRec(name);
			if (symbol.Kind == SymbolKind.Property)
				return (symbol as IPropertySymbol).Type?.FindMemberRec(name);
			return null;
		}

		/// <summary>
		/// Is used to get Symbols inside arrays, lists, enumerables etc.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		public static ITypeSymbol GetElementSymbol(this ISymbol symbol)
		{//This is a recursive method

			//If we have a property return the result of its type
			if (symbol.Kind == SymbolKind.Property)
				return (symbol as IPropertySymbol).Type.GetElementSymbol();

			//If we have an array return its ElementType
			if (symbol.Kind == SymbolKind.ArrayType)
				return (symbol as IArrayTypeSymbol).ElementType;

			//If we have a NamedType 
			if (symbol.Kind == SymbolKind.NamedType && (symbol as INamedTypeSymbol).IsGenericType)
			{
				//inheritedList.Add(new PageModel() { Title = "A", Headline = "B" });
				if (symbol.MetadataName.Equals("IEnumerable`1"))
					return (symbol as INamedTypeSymbol).TypeArguments.Single();
				//Check if it implements an IEnumerable interface
				var iEnumerableElement = (symbol as INamedTypeSymbol).AllInterfaces.Single(x => x.MetadataName.Equals("IEnumerable`1"))?.TypeArguments.Single();
				if (iEnumerableElement != null)
					return iEnumerableElement;
			}
			if (symbol.Kind == SymbolKind.NamedType)
				//Otherwise check its base type if exists
				return (symbol as INamedTypeSymbol).BaseType?.GetElementSymbol();
			return null;
		}

		/// <summary>
		/// Is Type under the symbol a string
		/// </summary>
		/// <param name="symbol"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool IsString(this ITypeSymbol symbol)
        {
			return (symbol as INamedTypeSymbol).SpecialType.HasFlag(SpecialType.System_String);
		}

        public static bool IsTaskOfString(this ITypeSymbol symbol)
        {
            return symbol != null && symbol.ToDisplayString().Equals(@"System.Threading.Tasks.Task<string>", StringComparison.Ordinal);
        }

        private static ITypeSymbol FindMemberRec(this ITypeSymbol symbol, string name)
		{
			var result = symbol.GetMembers(name).FirstOrDefault();
			if (result == null && symbol.BaseType != null)
				//Ask base type
				return symbol.BaseType.FindMemberRec(name);

			foreach (var iface in symbol.AllInterfaces)
			{
				var ifaceResult = iface.GetMembers(name).FirstOrDefault();
				if (ifaceResult != null)
				{
					result = ifaceResult;
					break;
				}
			}

			if (!(result is ITypeSymbol))
			{
				if (result is IPropertySymbol)
					return (result as IPropertySymbol).Type;
			}
			else
				return (result as ITypeSymbol);
			return null;
		}
	}
}
