using arc.app.Common;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data;

/// <summary>
/// A command that takes a typed argument and returns a typed result rather than a row count.
/// Used where the caller needs to know what the database actually did, for example which
/// configuration records were inserted, updated or renamed by a designer save.
/// </summary>
/// <typeparam name="TArgument">The type passed into the command.</typeparam>
/// <typeparam name="TResult">The type returned by the command.</typeparam>
public interface ICommandWithTypeReturningType<TArgument, TResult>
{
    /// <summary>
    /// Carries out the command against the database.
    /// </summary>
    /// <param name="connect">The database connection. The command is responsible for opening it if it needs an explicit transaction.</param>
    /// <param name="command">The typed argument for the command.</param>
    /// <param name="logWriter">Log writer class which inherits from ILogWriter.</param>
    /// <returns>The typed result of the command.</returns>
    Task<TResult> ExecuteAsync(NpgsqlConnection connect, TArgument command, ILogWriter logWriter = null);
}
