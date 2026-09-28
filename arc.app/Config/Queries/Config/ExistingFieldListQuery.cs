using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for the Add Existing Field form. Returns the reuse candidates for a target
    /// page as <c>existingFieldOptions</c>.
    /// </summary>
    /// <remarks>
    /// <c>Translate</c> is true so field labels and page titles held as language tags are rendered in
    /// the user's language. Only the display text is translated; option keys are composite ids
    /// (<c>form|page|fieldId</c>) and are unaffected.
    /// </remarks>
    internal class ExistingFieldListQuery : IDefinition
    {
        /// <summary>
        /// Returns the JSON query definition.
        /// </summary>
        /// <returns>A JSON string defining the existing field list query.</returns>
        public string Get()
        {
            return @"{ 'Query': 'ExistingFieldListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
