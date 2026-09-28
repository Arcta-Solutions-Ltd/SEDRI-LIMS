using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for retrieving field options available for rules in a form.
    /// Used when configuring form group rules or grid column rules.
    /// </summary>
    internal class FormFieldOptionsQuery : IDefinition
    {
        /// <summary>
        /// Returns the JSON query configuration for the form field options query.
        /// </summary>
        public string Get()
        {
            return @"{ 'Query': 'FormFieldOptionsQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
