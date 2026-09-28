using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Command to soft-delete a list item (marks the item as deleted and disables it).
/// </summary>
internal class DeleteTableEntryCommand : ICommand
{
	/// <summary>
	/// Executes the command to mark a list item as deleted and disabled.
	/// </summary>
	/// <param name="connect">Open Npgsql database connection.</param>
	/// <param name="args">Expected: args[0] = list item Id (as string).</param>
	/// <returns>The number of rows affected.</returns>
	public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
	{
		var id = int.Parse(args[0]);

		var sql = "Update ListItem set Deleted = true, Enabled = false Where Id = @Id and Fixed = false";
		return await connect.ExecuteAsync(sql, new { Id = id });
	}
}
