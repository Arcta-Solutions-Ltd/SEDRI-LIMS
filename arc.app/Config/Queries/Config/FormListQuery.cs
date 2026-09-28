using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class FormListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'FormList', 'Type': 'Special'}";
        }
    }
}
