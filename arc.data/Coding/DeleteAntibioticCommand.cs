using arc.app.Common;
using arc.domain.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Command for deleting an antibiotic record.
/// </summary>
internal class DeleteAntibioticCommand : ICommandWithTypeReturningInteger<Antibiotic>
{
    /// <summary>
    /// Executes the delete command for an antibiotic record asynchronously.
    /// </summary>
    /// <param name="connect">The database connection to execute the command.</param>
    /// <param name="command">The antibiotic object containing the data to be deleted.</param>
    /// <param name="logWriter">The log writer for logging actions (not utilized in this implementation).</param>
    /// <returns>The identifier of the deleted antibiotic record as an integer.</returns>
    /// <remarks>
    /// This method interacts with the database via two SQL commands:
    /// 1. Deletes the record from the "AntibioticCoding" table where the identifier matches.
    /// 2. Deletes the associated record from the "Antibiotic" table based on the mapping in "AntibioticCoding."
    /// It ensures the deletion process is consistent and returns the ID of the deleted record.
    /// </remarks>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, Antibiotic command, ILogWriter logWriter)
    {
        var sql = @"delete from AntibioticCoding where Id = @Id";
        await connect.ExecuteAsync(sql, command);

        sql = @"delete from Antibiotic
                        from AntibioticCoding ac 
                        where ac.antibioticid = ant.id and ac.Id = @Id";
        await connect.ExecuteAsync(sql, command);

        return command.Id;
    }
}
