using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditPagesQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditPagesQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
