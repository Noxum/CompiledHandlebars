﻿using CompiledHandlebars.Compiler.Introspection;
using CompiledHandlebars.Compiler.Visitors;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CompiledHandlebars.Compiler
{
  public static class HbsCompiler
  {
    public static Tuple<string, IEnumerable<HandlebarsException>> Compile(string hbsTemplate, string nameSpace, string name, Workspace workspace)
    {
      var parser = new HbsParser();
      try
      {
        var sw = new Stopwatch();
        sw.Start();
        var template = parser.Parse(hbsTemplate);
        long parseTime = sw.ElapsedMilliseconds;        
        template.Namespace = nameSpace;
        template.Name = name;
        sw.Restart();
        var codeGenerator = new CodeGenerationVisitor(new RoslynIntrospector(workspace), template);
        if (!codeGenerator.ErrorList.Any())
        {
          long initTime = sw.ElapsedMilliseconds;        
          sw.Restart();
          codeGenerator.GenerateCode();
          sw.Stop();
          long generationTime = sw.ElapsedMilliseconds;
          return new Tuple<string, IEnumerable<HandlebarsException>>(
            codeGenerator.CompilationUnit(
              $"{DateTime.Now} | parsing: {parseTime}ms; init: {initTime}; codeGeneration: {generationTime}!"
            ).NormalizeWhitespace(elasticTrivia: true).ToFullString(), codeGenerator.ErrorList);
        }
        return new Tuple<string, IEnumerable<HandlebarsException>>(string.Empty, codeGenerator.ErrorList);
      } catch(HandlebarsSyntaxError syntaxError)
      {
        return new Tuple<string, IEnumerable<HandlebarsException>>($"No result as SyntaxErrors occured: {syntaxError.Message}", new HandlebarsSyntaxError[] { syntaxError });
      }

    }
  }
}
