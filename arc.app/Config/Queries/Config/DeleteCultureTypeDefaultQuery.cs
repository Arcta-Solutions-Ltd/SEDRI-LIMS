using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteCultureTypeDefaultQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteCultureTypeDefaultQuery', 'Type': 'Config'}";
        }
    }
}
