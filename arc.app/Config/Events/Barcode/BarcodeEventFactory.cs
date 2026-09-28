using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class BarcodeEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "editbarcodeprintconfig" => new EditBarcodePrintConfigEventConfig(),
                _ => null,
            };
        }
    }
}
