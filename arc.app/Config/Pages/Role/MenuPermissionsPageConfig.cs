using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class MenuPermissionsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'menupermission',
                            pageTitle: '@RolManB@',
                            text: '@RolConC@.',
                            crafted: true,
                        }";

            return page;
        }
    }
}
