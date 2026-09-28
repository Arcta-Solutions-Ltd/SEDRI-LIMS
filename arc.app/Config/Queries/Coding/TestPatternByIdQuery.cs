using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestPatternByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'TestPatternById',
                        'TableName': 'TestPattern',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string' },
                            {'Name': 'TestPatternName', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
