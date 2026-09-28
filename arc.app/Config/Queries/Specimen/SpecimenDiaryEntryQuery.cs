using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenDiaryEntryQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenDiaryEntryQuery', 
                        'TableName': 'Queue', 
                        'ParameterMapping': 'specimendiaryparametermapper',
                        'ResultMapping': 'specimendiaryresultmapper',
                        'Type': 'Select', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'Username', 'Type': 'string'},
                            { 'Name': 'Added', 'Type': 'jdate'},
                            { 'Name': 'EventId', 'Type': 'int'}
                        ],
                        'Joins': [
                            { 'Table': 'Specimen', 'Fields': [{'Name':'AccessionNumber'}] }
                        ],
                        'Where' : [
                            {'Field': 'SpecimenId', 'Comparison': '=' },
                            {'Field': 'EventStatusId', 'Comparison': '=' }
                        ],
                        'ListItems': 'State',
                        'Orderby': 'Added',
                        'Descending': true
                    }";
        }
    }
}
