using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SelectOrganismPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'selectorganismpage',
                            pageGroup: 'organism',
                            groupAnchor: 2,
                            pageTitle: '@GenSelF@',
                            text: '@CodSel@.',
                            configureActions: 'nofields',
                            crafted: true
                        }";

            return page;
        }
    }
}
