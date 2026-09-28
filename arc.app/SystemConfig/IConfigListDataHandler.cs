using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public interface IConfigListDataHandler
    {
        Task<List<OptionsConfig>> GetTestListAsync();
        Task<List<OptionsConfig>> GetReportListAsync();
        Task<List<OptionsConfig>> GetDataSectionListAsync();
        Task<List<OptionsConfig>> GetDirectTestListAsync();
        Task<List<OptionsConfig>> GetCultureTestListAsync();
        Task<List<OptionsConfig>> GetMappingListAsync();
    }
}
