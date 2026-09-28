using arc.app.Common;

namespace arc.app.Config.Barcode
{
    internal class SpecimenBarcodePrintFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "specimenbarcodeprint1" => new SpecimenBarcodePrint1Config(),
                "specimenbarcodeprint2" => new SpecimenBarcodePrint2Config(),
                _ => null,
            };
        }
    }
}
