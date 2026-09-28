using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for saving user column layout preferences.
    /// </summary>
    internal class SaveColumnLayoutsEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                EventName: 'savecolumnlayoutsevent',
                Description: 'Save Column Layout',
                EventType: 'special',
                Topic: 'User',
                TableName: 'Users',
                DoNotSaveInQueue: true
            }";
        }
    }
}
