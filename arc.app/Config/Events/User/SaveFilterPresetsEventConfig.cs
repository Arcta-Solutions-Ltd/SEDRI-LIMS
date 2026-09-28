using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for saving user filter presets.
    /// </summary>
    internal class SaveFilterPresetsEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                EventName: 'savefilterpresetsevent',
                Description: 'Save Filter Presets',
                EventType: 'special',
                Topic: 'User',
                TableName: 'Users',
                DoNotSaveInQueue: true
            }";
        }
    }
}
