using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Barcodes
{
    public interface IBarcodesHandler
    {
        Task<string> GetBarcodeAsync();
        Task<string> GetBarcodeListAsync(string labelType);
        Task<string> GetBarcodeToEditAsync(string type, QueryFilterConfig queryFilters);
        Task EditBarcodePrintConfigAsync(string type, string dataToSave);
    }
}
