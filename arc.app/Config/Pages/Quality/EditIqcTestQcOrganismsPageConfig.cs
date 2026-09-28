using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditIqcTestQcOrganismsPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqctestqcorganismspage',
                pageTitle: '@QuaEdiQcOrgFor@',
                text: '@QuaEdiQcOrgDet@.',
                crafted: true
            }";
        }
    }
}
