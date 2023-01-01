using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EntityTypeConfigurationGenerator
{
    public static class ClassDeclarationSyntaxExtensions
    {
        public static IList<PropertyDefinition> GetPropertyDefinitions(this ClassDeclarationSyntax syntax, Compilation compilation)
        {
            var result = new List<PropertyDefinition>();
            GetPropertyDefinitions(syntax, compilation, result);
            return result;
        }

        public static void GetPropertyDefinitions(this ClassDeclarationSyntax syntax, Compilation compilation, IList<PropertyDefinition> result)
        {
            GetPropertyDefinitions(syntax, compilation, result, null);
        }

        public static void GetPropertyDefinitions(this ClassDeclarationSyntax syntax, Compilation compilation, IList<PropertyDefinition> result,
            TypeArgumentListSyntax typeArgumentListSyntax)
        {
            ParseBaseTypeSyntax(syntax, compilation, result);

            var members = syntax
                .Members
                .Where(x => x is PropertyDeclarationSyntax)
                .Cast<PropertyDeclarationSyntax>()
                .Where(x => x.Modifiers.Any(y => y.IsKind(SyntaxKind.PublicKeyword)))
                .ToList();

            foreach (var property in members)
            {
                var identifier = property.Identifier.ValueText;
                if (result.Any(x => x.Identifier == identifier))
                {
                    continue;
                }

                var def = new PropertyDefinition
                {
                    Identifier = property.Identifier.ValueText,
                    IsNullable = false
                };

                var leadingTrivia = property.GetLeadingTrivia();
                var trivia = leadingTrivia.FirstOrDefault(x => x.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                if (trivia != default)
                {
                    var xml = trivia.ToFullString().Replace("///", "");
                    var doc = XDocument.Parse(xml);
                    var summary = doc.Root?.Value.Trim();
                    def.Summary = summary;
                }

                switch (property.Type)
                {
                    case PredefinedTypeSyntax predefinedTypeSyntax:
                        def.Type = predefinedTypeSyntax.Keyword.ValueText;
                        break;
                    case NullableTypeSyntax nullableTypeSyntax:
                        def.IsNullable = true;
                        if (nullableTypeSyntax.ElementType is PredefinedTypeSyntax predefinedTypeSyntax2)
                        {
                            def.Type = predefinedTypeSyntax2.Keyword.ValueText;
                        }
                        break;
                }

                result.Add(def);
            }
        }

        public static string GetSummary(this ClassDeclarationSyntax syntax)
        {
            var leadingTrivia = syntax.GetLeadingTrivia();
            var trivia = leadingTrivia.FirstOrDefault(x => x.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
            if (trivia != default)
            {
                var xml = trivia.ToFullString().Replace("///", "");
                var doc = XDocument.Parse(xml);
                var summary = doc.Root?.Value.Trim();
                return summary;
            }

            return null;
        }

        private static void ParseBaseTypeSyntax(ClassDeclarationSyntax syntax, Compilation compilation, IList<PropertyDefinition> result)
        {
            if (syntax.BaseList != null)
            {
                foreach (var baseTypeSyntax in syntax.BaseList.Types)
                {
                    baseTypeSyntax.GetPropertyDefinitions(compilation, result);
                }
            }
        }
    }
}