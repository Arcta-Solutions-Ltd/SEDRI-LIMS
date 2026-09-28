using arc.app.Security;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Utils;

/// <summary>
/// Utility methods for evaluating laboratory report approval requirements.
/// </summary>
internal static class LaboratoryUtils
{
    /// <summary>
    /// Determines asynchronously whether reports for the specified laboratory require approval.
    /// </summary>
    /// <param name="serviceProvider">The dependency injection provider for accessing repositories.</param>
    /// <param name="laboratoryId">The laboratory identifier as a string.</param>
    /// <returns>True if approval is required, otherwise false.</returns>
    public static async Task<bool> DoReportsForThisLaboratoryNeedApprovalAsync(IServiceProvider serviceProvider, string laboratoryId)
    {
        return await DoReportsForThisLaboratoryNeedApprovalAsync(serviceProvider, laboratoryId.ToInt());
    }

    /// <summary>
    /// Determines asynchronously whether reports for the specified laboratory require approval.
    /// </summary>
    /// <param name="serviceProvider">The dependency injection provider for accessing repositories.</param>
    /// <param name="laboratoryId">The laboratory identifier as an integer.</param>
    /// <returns>True if approval is required, otherwise false.</returns>
    public static async Task<bool> DoReportsForThisLaboratoryNeedApprovalAsync(IServiceProvider serviceProvider, int laboratoryId)
    {
        if (laboratoryId == 0) return true;

        var laboratoryRepository = serviceProvider.GetService<ILaboratoryRepository>();
        var labQueryFilter = new QueryFilterConfig().AddInteger("id", laboratoryId);
        var laboratoryRecord = await laboratoryRepository.LaboratoryByIdAsync(labQueryFilter);
        return laboratoryRecord?.ApproveReports == "Yes";
    }
}
