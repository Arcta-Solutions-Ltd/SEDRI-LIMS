namespace arc.common.Utils
{
    public interface IJsonMapper
    {
        string FilterJustRequired(string sourceJson, string[] requiredFields);
        string Transform(string sourceJson, string transformRules, bool includeAllFields = false);
    }
}
