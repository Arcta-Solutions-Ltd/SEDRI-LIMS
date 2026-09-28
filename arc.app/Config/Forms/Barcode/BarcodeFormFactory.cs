using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class BarcodeFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "editpatientbarcodeform" => new EditPatientBarcodeFormConfig(),
                "editspecimenbarcodeform" => new EditSpecimenBarcodeFormConfig(),
                _ => null,
            };
        }
    }
}
