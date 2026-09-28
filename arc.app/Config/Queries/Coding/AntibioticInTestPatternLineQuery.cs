using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticInTestPatternLineQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                'Query': 'antibioticintestpatternlinequery', 
                'TableName': 'testpatternline', 
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
