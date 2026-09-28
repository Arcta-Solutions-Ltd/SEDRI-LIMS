using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query definition for the Move Subsection form.
    /// </summary>
    internal class MoveFormGroupQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'MoveFormGroupQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
