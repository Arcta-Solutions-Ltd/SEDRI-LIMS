using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class MonitoringFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "monitoringjsonviewer" => new MonitoringJsonViewerFormConfig(),
                "vieweventdetailsjson" => new ViewEventDetailsFormConfig(),
                _ => null,
            };
        }
    }
}
