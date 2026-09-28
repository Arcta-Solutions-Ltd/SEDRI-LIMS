using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public interface IFormConfigRepository
    {
        Task<string> GetNextAvailableFieldNameAsync(QueryFilterConfig queryFilters);
        Task<int> AddNewFormAsync(FullFormConfig newForm);
        Task<int> UpdatePageAsync(PageConfig pageToSave);
        Task<int> DeleteFormAsync(FullFormConfig formToDelete);
        Task<int> UpdateFormAsync(FullFormConfig newForm);
    }
}
