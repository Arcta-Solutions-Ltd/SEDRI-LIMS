using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Provides the query definition for retrieving the test list associated with a specimen.
/// </summary>
internal class TestListForSpecimenQuery : IDefinition
{
    /// <summary>
    /// Gets the JSON-formatted query definition for retrieving test details for a specimen.
    /// </summary>
    /// <returns>A string containing the JSON query definition.</returns>
    public string Get()
    {
        return @"{  
                    'Query': 'TestListForSpecimen', 'TableName': 'Tests', 'Type': 'Special',  
                    'ParameterMapping': 'testlistforspecimenparametermapper',
                    'Translate': true,
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string'},
                        {'Name': 'SpecimenId', 'Type': 'string'},
                        {'Name': 'TestName', 'Type': 'string'},
                        {'Name': 'Status', 'Type': 'string'},
                        {'Name': 'Requested', 'Type': 'datetime' },
                        {'Name': 'Completed', 'Type': 'datetime' },
                        {'Name': 'TestResults', 'Type': 'string' }
                    ],
                    'Joins': [
                        { 'Table': 'Specimen', 'On': 'SpecimenId', 'From': 'Id', 'Fields': [{'Name': 'SpecimenTypeId'}] },
                        { 'Table': 'AlertType', 'Type': 'Left', 'On': 'AlertTypeId', 'From': 'Id', 'Fields': [{'Name': 'Colour'}, {'Name': 'AlertCategoryId'}] }
                    ],
                    'Where' : [
                        {'Field': 'SpecimenId', 'Comparison': '=' } 
                    ],
                    'orderby': 'Requested'
                }";
    }
}
