using arc.app.Common;

namespace arc.app.Config.Queries.Export
{
    /// <summary>
    /// Query configuration that loads the saved JSON or XML mapping (or default empty mapping)
    /// for an export profile, plus the classified field options the editor needs to enforce
    /// constraints. Routed through the standard query/filteredget pipeline.
    /// </summary>
    internal class ExportProfileMappingForProfileQueryConfig : IDefinition
    {
        /// <summary>
        /// Returns the query configuration as a JSON string.
        /// </summary>
        public string Get()
        {
            return @"{
                        'Query': 'exportprofilemappingforprofile',
                        'TableName': 'ExportProfileMapping',
                        'Type': 'Special'
                    }";
        }
    }
}
