using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;
internal class DeleteIsolateCommand : ICommand
{
    /// <summary>
    /// Executes the cascading delete for a isolate by Id.
    /// </summary>
    /// <param name="connect">An open database connection.</param>
    /// <param name="args">Arguments where <c>args[0]</c> is the culture Id.</param>
    /// <returns>The number of rows affected by the final delete on <c>isolate</c>.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var id = int.Parse(args[0]);

        var sql = @"delete from ast where CultureId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from instrumentresults where CultureId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from culturetests where CultureId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from specimencomment where CultureId = @Id";
        await connect.ExecuteAsync(sql, new { Id = id });

        sql = @"delete from culture where Id = @Id";
        return await connect.ExecuteAsync(sql, new { Id = id });
    }
}
