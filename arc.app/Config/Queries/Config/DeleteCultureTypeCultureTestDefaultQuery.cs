using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteCultureTypeCultureTestDefaultQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteCultureTypeCultureTestDefaultQuery', 'Type': 'Config'}";
        }
    }
}
