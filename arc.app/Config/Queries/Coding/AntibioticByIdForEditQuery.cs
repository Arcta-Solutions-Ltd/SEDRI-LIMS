using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticByIdForEditQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                'Query': 'antibioticbyidforeditquery',
                'TableName': 'Antibiotic',
                'Type': 'Single',
                'Fields': [
                        {'Name': 'Id', 'Type': 'string' },
                        {'Name': 'AntibioticName', 'Type': 'string' },
                        {'Name': 'Code', 'Type': 'string' },
                        {'Name': 'Atc', 'Type': 'string' },
                        {'Name': 'Cid', 'Type': 'string' },
                        {'Name': 'Loinc', 'Type': 'string' }
                    ],
                'Where' : [
                    {'Field': 'Id', 'Comparison': '=' }
                ]
            }";
        }
    }
}
