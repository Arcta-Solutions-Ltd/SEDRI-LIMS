using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for retrieving a single form group by id.
    /// </summary>
    internal class FormGroupQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'FormGroupQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
