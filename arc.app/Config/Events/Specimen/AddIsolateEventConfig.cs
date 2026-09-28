using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides configuration for the 'addisolateevent' data event, including metadata and display settings.
/// </summary>
internal class AddIsolateEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for adding an isolate entry as a JSON string.
    /// The definition includes event type, data mappings, table name, and UI display specifications.
    /// </summary>
    /// <returns>A JSON-formatted string defining the isolate addition event.</returns>
    public string Get()
    {
        return @"{
                    EventName: 'addisolateevent',
                    Description: '@SpeAddM@',
                    EventType : 'special',
                    Topic : 'Culture',
                    TableName: 'Culture',
                    Mapping: 'addisolatemapper',
                    Display: [
                        { Label: 'culturetype', Translation: '@CulTyp@', List: 'Yes' },
                        { Label: 'organismid', Translation: '@GenOrgA@', List: 'No' },
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
