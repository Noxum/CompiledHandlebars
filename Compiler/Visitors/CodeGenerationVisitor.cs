﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompiledHandlebars.Compiler.AST;
using CompiledHandlebars.Compiler.Introspection;
using CompiledHandlebars.Compiler.CodeGeneration;

namespace CompiledHandlebars.Compiler.Visitors
{
  internal class CodeGenerationVisitor : IASTVisitor
  {
    private CompilationState state { get; set; }
    public CodeGenerationVisitor(RoslynIntrospector introspector, HandlebarsTemplate template)
    {
      state = new CompilationState(introspector, template);
      state.Introspector = introspector;
    }

    public void Visit(MarkupLiteral astLeaf)
    {
      state.SetCursor(astLeaf);
      state.PushStatement(SyntaxHelper.AppendStringLiteral(astLeaf.Value));
    }

    public void Visit(YieldStatement astLeaf)
    {
      state.SetCursor(astLeaf);
      if (astLeaf._type == TokenType.Encoded)
        state.PushStatement(SyntaxHelper.AppendMemberEncoded(astLeaf.Member.EvaluateToString(state)));
      else
        state.PushStatement(SyntaxHelper.AppendMember(astLeaf.Member.EvaluateToString(state)));
    }

    public void VisitEnter(HandlebarsTemplate template)
    {
      state.PushStatement(SyntaxHelper.DeclareAndCreateStringBuilder);
    }

    public void VisitLeave(HandlebarsTemplate template)
    {
      state.PushStatement(SyntaxHelper.ReturnSBToString);
    }

    internal CompilationState GenerateCode()
    {
      state.Template.Accept(this);
      return state;
    }

    public void VisitEnter(WithBlock astNode)
    {
      state.SetCursor(astNode);
      //TODO: if(IsTruthy(Member))
      state.StartBlock();
      state.ContextStack.Push(astNode.Member.EvaluateToContext(state));
    }

    public void VisitLeave(WithBlock astNode)
    {
      state.ContextStack.Pop();
      state.PushStatement(SyntaxHelper.IfIsTruthy(astNode.Member.EvaluateToString(state), state.EndBlock()));
    }
  }
}
