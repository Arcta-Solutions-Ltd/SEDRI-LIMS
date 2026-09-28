using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Defines the interface for handling instrument-related operations.
/// Provides methods to create pending results and add results for instruments.
/// </summary>
public interface IInstrumentInterfaceHandler
{
    /// <summary>
    /// Creates pending instrument results for a given culture ID.
    /// </summary>
    /// <param name="specimenId">The ID of the specimen associated with the results.</param>
    /// <param name="cultureId">The ID of the culture to create results for.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateInstrumentPendingResultsForCultureId(int specimenId, int cultureId);

    /// <summary>
    /// Creates pending instrument results for a given specimen ID.
    /// </summary>
    /// <param name="specimenId">The ID of the specimen to create results for.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateInstrumentPendingResultsForSpecimenId(int specimenId);

    /// <summary>
    /// Creates pending instrument results for a direct test identified by its name.
    /// </summary>
    /// <param name="specimenId">The ID of the specimen associated with the results.</param>
    /// <param name="testName">The name of the direct test.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateInstrumentPendingResultsForDirectTestId(int specimenId, string testName);

    /// <summary>
    /// Creates pending instrument results for a culture test identified by its name.
    /// </summary>
    /// <param name="specimenId">The ID of the specimen associated with the results.</param>
    /// <param name="cultureId">The ID of the culture associated with the test.</param>
    /// <param name="testName">The name of the culture test.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateInstrumentPendingResultsForCultureTestId(int specimenId, int cultureId, string testName);

    /// <summary>
    /// Adds instrument results for tests based on the specified topic.
    /// </summary>
    /// <param name="topic">The topic to identify the tests to add results for.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddInstrumentResultsForTests(string topic);

    /// <summary>
    /// Creates pending instrument results when editing a culture.
    /// </summary>
    /// <param name="dataBeingSaved">The data being saved during the edit process.</param>
    /// <param name="connect">The database connection.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateInstrumentPendingResultsWhenEditingACulture(string dataBeingSaved, NpgsqlConnection connect);
}
