using System.Collections.Generic;

namespace arc.domain.StatementBuilder
{
    public class SqlField
    {
        public string Table = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string JsonField { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool MultiSelect { get; set; } = false;
        public string TableName { get; set; } = string.Empty;
        public string JoinTable { get; set; } = string.Empty;
        public string JoinField { get; set; } = string.Empty;
        public string PrefixField { get; set; } = "id";
        public List<string> JoinClauses { get; set; } = null;
        public string FormName {  get; set; } = string.Empty;
        public bool AlternateIfNull { get; set; } = false;
        public bool UseAdditionalForAlternative { get; set; } = false;
        public string AlternateJoinTable { get; set; }
        public string AlternateJoinField { get; set; }
        public string AlternateFieldName { get; set; }
        public string AlternatePrefixField { get; set; } = "id";
        public List<string> AlternativeJoinClauses { get; set; } = null;
        public bool InlcudeAdditionalJoin { get; set; } = false;
        public string AdditionalTableName { get; set; }
        public string AdditionalJoinTable { get; set; }
        public string AdditionalJoinField { get; set; }
        public string AdditionalFieldName { get; set; }
        public string AdditionalPrefixField { get; set; } = "id";
        public List<string> AdditionalJoinClauses { get; set; } = null;
        public string IncludeMICComparison { get; set; } = "No";
        public string FieldConfigId { get; set; } = string.Empty;
    }
}
