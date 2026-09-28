using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the Add Alert event.
/// </summary>
internal class AddAlertEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Add Alert event.
    /// </summary>
    /// <returns>A JSON string representing the Add Alert event configuration.</returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'addAlert', 
                        Description: '@AleAdd@',
                        EventType : 'specialadddata', 
                        Topic : 'Alert', 
                        TableName: 'Alert',
                        ValidationRules: [
                            { field: 'AlertTypeId', rule: 'required', message: '@AleTypB@'},
                            { field: 'AlertName', rule: 'required', message: '@AleAleA@'},
                            { field: 'AlertMessage', rule: 'required', message: '@AleAleB@'}
                        ]
                    }";
    }
}

