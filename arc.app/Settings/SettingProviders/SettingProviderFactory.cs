using System.ComponentModel;

namespace arc.app.Settings.SettingProviders
{
    public class SettingProviderFactory : ISettingProviderFactory
    {
        public ISettingProvider GetProvider(string providerName)
        {
            return providerName.ToLower() switch
            {
                "number" => new NumberProvider(),
                "text" => new TextProvider(),
                "yearlist" => new YearListProvider(),
                "mapping" => new MappingProvider(),
                _ => null,
            };
        }
    }
}

