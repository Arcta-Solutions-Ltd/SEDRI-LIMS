using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AlertTypeUsedInAlertQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'alerttypeusedinalert', 'TableName': 'Alert', 'Type': 'Count',
                        'ParameterMapping': 'alerttypeusedinalertmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'AlertTypeId', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
