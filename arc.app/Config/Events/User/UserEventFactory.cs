using arc.app.Common;

namespace arc.app.Config.Events
{
    public class UserEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "adduser" => new AddUserEventConfig(),
                "changepassword" => new ChangePasswordEventConfig(),
                "deleteuser" => new DeleteUserEventConfig(),
                "edituser" => new EditUserEventConfig(),
                "mypassword" => new MyPasswordEventConfig(),
                "preference" => new PreferenceEventConfig(),
                "savefilterpresetsevent" => new SaveFilterPresetsEventConfig(),
                "savecolumnlayoutsevent" => new SaveColumnLayoutsEventConfig(),
                "savehomedashboardevent" => new SaveHomeDashboardEventConfig(),
                _ => null,
            };
        }
    }
}
