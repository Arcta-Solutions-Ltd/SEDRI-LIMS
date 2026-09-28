using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for the lite direct test list on a specimen (grid/card display fields only).
/// </summary>
internal class TestListForSpecimenLiteQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                    'Query': 'TestListForSpecimenLite', 'TableName': 'Tests', 'Type': 'Special',  
                    'ParameterMapping': 'testlistforspecimenparametermapper',
                    'ResultMapping': 'testlistforspecimenliteresultmapper',
                    'Translate': true,
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string'},
                        {'Name': 'SpecimenId', 'Type': 'string'},
                        {'Name': 'TestName', 'Type': 'string'},
                        {'Name': 'Status', 'Type': 'string'},
                        {'Name': 'Requested', 'Type': 'datetime' },
                        {'Name': 'Completed', 'Type': 'datetime' }
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
