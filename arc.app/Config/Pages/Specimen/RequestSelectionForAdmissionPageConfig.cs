using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Crafted page letting the user attach the specimen to one of the requests already raised against the
/// chosen admission, or start a new one. A form picks this page rather than
/// <see cref="RequestSelectionPageConfig"/> when request selection should be scoped to the admission.
/// </summary>
internal class RequestSelectionForAdmissionPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the admission scoped request selection page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'requestselectionforadmissionpage',
                        pageGroup: 'request',
                        groupAnchor: 1,
                        pageTitle: '@NeoReqSel@',
                        text: '@NeoReqSelA@.',
                        queryName: 'requestsforadmission',
                        requestScope: 'admission',
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
