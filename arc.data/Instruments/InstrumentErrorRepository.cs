using arc.app.Common;
using arc.app.Instruments;
using arc.common.Models.Instruments;
using System.Threading.Tasks;

namespace arc.data.Instruments
{
    /// <summary>
    /// This class handles the repository operations for instrument errors.
    /// Implements the IInstrumentErrorRepository interface.
    /// </summary>
    public class InstrumentErrorRepository : IInstrumentErrorRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the InstrumentErrorRepository class.
        /// </summary>
        /// <param name="sqlCommand">The ISqlCommand object for executing SQL commands.</param>
        /// <param name="sqlQuery">The ISqlQuery object for executing SQL queries.</param>
        /// <param name="logWriter">The ILogWriter object for logging information.</param>
        public InstrumentErrorRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Adds a new instrument error asynchronously.
        /// </summary>
        /// <param name="dataToSave">The instrument error data to save.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains an integer representing the result of the command.</returns>
        public async Task<int> AddAsync(InstrumentErrorModel dataToSave)
        {
            _logWriter.LogInfo("Run add instrument error command", "InstrumentErrorRepository", "Add");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddInstrumentErrorCommand(), "Add Instrument Error", dataToSave);
        }
    }

}
