using arc.app.Common;

namespace arc.app.Config.Queries
{
    public class UserCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'UserCount', 'TableName': 'Users', 'Type': 'Count'}";
        }
    }
}
