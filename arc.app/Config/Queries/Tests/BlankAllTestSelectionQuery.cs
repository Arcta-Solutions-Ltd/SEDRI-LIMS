using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BlankAllTestSelectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'blankalltestselection', 'Type': 'Special', 'Translate': true}";
        }
    }
}
