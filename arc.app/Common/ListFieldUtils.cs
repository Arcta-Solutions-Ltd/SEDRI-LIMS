using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public class ListFieldUtils : IListFieldUtils
    {
        IFormConfigAdapter _formConfigAdapter;
        IPageConfigAdapter _pageConfigAdapter;

        private List<PageConfig> _pages = new List<PageConfig>();
        public List<FieldConfig> Fields { get; set; }  = new List<FieldConfig>();

        public ListFieldUtils(IFormConfigAdapter formConfigAdapter, IPageConfigAdapter pageConfigAdapter)
        {
            _formConfigAdapter = formConfigAdapter;
            _pageConfigAdapter = pageConfigAdapter;
        }

        public async Task<List<string>> GetListFieldsForCultureTestsAsync(List<CultureTest> cultureTests)
        {
            var individualTests = cultureTests.Select(t => t.TestName).Distinct();
            var listFields = new List<string>();

            foreach(var test in individualTests)
            {
                var form = await _formConfigAdapter.GetFormAsync(test);
                var lists = await GetListFieldsForFormAsync(form);
                var fields = await GetFieldsForForm(form);
                listFields.AddRange(lists.Select(x => x.Id));
                Fields.AddRange(fields);
            }

            return listFields.Distinct().ToList();

        }
        public async Task<List<FieldConfig>> GetListFieldsForFormAsync(FormConfig form)
        {
            var listFields = new List<FieldConfig>();
            foreach (var page in form.Pages)
            {
                var pageConfig = await _pageConfigAdapter.GetPageAsync(page);
                listFields.AddRange(pageConfig.GetFieldList("dropdown"));
                listFields.AddRange(pageConfig.GetFieldList("combobox"));
                listFields.AddRange(pageConfig.GetFieldList("hierarchicalpicker"));
                _pages.Add(pageConfig);
            }

            return listFields;
        }

        private async Task<List<FieldConfig>> GetFieldsForForm(FormConfig form)
        {
            var fields = new List<FieldConfig>();
            foreach (var page in form.Pages)
            {
                var pageConfig = _pages.First(p => p.Name == page);
                if (pageConfig == null)
                {
                    pageConfig = await _pageConfigAdapter.GetPageAsync(page);
                }

                fields.AddRange(pageConfig.GetFieldList());
                _pages.Add(pageConfig);
            }

            return fields;
        }

    }
}
