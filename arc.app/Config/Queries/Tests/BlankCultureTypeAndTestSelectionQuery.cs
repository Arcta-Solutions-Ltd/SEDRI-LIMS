using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BlankCultureTypeAndTestSelectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'blankculturetypeandtestselection', 'Type': 'Special', 'Translate': true}";
        }
    }
}
