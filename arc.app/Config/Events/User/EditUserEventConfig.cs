using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides the event configuration for editing a user.
/// This configuration defines the metadata for the "edituser" event including its name, description,
/// event type, topic, associated table, and validation rules for required fields.
/// </summary>
internal class EditUserEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON string that defines the Edit User event configuration.
    /// </summary>
    /// <returns>
    /// A JSON string representing the event configuration with validation rules for the UserName
    /// and Password fields.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'edituser', 
                        Description: '@UseEdi@',
                        EventType : 'editdata', 
                        Topic : 'User', 
                        TableName: 'User',
                        ValidationRules: [
                            { field: 'UserName', rule: 'required', message: '@UseUse@'},
                            { field: 'Password', rule: 'required', message: '@UsePas@'}
                        ]
                    }";
    }
}
