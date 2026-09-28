using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Count query used to block admission deletion when specimens are linked.
/// </summary>
internal class DoesAdmissionContainSpecimensCheckQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{
                        Query: 'DoesAdmissionContainSpecimensCheckQuery',
                        TableName: 'Specimen',
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
