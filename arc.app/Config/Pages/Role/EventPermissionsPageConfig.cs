using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EventPermissionsPageConfig : IDefinition
    {
        public  string Get()
        {
            var page = @"{ 
                            name: 'eventpermission',
                            pageTitle: '@RolManA@',
                            text: '@RolConB@.',
                            crafted: true,
                        }";

            return page;
        }
    }
}
