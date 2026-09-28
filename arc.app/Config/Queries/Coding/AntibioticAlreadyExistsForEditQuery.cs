using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticAlreadyExistsForEditQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                'Query': 'antibioticbyalreadyexistsforedit',
                'TableName': 'antibiotic',
                'Type': 'count',
                'Fields': [
                        {'Name': 'id', 'Type': 'string' }
                    ],
                'Where' : [
                    {'Field': 'Id', 'Comparison': '!=', 'FieldToMatch': 'id' },
                    {'Field': 'AntibioticName', 'Comparison': 'equals', 'FieldToMatch': 'antibioticname', orGroup: 'nameOrCode'  },
                    {'Field': 'Code', 'Comparison': 'equals', 'FieldToMatch': 'code', orGroup: 'nameOrCode'  }
                ]
            }";
        }
    }
}
