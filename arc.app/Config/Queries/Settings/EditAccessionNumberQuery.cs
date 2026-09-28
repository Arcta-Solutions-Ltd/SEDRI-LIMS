using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditAccessionNumberQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'editaccessionnumberquery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
        }
    }
}
