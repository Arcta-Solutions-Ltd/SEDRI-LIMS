using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides the event configuration for adding a new user.
/// This configuration defines the metadata for the "adduser" event including its name, description,
/// event type, topic, associated table, and validation rules (e.g. required fields for UserName and Password).
/// </summary>
internal class AddUserEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON string that defines the Add User event configuration.
    /// </summary>
    /// <returns>
    /// A JSON string representing the event configuration with validation rules for UserName and Password.
    /// </returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'adduser', 
                    Description: '@UseAdd@',
                    EventType : 'adddata', 
                    Topic : 'User', 
                    TableName: 'Users',
                    ValidationRules: [
                        { field: 'UserName', rule: 'required', message: '@UseUse@'},
                        { field: 'Password', rule: 'required', message: '@UsePas@'}
                    ]
                }";
    }
}
