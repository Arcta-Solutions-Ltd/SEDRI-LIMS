using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;
internal class EditExpertRuleUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'editexpertruleuievent',
                        description: 'Edit Expert Rule',
                        type: 'form',
                        action: 'editexpertruleform'
                    }";

        return newEvent;
    }
}
