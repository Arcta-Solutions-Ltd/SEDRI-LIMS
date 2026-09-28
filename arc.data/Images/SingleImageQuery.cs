using arc.data.model.Image;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Represents a query for retrieving a single image by ID.
/// </summary>
internal class SingleImageQuery : IQueryReturningType<ImageDataModel>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve a single image by ID.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filters containing the image ID.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the image data model or null if not found.
    /// </returns>
    public async Task<ImageDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetStringValue("id");
        if (string.IsNullOrEmpty(id) || !int.TryParse(id, out var imageId))
        {
            return null;
        }

        const string sql = @"
            SELECT id, name, description, fileattachmentid
            FROM images
            WHERE id = @Id";

        return await connect.QueryFirstOrDefaultAsync<ImageDataModel>(sql, new { Id = imageId });
    }
}
