using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class UserFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "adduserform" => new AddUserFormConfig(),
                "changepasswordform" => new ChangePasswordFormConfig(),
                "deleteuserform" => new DeleteUserFormConfig(),
                "edituserform" => new EditUserFormConfig(),
                "mypasswordform" => new MyPasswordFormConfig(),
                "preferenceform" => new PreferenceFormConfig(),
                _ => null,
            };
        }
    }
}
