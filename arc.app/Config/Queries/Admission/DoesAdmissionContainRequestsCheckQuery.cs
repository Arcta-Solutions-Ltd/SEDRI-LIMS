using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Count query used to block admission deletion when requests are linked.
/// </summary>
internal class DoesAdmissionContainRequestsCheckQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{
                        Query: 'DoesAdmissionContainRequestsCheckQuery',
                        TableName: 'Request',
                        Type: 'Count',
                        Fields: [
                            { Name: 'Id', Type: 'int' }
                        ],
                        Where: [
                            { Field: 'Id', Comparison: '=', FieldToMatch: 'AdmissionId' }
                        ]
                    }";
    }
}
