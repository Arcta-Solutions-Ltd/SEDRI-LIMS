using arc.app.Common;
using arc.common.Models.Config;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Supplies the Add Existing Field form with the fields that may be referenced onto a target
    /// page. Delegates the scope and exclusion rules to <see cref="IExistingFieldCatalogue"/>.
    /// </summary>
    internal class GetExistingFieldListQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetExistingFieldListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Builds the reuse candidate list for the target page named in the id parameter.
        /// </summary>
        /// <param name="queryFilters">
        /// Filters containing an <c>id</c> parameter of
        /// <c>{formName}|{pageName}|{columnKey}|{formGroupKey}|{viewConfigId}</c>. Only the form and
        /// page names are required; the column, form group and view id parts are optional.
        /// </param>
        /// <param name="queryData">Query configuration metadata, unused by this handler.</param>
        /// <returns>
        /// JSON matching <see cref="ExistingFieldQueryResultModel"/>. <c>Id</c> is echoed back
        /// verbatim so the save payload keeps the full target context.
        /// </returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var idParam = queryFilters?.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? string.Empty;
            var idList = id.Split("|");

            if (idList.Length < 2 || string.IsNullOrWhiteSpace(idList[0]) || string.IsNullOrWhiteSpace(idList[1]))
            {
                logWriter?.LogError(
                    $"GetExistingFieldListQuery: malformed id '{id}', expected form|page[|column|formGroup|viewId]",
                    nameof(GetExistingFieldListQuery),
                    nameof(GetAsync));

                return JsonConvert.SerializeObject(new ExistingFieldQueryResultModel { Id = id });
            }

            var formName = idList[0];
            var pageName = idList[1];
            var viewConfigId = idList.Length > 4 ? idList[4] : null;

            var catalogue = _serviceProvider.GetService<IExistingFieldCatalogue>();
            var result = await catalogue.BuildAsync(viewConfigId, formName, pageName);
            result.Id = id;

            logWriter?.LogInfo(
                $"GetExistingFieldListQuery: id={id}, options={result.ExistingFieldOptions.Count}, targetTable={result.TargetTable ?? "none"}",
                nameof(GetExistingFieldListQuery),
                nameof(GetAsync));

            return JsonConvert.SerializeObject(result);
        }
    }
}
