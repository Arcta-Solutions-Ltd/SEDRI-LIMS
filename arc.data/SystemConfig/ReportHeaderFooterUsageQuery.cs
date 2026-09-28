using arc.common.Models.Reports.ReportDesigner;
using Dapper;
using ReportConfigTypeId = arc.common.Models.Reports.ReportDesigner.ReportConfigTypes;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// Counts how many report records reference a header or footer by name.
/// </summary>
internal class ReportHeaderFooterUsageQuery
{
    /// <summary>
    /// Counts report configs whose Header property matches the supplied name.
    /// </summary>
    /// <param name="connect">An open database connection.</param>
    /// <param name="headerName">The normalised header name to match.</param>
    /// <param name="transaction">Optional transaction for the lookup.</param>
    /// <returns>The number of matching report records.</returns>
    public async Task<int> CountReportsReferencingHeaderAsync(
        NpgsqlConnection connect,
        string headerName,
        IDbTransaction transaction = null)
    {
        return await CountReportsReferencingPropertyAsync(connect, "Header", headerName, transaction);
    }

    /// <summary>
    /// Counts report configs whose Footer property matches the supplied name.
    /// </summary>
    /// <param name="connect">An open database connection.</param>
    /// <param name="footerName">The normalised footer name to match.</param>
    /// <param name="transaction">Optional transaction for the lookup.</param>
    /// <returns>The number of matching report records.</returns>
    public async Task<int> CountReportsReferencingFooterAsync(
        NpgsqlConnection connect,
        string footerName,
        IDbTransaction transaction = null)
    {
        return await CountReportsReferencingPropertyAsync(connect, "Footer", footerName, transaction);
    }

    /// <summary>
    /// Counts report configs whose JSON contents reference a header or footer property value.
    /// </summary>
    /// <param name="connect">An open database connection.</param>
    /// <param name="propertyName">The JSON property to match (<c>Header</c> or <c>Footer</c>).</param>
    /// <param name="sectionName">The normalised section name to match.</param>
    /// <param name="transaction">Optional transaction for the lookup.</param>
    /// <returns>The number of matching report records.</returns>
    private static async Task<int> CountReportsReferencingPropertyAsync(
        NpgsqlConnection connect,
        string propertyName,
        string sectionName,
        IDbTransaction transaction)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            return 0;
        }

        var sql = $@"select count(*) from configs
                     where configtypeid = {ReportConfigTypeId.Report}
                       and lower(contents->>'{propertyName}') = lower(@SectionName)";

        return await connect.QueryFirstAsync<int>(sql, new { SectionName = sectionName.Trim() }, transaction);
    }
}
