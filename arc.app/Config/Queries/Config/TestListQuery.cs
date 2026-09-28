using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'TestList', 'Type': 'Special', 'Translate': true}";
        }
    }
}
