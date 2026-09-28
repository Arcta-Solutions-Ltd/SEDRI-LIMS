using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class UserQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "preference" => new PreferenceQuery(),
                "singleuserforuserlist" => new SingleUserForUserListQuery(),
                "userbyid" => new UserByIdQuery(),
                "userchangepassword" => new UserChangePasswordQuery(),
                "usercount" => new UserCountQuery(),
                "userlist" => new UserListQuery(),
                _ => null,
            };
        }
    }
}
