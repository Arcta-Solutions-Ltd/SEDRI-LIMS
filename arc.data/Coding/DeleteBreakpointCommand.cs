using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Implements a command to delete a breakpoint and its related records
/// in the resultLine and specimentypebreakpoint tables.
/// </summary>

internal class DeleteBreakpointCommand : ICommand
{
    /// <summary>
    /// Executes the deletion sequence for a breakpoint identified by its Id.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> to the PostgreSQL database.
    /// </param>
    /// <param name="args">
    /// Command arguments:
    /// args[0] – The breakpoint Id to delete (int parseable).
    /// </param>
    /// <returns>
    /// A task returning the number of rows affected by the final delete
    /// operation on the breakpoint table.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var id = int.Parse(args[0]);

        const string deleteResultLineSql = @"DELETE FROM resultLine WHERE BreakpointId = @Id";
        await connect.ExecuteAsync(deleteResultLineSql, new { Id = id });

        const string deleteSpecimenTypeSql = @"DELETE FROM specimentypebreakpoint WHERE BreakpointId = @Id";
        await connect.ExecuteAsync(deleteSpecimenTypeSql, new { Id = id });

        const string deleteBreakpointSql = @"DELETE FROM breakpoint WHERE Id = @Id";
        return await connect.ExecuteAsync(deleteBreakpointSql, new { Id = id });
    }
}
