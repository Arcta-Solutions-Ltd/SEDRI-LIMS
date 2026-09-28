using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LaboratoryUserCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'laboratoryusercount', 'TableName': 'LaboratoryUser', 'Type': 'Count',
                        'ParameterMapping': 'laboratoryusercountmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'LaboratoryId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
