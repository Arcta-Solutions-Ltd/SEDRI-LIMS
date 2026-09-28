using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticInAstQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                'Query': 'antibioticinastquery', 
                'TableName': 'ast', 
                'Type': 'Count',
                'Fields': [
                    {'Name': 'Id', 'Type': 'int'}
                ],
                'Where' : [
                    {'Field': 'Id', 'Comparison': '=', 'FieldToMatch': 'AntibioticId' }
                ]
            }";
        }
    }
}
