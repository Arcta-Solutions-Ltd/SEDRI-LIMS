using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditSettingQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'editsettingquery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
        }
    }
}
