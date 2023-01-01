namespace EntityTypeConfigurationGenerator
{
    public interface IConfigurationBuilderService
    {
        EntityConfiguration Build(SyntaxContext context);
    }
}