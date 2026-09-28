using arc.app.Common;
using arc.data.model.Image;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Images;

/// <summary>
/// Represents a command to update an existing image record.
/// </summary>
internal class UpdateImageCommand : ICommandWithTypeReturningInteger<ImageDataModel>
{
    /// <summary>
    /// Executes the command asynchronously to update an existing image record in the database.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="image">The image data model containing the updated details.</param>
    /// <param name="logWriter">The logger instance used for writing log information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of rows affected.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ImageDataModel image, ILogWriter logWriter = null)
    {
        var sql = @"
            UPDATE images
            SET name = @Name, description = @Description, fileattachmentid = @FileAttachmentId, lastmodifieddate = now()
            WHERE id = @Id";

        return await connect.ExecuteAsync(sql, image);
    }
}
