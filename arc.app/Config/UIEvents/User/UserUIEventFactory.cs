using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class UserUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "adduser" => new AddUserUIEventConfig(),
                "changepassworduievent" => new ChangePasswordUIEventConfig(),
                "deleteuseruievent" => new DeleteUserUIEventConfig(),
                "edituser" => new EditUserUIEventConfig(),
                "mypassworduievent" => new MyPasswordUIEventConfig(),
                "preferenceuievent" => new PreferenceUIEventConfig(),
                _ => null,
            };
        }
    }
}
