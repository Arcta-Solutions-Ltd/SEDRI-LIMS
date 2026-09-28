using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for editfieldgridcolumn. Used with deferSave subform; no backend persistence.
    /// </summary>
    internal class EditFieldGridColumnEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'editfieldgridcolumn',
                        Description: '@ConEdiGC@',
                        EventType: 'special',
                        Topic: 'Configuration'
                    }";
        }
    }
}
