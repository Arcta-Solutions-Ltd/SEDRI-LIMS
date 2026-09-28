using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestListForCultureQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'TestListForCulture', 'TableName': 'CultureTests', 'Type': 'Special', 
                        'ParameterMapping': 'testlistforcultureparametermapper',
                        'Translate': true,
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string'},
                            {'Name': 'CultureId', 'Type': 'string'},
                            {'Name': 'TestName', 'Type': 'string'},
                            {'Name': 'Status', 'Type': 'string'},
                            {'Name': 'Requested', 'Type': 'datetime' },
                            {'Name': 'Completed', 'Type': 'datetime' },
                            {'Name': 'TestResults', 'Type': 'string' }
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
}
