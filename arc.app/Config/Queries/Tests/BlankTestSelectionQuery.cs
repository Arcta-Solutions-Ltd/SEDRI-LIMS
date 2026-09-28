using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BlankTestSelectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'blanktestselection', 'Type': 'Special', 'Translate': true}";
        }
    }
}
