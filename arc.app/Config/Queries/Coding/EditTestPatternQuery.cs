using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditTestPatternQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'EditTestPattern',
                        'TableName': 'TestPattern',
                        'Type': 'Special'
                    }";
        }
    }
}
