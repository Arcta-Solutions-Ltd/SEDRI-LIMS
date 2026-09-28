using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Count query used to block request deletion when specimens are linked.
/// </summary>
internal class DoesRequestContainSpecimensCheckQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{
                        Query: 'DoesRequestContainSpecimensCheckQuery',
                        TableName: 'Specimen',
                        Type: 'Count',
                        Fields: [
                            { Name: 'Id', Type: 'int' }
                        ],
                        Where: [
                            { Field: 'Id', Comparison: '=', FieldToMatch: 'RequestId' }
                        ]
                    }";
    }
}
