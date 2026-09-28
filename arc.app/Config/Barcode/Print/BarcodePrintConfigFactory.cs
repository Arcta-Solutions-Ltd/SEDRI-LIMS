using arc.app.Common;
using arc.domain.Configuration.BarcodeConfig;
using System.Collections.Generic;

namespace arc.app.Config.Barcode
{
    public class BarcodePrintConfigFactory : IBarcodePrintConfigFactory
    {
        public BarcodePrintConfig GetBarcodePrintConfig(string name)
        {
            return new List<IDefinitionFactory>()
            {
                new SpecimenBarcodePrintFactory(),
                new PatientBarcodePrintFactory()
            }.GetConfigByName<BarcodePrintConfig>(name);
        }
    }
}
