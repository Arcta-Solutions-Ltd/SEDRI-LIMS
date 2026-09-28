using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides configuration metadata for the 'editCulture' event, including mapping,
/// display fields, and associated table and topic information.
/// </summary>
internal class EditCultureEventConfig : IDefinition
{
    /// <summary>
    /// Returns a JSON-formatted string defining the 'editCulture' event,
    /// including its name, description, mapping key, and display field settings.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the 'editCulture' event.
    /// </returns>
    public string Get()
    {
        return @"{
                EventName: 'editCulture',
                Description: '@SpeEdi@',
                EventType : 'special',
                Topic : 'Culture',
                TableName: 'Culture',
                Mapping: 'editculturemapper',
                Display: [
                    { Label: 'organismid', Translation: '@GenOrgA@', List: 'No' },
                    { Label: 'quantity', Translation: '@GenQua@', List: 'Yes' },
                    { Label: 'positivedate', Translation: '@SpePosA@', List: 'No', Date: true },
                    { Label: 'positivetime', Translation: '@SpePosB@', List: 'No' },
                    { Label: 'idpercentage', Translation: '@CulIdeCer@', List: 'No' },
                    { Label: 'aliquotid', Translation: '@SpeAli@', List: 'No' },
                    { Label: 'comment1', Translation: '@GenCom@', List: 'Yes' },
                    { Label: 'comment2', Translation: '@GenComA@', List: 'Yes' },
                    { Label: 'additionalnotes', Translation: '@GenAdd@', List: 'No' },
                    { Label: 'displayonreport', Translation: '@GenDis@', List: 'No' }
                ]
            }";
    }
}


