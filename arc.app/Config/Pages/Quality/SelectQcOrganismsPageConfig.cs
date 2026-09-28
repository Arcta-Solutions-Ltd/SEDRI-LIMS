using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SelectQcOrganismsPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'selectqcorganismspage',
                pageTitle: '@QuaSelQcOrg@',
                text: '@QuaEdiQcOrgDet@.',
                queryName: 'addiqctestquery',
                crafted: true
            }";
        }
    }
}
