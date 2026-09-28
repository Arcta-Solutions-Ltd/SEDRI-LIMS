namespace arc.app.Settings.SettingProviders
{
    public interface ISettingProviderFactory
    {
        ISettingProvider GetProvider(string name);
    }
}
