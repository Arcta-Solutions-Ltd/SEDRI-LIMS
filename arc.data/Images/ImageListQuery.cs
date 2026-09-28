using arc.data.model.Image;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Represents a query for retrieving a list of active images.
/// </summary>
internal class ImageListQuery : IQueryReturningType<List<ImageDataModel>>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve a list of active images.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filters to apply (not used in this implementation).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of active images.
    /// </returns>
    public async Task<List<ImageDataModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        const string sql = @"
            SELECT id, name, description, fileattachmentid
            FROM images
            ORDER BY name";

        var result = await connect.QueryAsync<ImageDataModel>(sql);
        return result.ToList();
    }
}
