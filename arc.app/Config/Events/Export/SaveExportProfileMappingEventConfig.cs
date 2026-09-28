using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    /// <summary>
    /// Event configuration that registers the saveexportprofilemapping event with the
    /// standard event pipeline. The matching IRun handler is wired up in
    /// <see cref="arc.app.Common.SpecialEventFactory"/>.
    /// </summary>
    internal class SaveExportProfileMappingEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the event configuration as a JSON string.
        /// </summary>
        public string Get()
        {
            return @"{
                        EventName: 'saveexportprofilemapping',
                        Description: '@ExpProMapSav@',
                        EventType: 'special',
                        Topic: 'Export',
                        TableName: 'ExportProfileMapping'
                    }";
        }
    }
}
