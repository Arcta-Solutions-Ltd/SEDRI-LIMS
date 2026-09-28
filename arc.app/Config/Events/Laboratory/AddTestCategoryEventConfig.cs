using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Test Category" event.
/// </summary>
internal class AddTestCategoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Test Category" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event name, description, type, and associated table name.
    /// It also includes validation rules ensuring required fields, such as TestCategoryId and DirectTestId,
    /// are properly specified during event execution.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata and validation rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'AddTestCategoryEvent',
                        Description: '@LabAddA@',
                        EventType : 'specialadddata',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@LabAB@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAA@'}
                        ]
                    }";
    }
}
