using arc.app.Common;

namespace arc.app.Config.Queries
{
    public class SingleUserForUserListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'SingleUserForUserList', 'Type': 'Special'}";
        }
    }
}
