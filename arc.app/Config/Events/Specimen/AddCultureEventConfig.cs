using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides configuration for the 'addCulture' data event, including metadata and display settings.
/// </summary>
internal class AddCultureEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for adding a culture entry as a JSON string.
    /// The definition includes event type, data mappings, table name, and UI display specifications.
    /// </summary>
    /// <returns>A JSON-formatted string defining the culture addition event.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'addCulture',
                        Description: '@SpeAdd@',
                        EventType : 'specialadddata',
                        Topic : 'Culture',
                        TableName: 'Culture',
                        ValidationRules: [
                            { field: 'CultureType', rule: 'required', message: '@CulCul@'}
                        ],
                        Mapping: 'addculturemapper',
                        Display: [
                            { Label: 'culturetype', Translation: '@CulTyp@', List: 'Yes' },
                            { Label: 'organismid', Translation: '@GenOrgA@', List: 'No' },
                            { Label: 'growth', Translation: '@SpeGroC@', List: 'Yes' },
                            { Label: 'quantity', Translation: '@GenQua@', List: 'Yes' },
                            { Label: 'positivedate', Translation: '@SpePosA@', List: 'No', Date: true },
                            { Label: 'positivetime', Translation: '@SpePosB@', List: 'No' },
                            { Label: 'apiidpanel', Translation: '@SpeApi@', List: 'Yes' },
                            { Label: 'idprofile', Translation: '@SpeId@', List: 'No' },
                            { Label: 'IdPercentage', Translation: '@SpeIdA@', List: 'No' },
                            { Label: 'aliquotid', Translation: '@SpeAli@', List: 'No' },
                            { Label: 'comment1', Translation: '@GenCom@', List: 'Yes' },
                            { Label: 'comment2', Translation: '@GenComA@', List: 'Yes' },
                            { Label: 'additionalnotes', Translation: '@GenAdd@', List: 'No' },
                            { Label: 'displayonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
    }
}
