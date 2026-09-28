using arc.app.Common;
using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    internal class GetEditPageQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetEditPageQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Loads page metadata for the Edit Page form, including resolved <c>TableName</c> for save routing.
        /// </summary>
        /// <param name="queryFilters">Filter parameters; requires <c>id</c> as <c>formName|pageName</c>.</param>
        /// <param name="queryData">Query configuration (unused).</param>
        /// <returns>JSON payload for the edit page form.</returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");

            var pageConfigAdapter = _serviceProvider.GetService<IPageConfigAdapter>();

            var pagetoLoad = await pageConfigAdapter.GetPageAsync(idList[1]);
            var fieldsInPage = pagetoLoad.GetFieldList();
            var selectorValues = fieldsInPage.Select(f => new { id = f.Label, label = f.Label, value = f.Id });

            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
            var form = await formAdapter.GetFormAsync(idList[0]);
            var showTableName = FormPageTargetTableExtensions.IsSpecimenRecordForm(form?.SingleItemName);

            var result = new
            {
                Id = pagetoLoad.Name,
                Name = pagetoLoad.PageTitle,
                Description = pagetoLoad.Text,
                ShowTableName = showTableName ? "Yes" : "No",
                TableName = showTableName
                    ? FormPageTargetTableExtensions.ResolvePageTarget(pagetoLoad.TableName)
                    : string.Empty,
                FieldList = selectorValues
            };

            return JsonConvert.SerializeObject(result);
        }
    }
}
