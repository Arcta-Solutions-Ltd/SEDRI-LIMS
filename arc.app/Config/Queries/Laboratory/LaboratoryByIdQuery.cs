using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Configuration for the "Laboratory By Id" query.
/// </summary>
internal class LaboratoryByIdQuery : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Laboratory By Id" query.
    /// </summary>
    /// <remarks>
    /// This query retrieves data for a specific laboratory based on its identifier. 
    /// The configuration includes details such as the query name, table name, query type, 
    /// fields to return, and filtering conditions.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata, fields, and where conditions.
    /// </returns>
    public string Get()
    {
        return @"{
                        'Query': 'LaboratoryById',
                        'TableName': 'Laboratory',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'LaboratoryName', 'Type': 'string'},
                            {'Name': 'LanguageId', 'Type': 'int'},
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'CodingListId','Type': 'string'},
                            {'Name': 'AntibioticGroupIds','Type': 'string'},
                            {'Name': 'ResistanceMechanismIsolateTestNames','Type': 'string'},
                            {'Name': 'DefaultWorkflowId','Type': 'int'},
                            {'Name': 'ApproveReports','Type': 'string'},
                            {'Name': 'RecordSusceptibilityChangeAudit','Type': 'string'}
                        ],
                        'Where': [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
    }
}
