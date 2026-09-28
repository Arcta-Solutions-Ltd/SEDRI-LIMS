using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Event configuration for saving the user's home dashboard layout.
/// </summary>
internal class SaveHomeDashboardEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                EventName: 'savehomedashboardevent',
                Description: 'Save Home Dashboard',
                EventType: 'special',
                Topic: 'User',
                TableName: 'Users',
                DoNotSaveInQueue: true
            }";
    }
}
