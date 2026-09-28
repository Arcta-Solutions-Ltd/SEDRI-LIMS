using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LaboratorySpecimenCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'laboratoryspecimencount', 'TableName': 'LaboratoryUser', 'Type': 'Count',
                        'ParameterMapping': 'laboratoryspecimencountmapper',
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
