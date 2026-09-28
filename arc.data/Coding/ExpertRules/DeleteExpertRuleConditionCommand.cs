using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Deletes a single expert rule condition row by id.
/// </summary>
internal class DeleteExpertRuleConditionCommand : ICommand
{
    /// <summary>
    /// Executes the delete for the given condition id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="args">Command arguments; first value is the condition id.</param>
    /// <returns>Number of rows deleted.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var id = int.Parse(args[0]);
        var sql = @"delete from expertrulecondition where Id = @Id";
        return await connect.ExecuteAsync(sql, new { Id = id });
    }
}
