using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AntibioticInBreakpointQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'antibioticinbreakpointquery', 
                'TableName': 'breakpoint', 
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
