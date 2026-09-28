using arc.app.Common;

namespace arc.app.Config.Pages.Coding
{
    internal class ChangeOrganismSelectorPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'changeorganismselectorpage',
                            pageTitle: '@OrgCha@',
                            text: '@OrgDec@.',
                            crafted: true,
                            nextButton: { onclickstate: { state: 'neworganism',
                                                          rules:[{ effect: 'neworganism', field: 'SourceOfClick', rule: '=', value: 'custom'}]
                                                        },
                                          buttontext: '@GenNex@',
                                          show: false
                                        }
                        }";

            return page;
        }
    }
}
