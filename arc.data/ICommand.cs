using Npgsql;
using System.Threading.Tasks;

namespace arc.data;

/// <summary>
/// Contract for a thin data-layer command that executes a single asynchronous operation
/// using an open PostgreSQL connection and positional string arguments resolved by the caller.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Runs the command implementation (typically one SQL statement).
    /// </summary>
    /// <param name="connect">An open <see cref="NpgsqlConnection"/>.</param>
    /// <param name="args">Command-specific positional arguments encoded as strings.</param>
    /// <returns>Affected row metadata or inserted id semantics as defined by the implementation.</returns>
    Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args);
}
