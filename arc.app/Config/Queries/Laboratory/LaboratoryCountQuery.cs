using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LaboratoryCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{    
                        'Query': 'laboratorycount', 'TableName': 'Laboratory', 'Type': 'Count',
                        'ParameterMapping': '',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ]
                    }";
        }
    }
}
