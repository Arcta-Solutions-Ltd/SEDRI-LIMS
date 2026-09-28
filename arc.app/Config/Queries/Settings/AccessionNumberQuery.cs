using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AccessionNumberQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'accessionnumberquery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
        }
    }
}
