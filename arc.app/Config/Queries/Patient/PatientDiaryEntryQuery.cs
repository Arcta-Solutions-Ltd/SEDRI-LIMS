using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PatientDiaryEntryQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'PatientDiaryEntryQuery', 
                        'TableName': 'Queue', 
                        'ParameterMapping': 'patientdiaryparametermapper',
                        'ResultMapping': 'patientdiaryresultmapper',
                        'Type': 'Select', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'Username', 'Type': 'string'},
                            { 'Name': 'Added', 'Type': 'datetime'},
                            { 'Name': 'EventId', 'Type': 'int'}
                        ],
                        'Joins': [
                            { 'Table': 'Specimen', 'Fields': [ { 'Name': 'AccessionNumber'}], 'Type':'Left' }
                        ],
                        'Where' : [
                            {'Field': 'PatientId', 'Comparison': '=' },
                            {'Field': 'EventStatusId', 'Comparison': '=' }
                        ],
                        'ListItems': 'State',
                        'Orderby': 'Added',
                        'Descending': true,
                        'Tags': 'PS'
                    }";
        }
    }
}
