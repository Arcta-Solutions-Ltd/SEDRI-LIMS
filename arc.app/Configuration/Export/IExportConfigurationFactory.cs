namespace arc.app.Configuration.Export
{
    public interface IExportConfigurationFactory
    {
        public IExportConfigurationProcessor GetProcessor(string processorName);
    }
}
