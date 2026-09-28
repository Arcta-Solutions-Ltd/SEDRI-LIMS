using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class RunIqcTestPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'runiqctestpage',
                pageTitle: '@QuaEntB@',
                text: '@QuaEntC@.',
                crafted: true
            }";
        }
    }
}
