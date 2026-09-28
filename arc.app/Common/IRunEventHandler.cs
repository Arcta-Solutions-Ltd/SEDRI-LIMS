using arc.common;
using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Defines the contract for running event handlers.
    /// </summary>
    public interface IRunEventHandler
    {
        /// <summary>
        /// Executes the event handling process asynchronously.
        /// </summary>
        /// <param name="contents">The event content as a JSON string.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the validation message.</returns>
        Task<string> RunAsync(string contents, TokenInfoModel token);

        /// <summary>
        /// Gets the result of the event.
        /// </summary>
        public IdModel Result { get; }
    }

}
