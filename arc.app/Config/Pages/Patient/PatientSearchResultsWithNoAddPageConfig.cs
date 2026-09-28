using arc.app.Common;

namespace arc.app.Config.Pages.Patient
{
    internal class PatientSearchResultsWithNoAddPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'patientsearchresultswithnoaddpage',
                            pageGroup: 'patient',
                            groupAnchor: 2,
                            pageTitle: '@PatPatG@',
                            text: '@PatSelE@.',
                            queryName: 'patientsearch',
                            configureActions: 'nofields',
                            tableName: 'None',
                            crafted: true,
                            nextButton: {
                                          show: false
                                        }
                            }";

            return page;
        }
    }
}

