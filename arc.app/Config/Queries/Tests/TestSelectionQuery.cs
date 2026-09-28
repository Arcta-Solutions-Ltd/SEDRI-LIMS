using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestSelectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'testselection', 'Type': 'Special', 'Translate': true}";
        }
    }
}
