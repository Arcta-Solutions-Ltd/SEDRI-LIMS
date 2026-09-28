using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteSettingQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'deletesettingquery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
        }
    }
}
