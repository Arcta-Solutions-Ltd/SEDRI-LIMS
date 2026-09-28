using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Defines a contract for handling batch print operations, including current and historical content processing.
/// </summary>
/// 
public interface IBatchPrintHandler
{
    /// <summary>
    /// Processes the specified batch print contents asynchronously using the provided token information.
    /// </summary>
    /// <param name="contents">The content to be printed in the current batch.</param>
    /// <param name="token">Authentication or session token information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a status or response string.</returns>
    Task<string> HandleAsync(string contents, TokenInfoModel token);
}