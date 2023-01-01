using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityTypeConfigurationGenerator
{
    public class EntityConfiguration
    {
        private readonly string _name;
        private readonly string _summary;

        public EntityConfiguration(string name, string summary, IList<PropertyDefinition> definitions)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _summary = summary;
            Definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
        }

        public IList<PropertyDefinition> Definitions { get; }

        public string GenerateCode()
        {
            return $@"public class {_name}EntityTypeConfiguration : IEntityTypeConfiguration<{_name}>
{{
    public void Configure(EntityTypeBuilder<{_name}> builder)
    {{
        builder.ToTable(""{_name.GetSnakeCase()}""){GenerateCommentDefinition()};
        // builder.HasKey(x => x.Id);

        {string.Join("\r\n        ", Definitions.Select(x => x.GenerateCode()))}
    }}
}}";
        }

        private string GenerateCommentDefinition()
        {
            return string.IsNullOrEmpty(_summary) ? null : $@".HasComment(""{_summary}"")";
        }
    }
}