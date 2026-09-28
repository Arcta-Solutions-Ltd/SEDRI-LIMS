using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class MonitoringPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "formattedjsonviewer" => new FormattedJsonViewerPageConfig(),
                "jsonviewer" => new JsonViewerPageConfig(),
                _ => null,
            };
        }
    }
}
