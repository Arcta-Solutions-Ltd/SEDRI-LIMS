using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Defines the structure of a report list query for retrieving report history data,
/// including selected fields, join conditions, filters, and ordering.
/// </summary>
internal class ReportListQuery : IDefinition
{
    /// <summary>
    /// Returns the query definition in JSON format.
    /// This includes the table to query, selected fields, join to Specimen table,
    /// filter on SpecimenId, and ordering by LastModifiedDate.
    /// </summary>
    /// <returns>A JSON string representing the report list query definition.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'reportlist', 'TableName': 'ReportHistory', 'Type': 'Select', 'ParameterMapping': 'testlistforspecimenparametermapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Name', 'Type': 'string'},
                            {'Name': 'ReportConfig', 'Type': 'string'},
                            {'Name': 'LastModifiedDate', 'Type': 'datetime'}
                        ],
                        'ListItems': 'ReportApproval',
                        'Joins': [
                            { 'Table': 'Specimen', 'Fields': [{'Name': 'AccessionNumber'}] }
                        ],
                        'Where' : [
                            {'Field': 'SpecimenId', 'Comparison': '=' } 
                        ],
                        'OrderBy' : 'LastModifiedDate'
                    }";
    }
}
