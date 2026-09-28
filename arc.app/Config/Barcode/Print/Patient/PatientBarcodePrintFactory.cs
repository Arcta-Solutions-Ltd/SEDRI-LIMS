using arc.app.Common;

namespace arc.app.Config.Barcode
{
    internal class PatientBarcodePrintFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "patientbarcodeprint1" => new PatientBarcodePrint1Config(),
                "patientbarcodeprint2" => new PatientBarcodePrint2Config(),
                _ => null,
            };
        }
    }
}
