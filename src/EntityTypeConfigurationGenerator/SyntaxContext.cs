using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EntityTypeConfigurationGenerator
{
    public class SyntaxContext
    {
        public SyntaxContext(Document document, SyntaxNode syntax)
        {
            Document = document;
            Syntax = syntax;
        }

        public bool IntendedSyntax => Syntax is ClassDeclarationSyntax;

        public Document Document { get; }

        public SyntaxNode Syntax { get; }
    }
}