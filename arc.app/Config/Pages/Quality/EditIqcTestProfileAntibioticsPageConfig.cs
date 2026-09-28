using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditIqcTestProfileAntibioticsPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                name: 'editiqctestprofileantibioticspage',
                pageTitle: '@QuaIqcTesProAnt@',
                text: '@QuaIqcTesProAntA@.',
                crafted: true
            }";
        }
    }
}
