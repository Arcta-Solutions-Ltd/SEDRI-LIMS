using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides the configuration for the Culture Print Selector special event.
/// </summary>
internal class CulturePrintSelectorEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the culture print selector event.
    /// </summary>
    /// <returns>A JSON string defining the culture print selector event configuration.</returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'cultureprintselector', 
                        Description: '@RepCulC@',
                        EventType : 'special', 
                        Topic : 'Specimen', 
                        TableName: 'Culture'
                    }";
    }
}
