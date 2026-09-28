using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Special query definition used to load a request for edit/delete forms.
/// </summary>
internal class EditRequestQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{
                Query: 'editrequestquery',
                TableName: 'Request',
                Type: 'Special'
            }";
    }
}
