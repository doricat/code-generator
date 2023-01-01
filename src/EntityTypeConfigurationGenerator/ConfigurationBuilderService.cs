using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Composition;

namespace EntityTypeConfigurationGenerator
{
    [Export(typeof(IConfigurationBuilderService))]
    public class ConfigurationBuilderService : IConfigurationBuilderService
    {
        public EntityConfiguration Build(SyntaxContext context)
        {
            var semanticModel = ThreadHelper.JoinableTaskFactory.Run(() => context.Document.GetSemanticModelAsync());
            if (semanticModel == null)
            {
                return null;
            }

            if (context.Syntax is ClassDeclarationSyntax syntax)
            {
                var definitions = syntax.GetPropertyDefinitions(semanticModel.Compilation);
                return new EntityConfiguration(syntax.Identifier.Text, syntax.GetSummary(), definitions);
            }

            return null;
        }
    }
}