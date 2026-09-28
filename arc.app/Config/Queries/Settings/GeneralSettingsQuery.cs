using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class GeneralSettingsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'generalsettingsquery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
        }
    }
}
