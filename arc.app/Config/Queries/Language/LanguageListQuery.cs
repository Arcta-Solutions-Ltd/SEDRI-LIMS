using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LanguageListQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'languagelist', 'TableName': 'Language', 'Type': 'config'
                    }";
        }
    }
}
