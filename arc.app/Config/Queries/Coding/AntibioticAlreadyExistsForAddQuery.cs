using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticAlreadyExistsForAddQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                'Query': 'antibioticbyalreadyexistsforadd',
                'TableName': 'antibiotic',
                'Type': 'count',
                'Fields': [
                        {'Name': 'id', 'Type': 'string' }
                    ],
                'Where' : [
                    {'Field': 'AntibioticName', 'Comparison': 'equals', 'FieldToMatch': 'antibioticname', orGroup: 'nameOrCode'  },
                    {'Field': 'Code', 'Comparison': 'equals', 'FieldToMatch': 'code', orGroup: 'nameOrCode'  }
                ]
            }";
        }
    }
}
