using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AddCultureQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'AddCultureQuery',
                        'TableName': 'Specimen',
                        'Type': 'Single',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'SpecimenTypeId', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";

        }
    }
}
