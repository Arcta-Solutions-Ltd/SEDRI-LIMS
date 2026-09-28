using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Configuration for the "Laboratory For Laboratory View" query.
/// </summary>
internal class LaboratoryForLaboratoryViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Laboratory For Laboratory View" query.
    /// </summary>
    /// <remarks>
    /// This query is designed to retrieve data for a single laboratory based on its identifier. 
    /// The configuration includes details such as query name, table name, query type, translation settings, 
    /// result mapping, fields to return, and filtering conditions.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata, fields, and where conditions.
    /// </returns>
    public string Get()
    {
        return @"{
                        'Query': 'LaboratoryForLaboratoryViewQuery',
                        'TableName': 'Laboratory',
                        'Type': 'Single',
                        'Translate': true,
                        'ResultMapping': 'laboratoryviewmapper',
                        'Fields': [
                            { 'Name': 'LaboratoryName', 'Type': 'string' }
                        ],
                        'Where': [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
    }
}
