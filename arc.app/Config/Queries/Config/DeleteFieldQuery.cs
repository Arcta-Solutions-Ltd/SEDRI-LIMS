using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteFieldQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteFieldQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
