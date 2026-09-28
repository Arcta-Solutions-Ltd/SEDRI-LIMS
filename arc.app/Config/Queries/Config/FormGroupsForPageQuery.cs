using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for retrieving form groups for a page.
    /// </summary>
    internal class FormGroupsForPageQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'FormGroupsForPageQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
