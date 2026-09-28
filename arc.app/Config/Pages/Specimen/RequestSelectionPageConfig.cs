using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Crafted page letting the user attach the specimen to one of the existing requests or start a new one.
/// This variant lists every request held for the patient; a form that should only offer the requests of the
/// chosen admission uses <see cref="RequestSelectionForAdmissionPageConfig"/> instead.
/// </summary>
internal class RequestSelectionPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the patient scoped request selection page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'requestselectionpage',
                        pageGroup: 'request',
                        groupAnchor: 1,
                        pageTitle: '@NeoReqSel@',
                        text: '@NeoReqSelA@.',
                        queryName: 'requestsforpatient',
                        requestScope: 'patient',
                        configureActions: 'nofields',
                        tableName: 'None',
                        crafted: true,
                        nextButton: { onclickstate: { state: 'newrequest',
                                                      rules: [{ effect: 'newrequest', field: 'SourceOfClick', rule: '=', value: 'custom' }]
                                                    },
                                      buttontext: '@GenNex@',
                                      show: false
                                    }
                    }";

        return page;
    }
}
