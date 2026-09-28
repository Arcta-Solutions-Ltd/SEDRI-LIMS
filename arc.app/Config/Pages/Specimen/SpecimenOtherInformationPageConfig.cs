using arc.app.Common;

namespace arc.app.Config.Pages;


/// <summary>
/// Provides the configuration for the Specimen Other Information page on add/edit culture forms.
/// Define Page Contents allows add/edit/delete at page level; all existing fields remain configurable.
/// </summary>
internal class SpecimenOtherInformationPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Specimen Other Information page configuration.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
            name: 'specimenotherinformationpage',
            pageTitle: '@SpeOth@',
            text: '@SpeProG@.',
            configureActions: 'add,edit,delete',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'AliquotID', type: 'singleline', label: '@SpeAli@', required: false, placeholder: '@SpeProH@', Max: 30 }
                            ]
                        }
                    ]
                }
            ]
        }";

        return page;
    }
}


