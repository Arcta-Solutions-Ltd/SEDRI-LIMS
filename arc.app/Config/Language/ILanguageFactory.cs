using arc.domain.Configuration.LanguageConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Language
{
    public interface ILanguageFactory
    {
        Task<List<LanguageItem>> GetAsync(string languageId);
    }
}
