using arc.app.Common;

namespace arc.app.Config.Queries.Coding;
internal class CheckDuplicateCustomEntryCodeQuery : IDefinition
{
    public string Get()
    {
        return @"{ 
                        'Query': 'CheckDuplicateCustomEntryCode',
                        'TableName': 'Additional',
                        'Type': 'Special'
                    }";
    }
}
