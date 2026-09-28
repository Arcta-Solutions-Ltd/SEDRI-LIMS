using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Deletes a single expert rule action row by id.
/// </summary>
internal class DeleteExpertRuleActionCommand : ICommand
{
    /// <summary>
    /// Executes the delete for the given action id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="args">Command arguments; first value is the action id.</param>
    /// <returns>Number of rows deleted.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var id = int.Parse(args[0]);
        var sql = @"delete from expertruleaction where Id = @Id";
        return await connect.ExecuteAsync(sql, new { Id = id });
    }
}
