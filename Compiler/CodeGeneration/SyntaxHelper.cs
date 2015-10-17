﻿using CompiledHandlebars.Compiler.Introspection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CompiledHandlebars.Compiler.CodeGeneration
{
  /// <summary>
  /// Reocurring neverchanging SyntaxNodes
  /// </summary>
  internal static class SyntaxHelper
  {
    /// <summary>
    /// Yields code that declares and creates a StringBuilder:
    /// var sb = new StringBuilder();
    /// </summary>
    internal static LocalDeclarationStatementSyntax DeclareAndCreateStringBuilder =
      SF.LocalDeclarationStatement(
        SF.VariableDeclaration(
            SF.ParseTypeName("var"), //Only way to get var as it was only introduced in C#4.0 and might break code before that. Nevertheless: THEFUCK?!
            new SeparatedSyntaxList<VariableDeclaratorSyntax>().Add(
              SF.VariableDeclarator(SF.Identifier("sb"), default(BracketedArgumentListSyntax),
              SF.EqualsValueClause(
                SF.ObjectCreationExpression(
                  SF.ParseTypeName("StringBuilder"),
                  SF.ArgumentList(),
                  default(InitializerExpressionSyntax)
                )
              )
            )
          )
        )
      );


    /// <summary>
    /// Yields a return statement for the Stringbuilder.ToString() Method:
    /// return sb.ToString();
    /// </summary>
    internal static ReturnStatementSyntax ReturnSBToString =
      SF.ReturnStatement(
        SF.Token(SyntaxKind.ReturnKeyword),
        SF.InvocationExpression(SF.ParseExpression("sb.ToString")),
        SF.Token(SyntaxKind.SemicolonToken)
      );


    /// <summary>
    /// Yields using Directives:
    /// using System.Text;
    /// </summary>
    internal static UsingDirectiveSyntax[] UsingDirectives =
      new UsingDirectiveSyntax[]
      {
      SF.UsingDirective(SF.ParseName("System.Text")),
      SF.UsingDirective(SF.ParseName("System.Net"))
      };


    /// <summary>
    /// Yields a NamespaceDeclaration:
    /// namespace CompiledHandlebars{}
    /// </summary>
    internal static NamespaceDeclarationSyntax HandlebarsNamespace(string nameSpace, string comment)
    {
      return SF.NamespaceDeclaration(SF.ParseName(nameSpace))
              .WithLeadingTrivia(
                SF.SyntaxTrivia(SyntaxKind.MultiLineCommentTrivia, string.Concat("/*",comment,"*/")));
    }


    /// <summary>
    /// Yields the CompiledHandlebars Class Declaration ClassDeclaration
    /// public static class CompiledHandlebarsTemplate<TViewModel> {}
    /// </summary>
    internal static ClassDeclarationSyntax CompiledHandlebarsClassDeclaration(string templateName)
    {
      return
        SF.ClassDeclaration(
          new SyntaxList<AttributeListSyntax>(),
          SF.TokenList(
            SF.Token(SyntaxKind.PublicKeyword),
            SF.Token(SyntaxKind.StaticKeyword)),
          SF.Identifier(templateName),
          default(TypeParameterListSyntax),
          default(BaseListSyntax),
          default(SyntaxList<TypeParameterConstraintClauseSyntax>),
          default(SyntaxList<MemberDeclarationSyntax>)
        );
    }
    
    /// <summary>
    /// Yields the Render Method with ViewModel Parameter:
    /// public static string Render(TViewModel viewModel){}
    /// </summary>
    internal static MethodDeclarationSyntax RenderWithParameter(string typeName)
    {
      return
        SF.MethodDeclaration(
          new SyntaxList<AttributeListSyntax>(),
          SF.TokenList(
            SF.Token(SyntaxKind.PublicKeyword),
            SF.Token(SyntaxKind.StaticKeyword)),
          SF.PredefinedType(SF.Token(SyntaxKind.StringKeyword)),
          default(ExplicitInterfaceSpecifierSyntax),
          SF.Identifier("Render"),
          default(TypeParameterListSyntax),
          SF.ParameterList(new SeparatedSyntaxList<ParameterSyntax>().Add(SyntaxFactory.Parameter(
            default(SyntaxList<AttributeListSyntax>),
            default(SyntaxTokenList),
            SF.ParseTypeName(typeName),
            SF.Identifier("viewModel"),
            default(EqualsValueClauseSyntax)))
          ),
          default(SyntaxList<TypeParameterConstraintClauseSyntax>),
          SF.Block(),
          default(SyntaxToken)
        );
    }


    internal static ExpressionStatementSyntax AppendStringLiteral(string value)
    {
      return
        SF.ExpressionStatement(
          SF.InvocationExpression(
            SF.ParseExpression("sb.Append")
          ).AddArgumentListArguments(
            SF.Argument(SF.LiteralExpression(SyntaxKind.StringLiteralExpression, SF.Literal(value)))
          )
        );
    }

    internal static ExpressionStatementSyntax AppendMemberEncoded(string memberName)
    {
      return
        SF.ExpressionStatement(
          SF.InvocationExpression(
            SF.ParseExpression("sb.Append")
          )
          .AddArgumentListArguments(
            SF.Argument(
              SF.InvocationExpression(
                SF.ParseExpression("WebUtility.HtmlEncode")
              )
              .AddArgumentListArguments(
                SF.Argument(SF.ParseExpression(memberName))
              )
            )
          )
        );
    }

    /// <summary>
    /// Yields a "sb.append(memberName)" Statement
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns></returns>
    internal static ExpressionStatementSyntax AppendMember(string memberName)
    {
      return
        SF.ExpressionStatement(
          SF.InvocationExpression(
            SF.ParseExpression("sb.Append")
          ).AddArgumentListArguments(
            SF.Argument(SF.ParseExpression(memberName))
          )
        );
    }

    internal static MethodDeclarationSyntax IsTruthyMethodBool()
    {
      return
        SF.MethodDeclaration(
          new SyntaxList<AttributeListSyntax>(),
          SF.TokenList(
            SF.Token(SyntaxKind.PublicKeyword),
            SF.Token(SyntaxKind.StaticKeyword)),
          SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword)),
          default(ExplicitInterfaceSpecifierSyntax),
          SF.Identifier("IsTruthy"),
          default(TypeParameterListSyntax),
          SF.ParameterList(new SeparatedSyntaxList<ParameterSyntax>().Add(SyntaxFactory.Parameter(
            default(SyntaxList<AttributeListSyntax>),
            default(SyntaxTokenList),
            SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword)),
            SF.Identifier("b"),
            default(EqualsValueClauseSyntax)))
          ),
          default(SyntaxList<TypeParameterConstraintClauseSyntax>),
          SF.Block(
            SF.ReturnStatement(SF.ParseExpression("b"))    
          ),
          default(SyntaxToken)
      );
    }

    internal static MethodDeclarationSyntax IsTruthyMethodString()
    {
      return
        SF.MethodDeclaration(
          new SyntaxList<AttributeListSyntax>(),
          SF.TokenList(
            SF.Token(SyntaxKind.PublicKeyword),
            SF.Token(SyntaxKind.StaticKeyword)),
          SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword)),
          default(ExplicitInterfaceSpecifierSyntax),
          SF.Identifier("IsTruthy"),
          default(TypeParameterListSyntax),
          SF.ParameterList(new SeparatedSyntaxList<ParameterSyntax>().Add(SyntaxFactory.Parameter(
            default(SyntaxList<AttributeListSyntax>),
            default(SyntaxTokenList),
            SF.PredefinedType(SF.Token(SyntaxKind.StringKeyword)),
            SF.Identifier("s"),
            default(EqualsValueClauseSyntax)))
          ),
          default(SyntaxList<TypeParameterConstraintClauseSyntax>),
          SF.Block(
            SF.ReturnStatement(SF.ParseExpression("!string.IsNullOrEmpty(s)"))
          ),
          default(SyntaxToken)
      );
    }

    internal static MethodDeclarationSyntax IsTruthyMethodObject()
    {
      return
        SF.MethodDeclaration(
          new SyntaxList<AttributeListSyntax>(),
          SF.TokenList(
            SF.Token(SyntaxKind.PublicKeyword),
            SF.Token(SyntaxKind.StaticKeyword)),
          SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword)),
          default(ExplicitInterfaceSpecifierSyntax),
          SF.Identifier("IsTruthy"),
          default(TypeParameterListSyntax),
          SF.ParameterList(new SeparatedSyntaxList<ParameterSyntax>().Add(SyntaxFactory.Parameter(
            default(SyntaxList<AttributeListSyntax>),
            default(SyntaxTokenList),
            SF.PredefinedType(SF.Token(SyntaxKind.ObjectKeyword)),
            SF.Identifier("o"),
            default(EqualsValueClauseSyntax)))
          ),
          default(SyntaxList<TypeParameterConstraintClauseSyntax>),
          SF.Block(
            SF.ReturnStatement(SF.ParseExpression("o!=null"))
          ),
          default(SyntaxToken)
      );
    }


    internal static IfStatementSyntax IfIsTruthy(Context lastCheckedContext, Context contextToCheck, AST.IfType ifType)
    {
      var condition = CheckContextForTruthy(lastCheckedContext, contextToCheck, ifType);
      if (condition == null)
        return null;
      else return SF.IfStatement(condition, SF.EmptyStatement());
    }


    internal static StatementSyntax AddCommentToStatement(StatementSyntax statement, string comment)
    {
      return
        statement.WithTrailingTrivia(
          SF.Comment(string.Concat("/*",comment,"*/"))
        );
    }

    /// <summary>
    /// Yields a foreach(var loopVariable in loopedVariable) Statement
    /// </summary>
    /// <param name="loopVariable"></param>
    /// <param name="loopedVariable"></param>
    /// <param name="block"></param>
    /// <returns></returns>
    internal static StatementSyntax ForLoop(string loopVariable, string loopedVariable, List<StatementSyntax> block)
    {
      return
        SF.ForEachStatement(
             SF.ParseTypeName("var"),
             loopVariable,
             SF.ParseExpression(loopedVariable),
             SF.Block(block)
          );
    }

    private static ExpressionSyntax CheckContextForTruthy(Context lastCheckedContext, Context contextToCheck, AST.IfType ifType)
    {
      var argumentList = new List<string>();
      var pathToCheck = contextToCheck.FullPath;
      if (lastCheckedContext != null
          && contextToCheck.FullPath.StartsWith(lastCheckedContext.FullPath)
          && lastCheckedContext.FullPath.Contains("."))
      {//The context to check is directly depended from the context checked before
        if (lastCheckedContext.FullPath.Equals(contextToCheck.FullPath))
          return null;
        //Get the unchecked subpath
        pathToCheck = contextToCheck.FullPath.Substring(lastCheckedContext.FullPath.Length + 1);
        //Split it into elements
        var elements = pathToCheck.Split('.');
        for(int i = 1;i<=elements.Length;i++)
        {//then join them back together with the prefix
          argumentList.Add(string.Join(".", lastCheckedContext.FullPath, string.Join(".", elements.Take(i).ToArray())));
        }
      } else
      {//The context to check is independed from the context checked before
        var elements = pathToCheck.Split('.');
        for (int i = 1; i <= elements.Length; i++)
        {
          argumentList.Add(string.Join(".", elements.Take(i).ToArray()));
        }
      }
      var result = ifType == AST.IfType.If ? SF.ParseExpression($"IsTruthy({argumentList[0]})")
                                           : SF.ParseExpression($"!IsTruthy({argumentList[0]})");
      foreach (var element in argumentList.Skip(1))
        result = ifType == AST.IfType.If ? BinaryIfIsTruthyExpression(result, element)
                                      : BinaryUnlessIsTruthyExpression(result, element);
      return result;
    }


    /// <summary>
    /// Yields IsTruthy(a) && IsTruthy(b)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static BinaryExpressionSyntax BinaryIfIsTruthyExpression(ExpressionSyntax a, string b)
    {
      return SF.BinaryExpression(SyntaxKind.LogicalAndExpression,
        a,
        SF.ParseExpression($"IsTruthy({b})")
      );
    }
    /// <summary>
    /// Yields !IsTruthy(a) || !IsTruthy(b)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static BinaryExpressionSyntax BinaryUnlessIsTruthyExpression(ExpressionSyntax a, string b)
    {
      return SF.BinaryExpression(SyntaxKind.LogicalOrExpression,
        a,
        SF.ParseExpression($"!IsTruthy({b})")
      );
    }

  }

}
