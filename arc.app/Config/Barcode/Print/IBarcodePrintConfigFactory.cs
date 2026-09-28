using arc.domain.Configuration.BarcodeConfig;

namespace arc.app.Config.Barcode
{
    public interface IBarcodePrintConfigFactory
    {
        BarcodePrintConfig GetBarcodePrintConfig(string barcodePrintConfigName);
    }
}
