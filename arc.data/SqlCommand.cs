using arc.app.Common;
using arc.data.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data
{
    /// <summary>
    /// Used to wrap a sql command with a connection and transaction in a common way.
    /// </summary>
    public class SqlCommand(IOptionsMonitor<DataOptions> options, ILogWriter logger) : ISqlCommand
    {
        private readonly IOptionsMonitor<DataOptions> _options = options;
        private readonly ILogWriter _logger = logger;

        /// <summary>
        /// Carries out a command against the database.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">List of string arguments passed to the command</param>
        /// <returns>Does not return anything</returns>
        public async Task CarryOutCommandAsync(ICommand command, string errorMessage, params string[] args)
        {
            await ExecuteCommandAsync(() => command.ExecuteAsync(Connect(), args), errorMessage, nameof(CarryOutCommandAsync));
        }

        /// <summary>
        /// Carries out a command against the database.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">List of string arguments passed to the command</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> CarryOutCommandReturningIntegerAsync(ICommandReturningInteger command, string errorMessage, params string[] args)
        {
            return await ExecuteCommandAsync(() => command.ExecuteAsync(Connect(), args), errorMessage, nameof(CarryOutCommandReturningIntegerAsync));
        }

        /// <summary>
        /// Carries out a command against the database while passing a type into the command.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">Instance of the type passed to the command</param>
        /// <param name="logWriter">Log writer class which inherits from ILogWriter</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> CommandWithTypeQueryAsync<T>(ICommandWithTypeReturningInteger<T> command, string errorMessage, T args, ILogWriter logWriter = null)
        {
            return await ExecuteCommandAsync(() => command.ExecuteAsync(Connect(), args, _logger), errorMessage, nameof(CommandWithTypeQueryAsync));
        }

        /// <summary>
        /// Carries out a command against the database while passing a type and some string arguments into the command.
        /// </summary>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="entity">Instance of the type passed to the command</param>
        /// <param name="args">List of string arguments passed to the command</param>
        /// <param name="logWriter">Log writer class which inherits from ILogWriter</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> CommandWithTypeAndStringParametersAsync<T>(ICommandWithTypeAndStringParameters<T> command, string errorMessage, T entity, params string[] args)
        {
            return await ExecuteCommandAsync(() => command.ExecuteAsync(Connect(), entity, args), errorMessage, nameof(CommandWithTypeAndStringParametersAsync));
        }

        /// <summary>
        /// Carries out a command against the database which returns a typed result rather than a row count.
        /// </summary>
        /// <remarks>
        /// Unlike the other members of this class this overload does not open an ambient
        /// <see cref="TransactionScope"/>. Commands using it open the connection and manage an explicit
        /// <c>NpgsqlTransaction</c> themselves, which Npgsql will not allow inside an enlisted ambient
        /// transaction. The unit of work is therefore stated in the command rather than inferred here.
        /// </remarks>
        /// <param name="command">The method which represents the command to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the command fails</param>
        /// <param name="args">Instance of the type passed to the command</param>
        /// <param name="logWriter">Log writer class which inherits from ILogWriter</param>
        /// <returns>The typed result produced by the command</returns>
        public async Task<TResult> CommandWithTypeReturningTypeAsync<TArgument, TResult>(ICommandWithTypeReturningType<TArgument, TResult> command, string errorMessage, TArgument args, ILogWriter logWriter = null)
        {
            try
            {
                using var connection = Connect();
                return await command.ExecuteAsync(connection, args, logWriter ?? _logger);
            }
            catch (Exception e)
            {
                _logger.LogError($"{errorMessage} sql execution error : {e.Message}", nameof(SqlCommand), nameof(CommandWithTypeReturningTypeAsync));
                throw;
            }
        }

        private async Task<int> ExecuteCommandAsync(Func<Task<int>> execute, string errorMessage, string methodName)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await execute();
                    scope.Complete();
                    return result;
                }
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogError($"{errorMessage} Transaction aborted : {ex.Message}", nameof(SqlCommand), methodName);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"{errorMessage} sql execution error : {e.Message}", nameof(SqlCommand), methodName);
                throw;
            }
        }

        private NpgsqlConnection Connect()
        {
            return new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        }
    }
}

