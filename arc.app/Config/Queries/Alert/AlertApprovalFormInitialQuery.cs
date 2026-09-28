using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Minimal query that returns AlertId for the add alert approval form.
/// Used to populate the form with the parent AlertId when opened from the embedded list Add button.
/// </summary>
internal class AlertApprovalFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the alert approval form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'AlertApprovalFormInitialQuery',
            'TableName': 'Alert',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int', 'KnownAs': 'AlertId' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
