using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ReorderFormGroupsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'reorderformgroupsuievent',
                        description: 'Reorder Form Groups',
                        type: 'form',
                        action: 'reorderformgroupsform'
                    }";

            return newEvent;
        }
    }
}
