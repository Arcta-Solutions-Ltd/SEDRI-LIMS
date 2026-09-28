using arc.app.Common;
using arc.common.Models.Instruments;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a command to add an instrument error entry to the database.
/// Implements the <see cref="ICommandWithTypeReturningInteger{T}"/> interface.
/// </summary>
internal class AddInstrumentErrorCommand : ICommandWithTypeReturningInteger<InstrumentErrorModel>
{
    /// <summary>
    /// Executes the command asynchronously to insert an instrument error into the database.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="instrument">The instrument error data to be inserted.</param>
    /// <param name="logWriter">The log writer for logging operations (if applicable).</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added error entry.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, InstrumentErrorModel instrument, ILogWriter logWriter)
    {
        if (instrument.ErrorStatusId == 0)
        {
            instrument.ErrorStatusId = 11;
        }

        var sql = @"insert into instrumenterrors(profilename, errortext, message, instrumentresultid, instrumentdirectionid, errorstatusid, lastmodifieddate)
                        values(@ProfileName, @Description, cast(@Message As Json), @InstrumentResultId, @DirectionId, @ErrorStatusId, now()) returning id";

        return await connect.ExecuteAsync(sql, instrument);
    }
}
