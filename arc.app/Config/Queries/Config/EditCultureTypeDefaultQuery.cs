using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditCultureTypeDefaultQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditCultureTypeDefaultQuery', 'Type': 'Config'}";
        }
    }
}
