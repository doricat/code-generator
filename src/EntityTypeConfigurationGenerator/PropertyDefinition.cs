namespace EntityTypeConfigurationGenerator
{
    public class PropertyDefinition
    {
        public string Identifier { get; set; }

        public string Type { get; set; }

        public bool IsNullable { get; set; }

        public string Summary { get; set; }

        public string GenerateCode()
        {
            return $"builder.Property(x => x.{Identifier}).HasColumnName(\"{Identifier.GetSnakeCase()}\").HasColumnType(\"{GetColumnType()}\"){GenerateLengthDefinition()}{GenerateRequiredDefinition()}{GenerateCommentDefinition()};";
        }

        public string GetColumnType()
        {
            return DefaultTypeMapping.GetType(Type);
        }

        public string GenerateLengthDefinition()
        {
            return Type == "string" || Type == "String" ? ".HasMaxLength(50)" : null;
        }

        public string GenerateRequiredDefinition()
        {
            return $".IsRequired({(IsNullable ? "false" : null)})";
        }

        public string GenerateCommentDefinition()
        {
            return string.IsNullOrEmpty(Summary) ? null: $@".HasComment(""{Summary}"")";
        }
    }
}