using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for addfieldgridcolumn. Used with deferSave subform; no backend persistence.
    /// </summary>
    internal class AddFieldGridColumnEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'addfieldgridcolumn',
                        Description: '@ConAddGC@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
