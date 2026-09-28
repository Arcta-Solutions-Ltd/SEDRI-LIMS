using arc.app.Common;

namespace arc.app.Config.Queries.ExpertRules;

/// <summary>
/// Minimal query that returns ExpertRuleId for the add expert rule approval form.
/// Used to populate the form with the parent ExpertRuleId when opened from the embedded list Add button.
/// </summary>
internal class ExpertRuleApprovalFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the expert rule approval form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExpertRuleApprovalFormInitialQuery',
            'TableName': 'expertrule',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int', 'KnownAs': 'ExpertRuleId' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
