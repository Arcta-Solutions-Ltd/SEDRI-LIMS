using arc.common.Models.Language;
using arc.domain.Configuration.LanguageConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface ILanguageRepository
    {
        Task<List<LanguageItem>> GetLanguageAsync(QueryFilterConfig parameters);
        Task<int> AddLanguageAsync(LanguageModel dataToSave);
        Task<int> DeleteLanguageAsync(int translationId);
        Task<LanguageModel> GetLanguageByTranslationIdAsync(QueryFilterConfig parameters);
        Task<int> UpdateLanguagePackAsync(LanguageModel dataToSave);
        Task<IEnumerable<AllLanguageModel>> AllLanguagesAsync();
    }
}
