using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class BarcodeUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "editpatientbarcodeuievent" => new EditPatientBarcodeUIEventConfig(),
                "editspecimenbarcodeuievent" => new EditSpecimenBarcodeUIEventConfig(),
                _ => null,
            };
        }
    }
}
