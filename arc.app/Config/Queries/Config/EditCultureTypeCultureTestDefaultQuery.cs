using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditCultureTypeCultureTestDefaultQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditCultureTypeCultureTestDefaultQuery', 'Type': 'Config'}";
        }
    }
}
