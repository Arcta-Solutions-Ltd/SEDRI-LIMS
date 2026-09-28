using arc.app.Common;
using arc.domain.Instruments;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a command to update an instrument result in the database.
/// Implements the <see cref="ICommandWithTypeReturningInteger{T}"/> interface.
/// </summary>
internal class UpdateInstrumentCommand : ICommandWithTypeReturningInteger<InstrumentResult>
{
    /// <summary>
    /// Executes the command asynchronously to delete existing instrument results for a specified culture ID
    /// and inserts a new instrument result into the database.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="instrument">The instrument result data to be inserted or updated.</param>
    /// <param name="logWriter">The log writer for logging operations (if applicable).</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added instrument result.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, InstrumentResult instrument, ILogWriter logWriter)
    {
        var sql = "delete from instrumentresults where cultureid = @cultureid and statusid = 882";
        await connect.ExecuteAsync(sql, instrument);

        var moreData = string.IsNullOrWhiteSpace(instrument.MoreData) ? "{}" : instrument.MoreData;
        sql = @"insert into instrumentresults(instrumentprofile, specimenid, cultureid, barcode, requestmade, lastmodifieddate, statusid, moredata, instrumentmachineid, accessionnumber, culturenumber)
                        values(@InstrumentName, @SpecimenId, @CultureId, @barcode, now(), now(), @statusid, cast(@MoreData as jsonb), @InstrumentMachineId, @AccessionNumber, @CultureNumber) returning id";

        return await connect.QuerySingleAsync<int>(sql, new { instrument.InstrumentName, instrument.SpecimenId, instrument.CultureId, instrument.Barcode, instrument.StatusId, MoreData = moreData, instrument.InstrumentMachineId, instrument.AccessionNumber, instrument.CultureNumber });
    }
}
