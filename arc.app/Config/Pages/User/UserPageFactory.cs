using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class UserPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "changepasswordpage" => new ChangePasswordPageConfig(),
                "deleteuserpage" => new DeleteUserPageConfig(),
                "mypasswordpage" => new MyPasswordPageConfig(),
                "preferencepage" => new PreferencePageConfig(),
                "useradddetails" => new AddUserDetailsPageConfig(),
                "usereditdetails" => new EditUserDetailsPageConfig(),
                "userproperties" => new UserPropertiesPageConfig(),
                _ => null,
            };
        }
    }
}
