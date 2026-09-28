using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;
internal class DeleteExpertRuleUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteexpertruleuievent',
                        description: 'Delete Expert Rule',
                        type: 'form',
                        action: 'deleteexpertruleform'
                    }";

        return newEvent;
    }
}
