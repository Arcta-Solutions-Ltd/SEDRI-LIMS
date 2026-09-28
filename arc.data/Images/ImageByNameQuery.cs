using arc.data.model.Image;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Represents a query for retrieving an image by name.
/// </summary>
internal class ImageByNameQuery : IQueryReturningType<ImageDataModel>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve an image by name.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filters containing the image name.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the image data model or null if not found.
    /// </returns>
    public async Task<ImageDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var name = queryFilters.GetStringValue("name");
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        const string sql = @"
            SELECT id, name, description, fileattachmentid
            FROM images
            WHERE name = @Name";

        return await connect.QueryFirstOrDefaultAsync<ImageDataModel>(sql, new { Name = name });
    }
}
