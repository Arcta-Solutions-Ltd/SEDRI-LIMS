using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Handles state retrieval from the database for workflow management.
/// Provides methods to get the current state of records based on table and field specifications.
/// </summary>
public class StateHandler : IStateHandler
{
    private readonly IStateRepository _stateRepository;

    /// <summary>
    /// Creates a new instance of <see cref="StateHandler"/>.
    /// </summary>
    /// <param name="stateRepository">The repository used to access state data from the database.</param>
    public StateHandler(IStateRepository stateRepository)
    {
        _stateRepository = stateRepository;
    }

    /// <summary>
    /// Retrieves the current state value from the database for the specified record.
    /// </summary>
    /// <param name="id">The unique identifier of the record to query.</param>
    /// <param name="table">The database table name containing the state field.</param>
    /// <param name="field">The field name containing the state value.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the current state value,
    /// or an empty string if the id is null, empty, or "0".
    /// </returns>
    public async Task<string> GetCurrentStateFromDatabaseAsync(string id, string table, string field)
    {
        var currentState = "";
        if (!string.IsNullOrEmpty(id) && id != "0")
        {
            currentState = await _stateRepository.GetStateAsync(id, table, field);
        }
        return currentState;
    }
}
