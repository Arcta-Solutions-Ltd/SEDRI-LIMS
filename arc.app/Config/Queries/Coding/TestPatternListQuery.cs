using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestPatternListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'TestPatternList',
                        'TableName': 'TestPattern',
                        'Type': 'Special'
                    }";
        }
    }
}
