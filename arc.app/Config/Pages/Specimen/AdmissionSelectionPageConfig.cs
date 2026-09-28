using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Crafted page letting the user attach the request to one of the patient's existing admissions or start a
/// new one. Choosing to start a new one raises the <c>newadmission</c> state, which is what makes the
/// admission data entry page visible.
/// </summary>
internal class AdmissionSelectionPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the admission selection page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'admissionselectionpage',
                        pageGroup: 'admission',
                        groupAnchor: 1,
                        pageTitle: '@NeoAdmSel@',
                        text: '@NeoAdmSelA@.',
                        queryName: 'admissionsforpatient',
                        configureActions: 'nofields',
                        tableName: 'None',
                        crafted: true,
                        nextButton: { onclickstate: { state: 'newadmission',
                                                      rules: [{ effect: 'newadmission', field: 'SourceOfClick', rule: '=', value: 'custom' }]
                                                    },
                                      buttontext: '@GenNex@',
                                      show: false
                                    }
                    }";

        return page;
    }
}
