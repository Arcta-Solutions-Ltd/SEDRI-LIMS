using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ILanguageHandler
    {
        Task<string> TranslateAsync(string textToTranslate, string languageId);
        Task<string> TranslateJsonAsync(string json, string languageId);
        Task<string> GetFrontEndTagsAsync(string languageId);
        Task<List<ListConfig>> TranslateListAsync(List<ListConfig> listToTranslate, string languageId);
    }
}
