using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BlankCultureTestSelectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'blankculturetestselection', 'Type': 'Special', 'Translate': true}";
        }
    }
}
