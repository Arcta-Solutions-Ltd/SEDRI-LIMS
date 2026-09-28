using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ViewRoleRecordUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewrolerecord',
                        description: 'View role record',
                        type: 'view-record',
                        action: 'roles'
                    }";

            return newEvent;
        }
    }
}
