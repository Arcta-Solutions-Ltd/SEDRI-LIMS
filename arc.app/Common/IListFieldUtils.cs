using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IListFieldUtils
    {
        Task<List<string>> GetListFieldsForCultureTestsAsync(List<CultureTest> cultureTests);
        Task<List<FieldConfig>> GetListFieldsForFormAsync(FormConfig form);
        public List<FieldConfig> Fields { get; set; }
    }
}
