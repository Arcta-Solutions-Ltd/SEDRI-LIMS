using arc.app.Common;
using arc.app.Exports;
using arc.common;
using arc.common.Models;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.data.Configuration;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Exports;

/// <summary>
/// Repository class for executing various export-related queries against the ARC database.
/// </summary>
public class ExportRepository : IExportRepository
{
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;
    private readonly IOptionsMonitor<DataOptions> _options;
    private readonly IJsonReplacer _jsonReplacer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportRepository"/> class with required dependencies.
    /// </summary>
    /// <param name="sqlQuery">Query executor for SQL operations.</param>
    /// <param name="logWriter">Logger used to write log entries.</param>
    /// <param name="options">Options monitor providing configuration settings.</param>
    public ExportRepository(ISqlQuery sqlQuery, ILogWriter logWriter, IOptionsMonitor<DataOptions> options, IJsonReplacer jsonReplacer)
    {
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
        _options = options;
        _jsonReplacer = jsonReplacer;
    }

    /// <summary>
    /// Executes the RunExportQuery and returns a list of IDs.
    /// </summary>
    /// <param name="queryFilter">Query filter configuration including token filters.</param>
    /// <param name="token">Authentication token info model.</param>
    /// <returns>Enumerable of <see cref="IdModel"/> results.</returns>
    public async Task<IEnumerable<IdModel>> RunExportAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        queryFilter.AddTokenFilter("specimen", token);
        _logWriter.LogInfo("Run export query", "ExportRepository", nameof(RunExportAsync));
        return await _sqlQuery.QueryReturningTypeAsync(new RunExportQuery(), "Run export", queryFilter);
    }

    /// <summary>
    /// Executes a data export using <see cref="ExportRunQuery"/> and returns CSV-formatted rows.
    /// </summary>
    /// <param name="queryFilter">Query filter configuration.</param>
    /// <param name="token">Authentication token info model.</param>
    /// <param name="fieldConfigs">Field configurations to include in the export.</param>
    /// <returns>List of string rows representing the exported data.</returns>
    public async Task<List<string>> ExportRunAsync(QueryFilterConfig queryFilter, TokenInfoModel token, List<FieldConfig> fieldConfigs)
    {
        queryFilter.AddTokenFilter("specimen", token);
        _logWriter.LogInfo("Run export query", "ExportRepository", nameof(ExportRunAsync));
        var query = new ExportRunQuery();
        var retVal = new List<string>();

        using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
        {
            retVal = await query.ExecuteAsync(connect, queryFilter, fieldConfigs, _jsonReplacer);
        }
        return retVal;
    }

    /// <summary>
    /// Executes the AST export query, retrieving antimicrobial susceptibility test results.
    /// </summary>
    /// <param name="queryFilter">Query filter configuration.</param>
    /// <param name="token">Authentication token info model.</param>
    /// <returns>List of <see cref="WhonetAntibiotic"/> objects.</returns>
    public async Task<List<WhonetAntibiotic>> AstExportAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        queryFilter.AddTokenFilter("specimen", token);
        _logWriter.LogInfo("Ast export query", "ExportRepository", nameof(AstExportAsync));
        var retVal = new List<WhonetAntibiotic>();

        using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
        {
            retVal = await AstExportQuery.ExecuteAsync(connect, queryFilter);
        }
        return retVal;
    }

    /// <summary>
    /// Executes the export query for specimen comments.
    /// </summary>
    /// <param name="queryFilter">Query filter configuration.</param>
    /// <param name="token">Authentication token info model.</param>
    /// <returns>List of <see cref="ExportComment"/> entries.</returns>
    public async Task<List<ExportComment>> ExportCommentAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        queryFilter.AddTokenFilter("SpecimenComment", token);
        _logWriter.LogInfo("Comment export query", "ExportRepository", nameof(ExportCommentAsync));
        var retVal = new List<ExportComment>();

        using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
        {
            retVal = await CommentExportQuery.ExecuteAsync(connect, queryFilter);
        }
        return retVal;
    }
}
