using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for retrieving the visibility rule and state definition of a single page.
    /// </summary>
    internal class PageRulesQuery : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the page rules query.
        /// </summary>
        /// <returns>A JSON formatted query definition.</returns>
        public string Get()
        {
            return @"{ 'Query': 'PageRulesQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
