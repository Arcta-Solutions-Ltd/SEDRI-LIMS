using arc.app.Common;

namespace arc.app.Config.Pages.Patient
{
    internal class PatientSearchResultsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'patientsearchresultspage',
                            pageGroup: 'patient',
                            groupAnchor: 2,
                            pageTitle: '@PatPatG@',
                            text: '@PatSelE@.',
                            queryName: 'patientsearch',
                            entryState: 'patientsearch',
                            configureActions: 'nofields',
                            tableName: 'None',
                            crafted: true,
                            nextButton: { enabledState: 'searchentered',
                                          onclickstate: { state: 'newpatient',
                                                          rules:[{ effect: 'newpatient', field: 'SourceOfClick', rule: '=', value: 'custom'}]
                                                        },
                                          buttontext: '@GenNex@',
                                          show: false
                                        }
                            }";

            return page;
        }
    }
}


