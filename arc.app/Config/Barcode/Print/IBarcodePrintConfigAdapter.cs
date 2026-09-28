using arc.domain.Configuration.BarcodeConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Pages
{
    public interface IBarcodePrintConfigAdapter
    {
        Task<List<BarcodePrintConfig>> GetBarcodeConfigByTypeAsync(string barcodeConfigType);
        Task<BarcodePrintConfig> GetBarcodePrintConfigAsync(string barcodeConfigName);
        Task UpdateBarcodePrintConfigAsync(string dataToSave);
    }
}
