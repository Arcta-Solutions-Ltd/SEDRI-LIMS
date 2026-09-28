using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for the lite isolate test list on a culture (grid/card display fields only).
/// </summary>
internal class TestListForCultureLiteQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                    'Query': 'TestListForCultureLite', 'TableName': 'CultureTests', 'Type': 'Special', 
                    'ParameterMapping': 'testlistforcultureparametermapper',
                    'ResultMapping': 'testlistforcultureliteresultmapper',
                    'Translate': true,
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string'},
                        {'Name': 'CultureId', 'Type': 'string'},
                        {'Name': 'TestName', 'Type': 'string'},
                        {'Name': 'Status', 'Type': 'string'},
                        {'Name': 'Requested', 'Type': 'datetime' },
                        {'Name': 'Completed', 'Type': 'datetime' }
                    ],
                    'Joins': [
                        { 'Table': 'AlertType', 'Type': 'Left', 'On': 'AlertTypeId', 'From': 'Id', 'Fields': [{'Name': 'Colour'}, {'Name': 'AlertCategoryId'}] },
                        { 'Table': 'Culture', 'On': 'CultureId', 'From': 'Id', 'Fields': [{'Name': 'SpecimenId'}] },
                        { 'Table': 'Specimen', 'On': 'ab.SpecimenId', 'From': 'Id', 'Fields': [{'Name': 'StateId'}] }
                    ],
                    'Where' : [
                        {'Field': 'CultureId', 'Comparison': '=' } 
                    ],
                    'orderby': 'Requested'
                }";
    }
}
