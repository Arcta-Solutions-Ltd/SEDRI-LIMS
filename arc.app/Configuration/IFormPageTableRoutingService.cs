using arc.common.Models.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Builds save-time field routing from form page <c>TableName</c> configuration.
/// </summary>
public interface IFormPageTableRoutingService
{
    /// <summary>
    /// Builds the TableExceptions list required by <see cref="arc.common.Data.IGenerateMoreData"/>
    /// from each page's <c>TableName</c> and the arc.data.model column definitions.
    /// </summary>
    /// <param name="formName">Form configuration name from the save payload.</param>
    /// <returns>Field routing entries for non-specimen page targets.</returns>
    Task<IReadOnlyList<TableExceptionModel>> BuildRoutingAsync(string formName);
}
