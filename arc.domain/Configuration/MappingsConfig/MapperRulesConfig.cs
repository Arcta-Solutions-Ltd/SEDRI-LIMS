namespace arc.domain.Configuration.MappingsConfig
{
    public class MapperRulesConfig
    {
        public string Type { get; set; }
        public string Source { get; set; }
        public string Where { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }

        public void ChangeFieldName(string oldName, string newName)
        { 
            if (Source.ToLower() == oldName.ToLower())
            {
                Source = newName;
            }
            if (Value.ToLower() == oldName.ToLower())
            {
                Value = newName;
            }
        }
    }
}
