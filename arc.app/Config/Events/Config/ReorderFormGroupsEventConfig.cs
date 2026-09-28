using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ReorderFormGroupsEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'reorderformgroups',
                        Description: '@ConReorderFG@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
