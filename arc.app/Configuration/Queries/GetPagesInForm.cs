using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Forms;
using arc.app.Config.Pages;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Supplies the Define Page Contents screen with the pages of a form, including each page's
    /// columns, form groups and fields.
    /// </summary>
    internal class GetPagesInForm : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetPagesInForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the resolved page definitions for a form.
        /// </summary>
        /// <param name="queryFilters">
        /// Filters whose first parameter value is <c>{viewConfigId}|{formName}</c>, the record id of
        /// the Define Page Contents record view.
        /// </param>
        /// <param name="queryData">Query configuration metadata, unused by this handler.</param>
        /// <returns>
        /// JSON with <c>viewId</c>, <c>form</c>, <c>singleItemName</c> and <c>pages</c>. Each entry in
        /// <c>pages</c> is a full page config, so consumers can read <c>TableName</c> and
        /// <c>ConfigureActions</c> as well as the field lists, plus a <c>canReuseFields</c> flag
        /// saying whether referenced fields may be added to that page. <c>viewId</c> is echoed back
        /// so the client can build the view scoped ids that the add field and add existing field
        /// forms need.
        /// </returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
            var pageAdapter = _serviceProvider.GetService<IPageConfigAdapter>();
            var fieldCatalogue = _serviceProvider.GetService<IExistingFieldCatalogue>();

            var idList = queryFilters.Parameters[0].Value.Split("|");
            var form = await formAdapter.GetFormAsync(idList[1]);

            var pageList = new List<object>();

            foreach (var page in form.Pages)
            {
                var newPage = await pageAdapter.GetPageAsync(page);

                // Serialized rather than projected so every page property still reaches the client
                // as the page configuration grows.
                var pageObject = JObject.FromObject(newPage);
                pageObject["canReuseFields"] = fieldCatalogue != null
                    && await fieldCatalogue.IsReuseAvailableAsync(idList[1], page);

                pageList.Add(pageObject);
            }

            var returnObject = new
            {
                viewId = idList[0],
                form = idList[1],
                singleItemName = form.SingleItemName,
                pages = pageList
            };

            return JsonConvert.SerializeObject(returnObject);
        }
    }
}
