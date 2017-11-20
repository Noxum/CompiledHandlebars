﻿using System;
using CompiledHandlebars.Compiler.AST.Expressions;
using CompiledHandlebars.Compiler.Visitors;
using System.Collections.Generic;

namespace CompiledHandlebars.Compiler.AST
{
	internal class YieldStatement : EncodableHandlebarsLeaf
	{
		internal readonly Expression Expr;
		internal YieldStatement(Expression expr, int line, int column) : base(line, column)
		{
			Expr = expr;
		}

		internal override void Accept(IASTVisitor visitor)
		{
			visitor.Visit(this);
		}

		internal override bool HasExpressionOnLoopLevel<T>()
		{
			return (Expr is T);
		}

		internal HelperCall TransformToHelperCall()
		{
			var helperCall = new HelperCall(
				 Expr.ToString(),
				 new List<Expression>() { new MemberExpression(new ThisIdentifier(null)) },
				 Line,
				 Column);
			helperCall.SetTokenType(Type);
			return helperCall;
		}
	}
}
