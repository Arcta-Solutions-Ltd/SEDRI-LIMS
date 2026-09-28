using arc.app.Common;
using arc.data.model.Image;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Represents a command to add a new image record.
/// </summary>
internal class AddImageCommand : ICommandWithTypeReturningInteger<ImageDataModel>
{
    /// <summary>
    /// Executes the command asynchronously to insert a new image record into the database.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="image">The image data model containing the details to be added.</param>
    /// <param name="logWriter">The logger instance used for writing log information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added image record.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ImageDataModel image, ILogWriter logWriter = null)
    {
        var sql = @"
            INSERT INTO images(name, description, fileattachmentid, lastmodifieddate)
            VALUES(@Name, @Description, @FileAttachmentId, now())
            RETURNING id";

        return await connect.ExecuteScalarAsync<int>(sql, image);
    }
}
