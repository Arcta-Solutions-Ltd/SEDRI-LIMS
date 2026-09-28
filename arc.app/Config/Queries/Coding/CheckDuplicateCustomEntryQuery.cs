using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CheckDuplicateCustomEntryQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CheckDuplicateCustomEntry',
                        'TableName': 'Additional',
                        'Type': 'Special'
                    }";
        }
    }
}
