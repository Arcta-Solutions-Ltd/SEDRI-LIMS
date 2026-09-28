using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for the Move to Form Group form.
    /// Returns field info and form group options (all pages) for moving a field across form groups.
    /// </summary>
    internal class MoveFieldQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'MoveFieldQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
