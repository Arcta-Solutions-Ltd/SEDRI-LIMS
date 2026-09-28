using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.domain.Configuration.ListsConfig;
using System.Linq;
using Newtonsoft.Json;

namespace arc.app.List;

/// <summary>
/// Executes a query to update and retrieve details about a table list. 
/// This class implements the <see cref="IQueryRun"/> interface.
/// </summary>
internal class UpdateTableListQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTableListQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">Provides access to required services like <see cref="IListRepository"/>.</param>
    public UpdateTableListQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query to fetch and update the details of a table list based on the specified query filter.
    /// </summary>
    /// <param name="queryFilter">The configuration for filtering the query.</param>
    /// <param name="token">Optional token information for authorization, if required.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a serialized JSON string of the updated list configuration.
    /// </returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var listRepository = _serviceProvider.GetService<IListRepository>();

        var listDetails = await listRepository.GetListByIdAsync(queryFilter);
        var listContents = await listRepository.GetListValuesByIdAsync(listDetails.Id);

        var returnResult = new ListConfig { Name = listDetails.Name, Options = listContents.ToList() };
        return JsonConvert.SerializeObject(returnResult);
    }
}

