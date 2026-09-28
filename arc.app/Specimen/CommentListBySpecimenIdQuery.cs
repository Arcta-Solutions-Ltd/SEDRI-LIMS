using arc.app.Common;
using arc.app.Configuration;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Executes a query to retrieve a list of comments associated with a specimen ID.
/// </summary>
public class CommentListBySpecimenIdQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentListBySpecimenIdQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">Provides access to application services.</param>
    public CommentListBySpecimenIdQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query to retrieve specimen comments and returns the result as a JSON string.
    /// </summary>
    /// <param name="queryFilter">Filters to apply to the query.</param>
    /// <param name="token">Authentication or session token information.</param>
    /// <returns>A JSON-formatted string containing the specimen comments.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var commentRepository = _serviceProvider.GetService<ICommentRepository>();
        var result = await commentRepository.GetSpecimenCommentListByIdAsync(queryFilter);
        return JsonConvert.SerializeObject(result);
    }
}

