using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Crafted Organism Identity page on add/edit culture forms. Locked in Define Page Contents
    /// (<c>configureActions: nofields</c>) and the first anchor in the organism page group.
    /// </summary>
    internal class CultureOrganismPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON definition for the culture organism identity page.
        /// </summary>
        /// <returns>A JSON string defining the crafted organism identity page configuration.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'cultureorganismpage',
                            pageTitle: '@SpeOrgA@',
                            text: '@SpeProF@.',
                            pageGroup: 'organism',
                            groupAnchor: 1,
                            configureActions: 'nofields',
                            crafted: true,
                            nextButton: { onclickstate: { state: 'organismsearch',
                                                          rules:[{ effect: 'organismsearch', field: 'SourceOfClick', rule: '=', value: 'custom'}]
                                                        },
                                          buttontext: '@GenNex@',
                                          show: false
                                        },
                            prevButton: { show: false },
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'organismid',  optionsName: 'specimenorganism', Configurable: 'No' },
                                                { id: 'organismcodeId', optionsName: 'specimenorganismcode', Configurable: 'No' }                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}

