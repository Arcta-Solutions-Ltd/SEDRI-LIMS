using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteDirectTestDefaultQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteDirectTestDefaultQuery', 'Type': 'Config'}";
        }
    }
}
