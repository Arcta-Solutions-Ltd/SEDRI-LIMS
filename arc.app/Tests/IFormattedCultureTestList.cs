using arc.common.Models;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Tests
{
    public interface IFormattedCultureTestList
    {
        Task<List<KeyValueModel>> GetListAsync(List<CultureTest> cultureTestList, TokenInfoModel token);
        List<KeyValueModel> TranslateHeadings(List<KeyValueModel> listToConvert, Dictionary<string, string> conversions);
    }
}
