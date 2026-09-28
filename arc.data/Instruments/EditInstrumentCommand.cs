using arc.app.Common;
using arc.domain.Instruments;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a command to edit an existing instrument result in the database.
/// Implements the <see cref="ICommandWithTypeReturningInteger{T}"/> interface.
/// </summary>
internal class EditInstrumentCommand : ICommandWithTypeReturningInteger<InstrumentResult>
{
    /// <summary>
    /// Executes the command asynchronously to update an instrument result in the database.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="instrument">The instrument result data containing the updates.</param>
    /// <param name="logWriter">The log writer for logging operations (if applicable).</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of rows affected by the update.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, InstrumentResult instrument, ILogWriter logWriter)
    {
        var sql = @"update instrumentresults set resultreceived = now(), lastmodifieddate = now(), moredata = Cast(@MoreData As json) where id = @id";

        return await connect.ExecuteAsync(sql, instrument);
    }
}
