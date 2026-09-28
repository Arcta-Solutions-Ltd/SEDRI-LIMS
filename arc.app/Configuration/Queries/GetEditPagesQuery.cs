using arc.app.Common;
using arc.common.Models.Config;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Loads the Page Order editor data for a form, including group membership and locked anchors.
    /// </summary>
    internal class GetEditPagesQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetEditPagesQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the form's pages in order, annotated with group id, group title and locked flag.
        /// Rows are matched by page name id.
        /// </summary>
        /// <param name="queryFilters">Filter parameters; requires <c>id</c> identifying the form.</param>
        /// <param name="queryData">Query configuration (unused).</param>
        /// <returns>JSON <see cref="PageOrderModel"/> for the Page Order editor.</returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var formName = PageGroupUtils.ResolveFormName(id);

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            var form = await formConfigDefinition.LoadFormAsync(formName);
            var groups = PageGroupUtils.BuildGroups(form);
            var groupedNames = groups
                .SelectMany(g => g.AllPages)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var rows = (form.PagesConfig ?? Enumerable.Empty<PageConfig>()).Select(p =>
            {
                var inGroup = groupedNames.Contains(p.Name ?? "");
                var definition = inGroup ? PageGroupCatalogue.ById(p.PageGroup) : null;
                return new PageOrderRowModel
                {
                    Id = p.Name,
                    Label = p.PageTitle,
                    GroupId = definition?.Id,
                    GroupTitle = definition?.TitleTag,
                    Locked = inGroup && p.GroupAnchor > 0
                };
            }).ToList();

            logWriter?.LogInfo(
                $"GetEditPagesQuery: form={form.Name}, pageCount={rows.Count}, groups={PageGroupUtils.DescribeLayout(form)}",
                nameof(GetEditPagesQuery),
                nameof(GetAsync));

            var result = new PageOrderModel
            {
                Id = id,
                PageOrder = rows
            };

            return JsonConvert.SerializeObject(result);
        }
    }
}
