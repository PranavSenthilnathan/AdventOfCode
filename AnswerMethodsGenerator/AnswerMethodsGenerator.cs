﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AnswerMethodsGenerator
{
    [Generator]
    public class AnswerMethodsGenerator : IIncrementalGenerator
    {
        enum AnswerMethodArgumentType
        {
            None,
            String,
            StringArray
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(static postInitializationContext =>
            postInitializationContext.AddSource("AnswerMethods.Generated.cs", SourceText.From("""
                namespace AdventOfCode
                {
                    public static partial class AnswerMethods
                    {
                        public static partial System.Func<string, string>[] GetAnswerMethods(int year, int day, int part);
                    }
                }
                """, Encoding.UTF8)));
            var valuesProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
                "AdventOfCode.AnswerMethodAttribute",
                (syntaxNode, token) => syntaxNode is MethodDeclarationSyntax,
                (ctx, token) => 
                {
                    var compilation = ctx.SemanticModel.Compilation;
                    var typeString = compilation.GetTypeByMetadataName("System.String");
                    var typeStringArray = compilation.CreateArrayTypeSymbol(typeString);

                    var targetMethod = ctx.TargetSymbol as IMethodSymbol;
                    var attrs = ctx.Attributes;
                    var targetMethodName = targetMethod.Name;
                    if (targetMethod.Parameters.Length > 1) return default;
                    var argType = AnswerMethodArgumentType.None;
                    if (targetMethod.Parameters.Length == 1)
                    {
                        var assignableFromString = compilation.ClassifyConversion(typeString, targetMethod.Parameters[0].Type);
                        var assignableFromStringArray =
                            compilation.ClassifyConversion(typeStringArray, targetMethod.Parameters[0].Type);

                        if (assignableFromString.IsImplicit)
                            argType = AnswerMethodArgumentType.String;
                        else if (assignableFromStringArray.IsImplicit)
                            argType = AnswerMethodArgumentType.StringArray;
                        else
                            return default;
                    }

                    return new
                    {
                        methodName = $"{targetMethod.ContainingType}.{targetMethod.Name}",
                        argType,
                        attributes = 
                            attrs
                                .Select(attr => attr.ConstructorArguments.Select(arg => (int)arg.Value).ToArray())
                                .Select(attr => (attr[0], attr[1], attr[2]))
                                .ToArray()
                    };
                });

            context.RegisterSourceOutput(
                valuesProvider.Collect(),
                (ctx, infos) =>
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("""
                        namespace AdventOfCode;
                        public static partial class AnswerMethods
                        {
                            public static partial System.Func<string, string>[] GetAnswerMethods(int year, int day, int part)
                            {
                                switch ((year, day, part))
                                {
                        """);

                    //sb.AppendLine("throw new Exception(\"tbd\");");
                    foreach (var grp in infos
                        .Where(x => x != default)
                        .SelectMany(info => info.attributes.Select(attr => (attr, info.methodName, info.argType)))
                        .GroupBy(info => info.attr))
                    {
                        var funcs = string.Join(", ", grp.Select(m => m.argType switch
                        {
                            AnswerMethodArgumentType.None => $"_ => {m.methodName}",
                            AnswerMethodArgumentType.String => $"str => {m.methodName}(str)",
                            AnswerMethodArgumentType.StringArray => @$"str => {m.methodName}(str.Split('\n'))",
                        }));

                        sb.AppendLine(
                            $"""
                                    case {grp.Key}: 
                                        return [{string.Join(", ", funcs)}];
                        """);
                    }

                    sb.AppendLine("""
                                default: return [];
                            }
                        }
                    }
                    """);

                    ctx.AddSource("AnswerMethods.Implementation.Generated.cs", sb.ToString());
                });
        }
    }
}
