using arc.app.Common;
using System.Threading.Tasks;

namespace arc.data
{
    /// <summary>
    /// Interface used for class which wraps a sql command with a connection and transaction in a common way.
    /// </summary>
    public interface ISqlCommand
    {
        /// <summary>
        /// Carries out a command against the database.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">List of string arguments passed to the command</param>
        /// <returns>Does not return anything</returns>
        Task CarryOutCommandAsync(ICommand command, string errorMessage, params string[] args);
        /// <summary>
        /// Carries out a command against the database.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">List of string arguments passed to the command</param>
        /// <returns>The number of rows affected</returns>
        Task<int> CarryOutCommandReturningIntegerAsync(ICommandReturningInteger query, string errorMessage, params string[] args);
        /// <summary>
        /// Carries out a command against the database while passing a type into the command.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">Instance of the type passed to the command</param>
        /// <param name="logWriter">Log writer class which inherits from ILogWriter</param>
        /// <returns>The number of rows affected</returns>
        Task<int> CommandWithTypeQueryAsync<T>(ICommandWithTypeReturningInteger<T> command, string errorMessage, T args, ILogWriter logWriter = null);
        /// <summary>
        /// Carries out a command against the database while passing a type and some string arguments into the command.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="entity">Instance of the type passed to the command</param>
        /// <param name="args">List of string arguments passed to the command</param>
        /// <param name="logWriter">Log writer class which inherits from ILogWriter</param>
        /// <returns>The number of rows affected</returns>
        Task<int> CommandWithTypeAndStringParametersAsync<T>(ICommandWithTypeAndStringParameters<T> command, string errorMessage, T entity, params string[] args);
        /// <summary>
        /// Carries out a command against the database which returns a typed result rather than a row count.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">Instance of the type passed to the command</param>
        /// <param name="logWriter">Log writer class which inherits from ILogWriter</param>
        /// <returns>The typed result produced by the command</returns>
        Task<TResult> CommandWithTypeReturningTypeAsync<TArgument, TResult>(ICommandWithTypeReturningType<TArgument, TResult> command, string errorMessage, TArgument args, ILogWriter logWriter = null);
    }
}
