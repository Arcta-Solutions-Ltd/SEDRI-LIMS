using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for retrieving parent-link metadata when adding or editing list-backed fields.
    /// </summary>
    internal class FieldParentLinkQuery : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the field parent link query.
        /// </summary>
        /// <returns>A JSON formatted query definition.</returns>
        public string Get()
        {
            return @"{ 'Query': 'FieldParentLinkQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
