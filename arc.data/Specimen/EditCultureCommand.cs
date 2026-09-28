using arc.app.Common;
using arc.common.Data;
using arc.common.Models.Common;
using arc.common.Utils;
using arc.common;
using arc.data.Configuration;
using arc.common.ExtensionMethods;
using arc.data.Instruments;
using arc.data.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;

namespace arc.data.Specimen;

/// <summary>
/// Handles editing of a culture record and propagates key changes (growth and positivity)
/// to any child cultures linked via <c>ParentCultureId</c>.
/// </summary>
internal class EditCultureCommand
{
    private readonly IOptionsMonitor<DataOptions> _options;
    private ILogWriter _logWriter;
    private readonly IGenerateMoreData _moreDataGenerator;
    private readonly IMoreDataRepository _moreDataRepository;
    private readonly IJsonElementRemover _jsonElementRemover;
    private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;

    /// <summary>
    /// Creates a new instance of <see cref="EditCultureCommand"/>.
    /// </summary>
    public EditCultureCommand(IOptionsMonitor<DataOptions> options, ILogWriter logWriter, IGenerateMoreData moreDataGenerator, IMoreDataRepository moreDataRepository, IJsonElementRemover jsonElementRemover, IInstrumentInterfaceHandler instrumentInterfaceHandler)
    {
        _options = options;
        _logWriter = logWriter;
        _moreDataGenerator = moreDataGenerator;
        _moreDataRepository = moreDataRepository;
        _jsonElementRemover = jsonElementRemover;
        _instrumentInterfaceHandler = instrumentInterfaceHandler;
    }

    /// <summary>
    /// Applies edits to a culture and updates child cultures to keep growth and positivity in sync.
    /// All database work runs on a single enlisted <see cref="NpgsqlConnection"/> inside one <see cref="TransactionScope"/>.
    /// </summary>
    /// <param name="dataToSave">Payload containing culture fields to edit (must include <c>Id</c>).</param>
    /// <param name="command">Event command metadata.</param>
    /// <param name="eventData">Event configuration (dates/fields).</param>
    /// <param name="id">Culture record id being edited.</param>
    /// <returns>0 on success.</returns>
    public async Task<int> ExecuteAsync(string dataToSave, EventModel command, EventConfig eventData, string id)
    {
        _logWriter.LogInfo($"EditCultureCommand ExecuteAsync started for culture id={id}", "EditCultureCommand", "ExecuteAsync");

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        await connect.OpenAsync();

        _logWriter.LogInfo($"EditCultureCommand transaction started on shared connection for culture id={id}", "EditCultureCommand", "ExecuteAsync");

        var genericRepository = new SqlGenericRepository(_options, _logWriter, _moreDataGenerator, _moreDataRepository, _jsonElementRemover, _instrumentInterfaceHandler);
        genericRepository.AddConfiguration("Culture");

        _logWriter.LogInfo("EditCultureCommand calling EditWithoutScopeAsync", "EditCultureCommand", "ExecuteAsync");
        await genericRepository.EditWithoutScopeAsync(dataToSave, id, command, eventData.StringFields, connect);
        _logWriter.LogInfo("EditCultureCommand EditWithoutScopeAsync completed", "EditCultureCommand", "ExecuteAsync");

        var payload = JObject.Parse(dataToSave);
        int cultureId = payload.Value<int>("Id");
        int? growthId = payload.GetNullableInt32("GrowthId");
        var positiveDate = payload.GetNullableDateTime("PositiveDate");
        var positiveTime = payload.GetString("PositiveTime");

        _logWriter.LogInfo($"EditCultureCommand extracted payload: cultureId={cultureId}, growthId={growthId?.ToString() ?? "null"}", "EditCultureCommand", "ExecuteAsync");

        const string selectSql = "select parentCultureId from culture where id = @Id";
        var parentCultureId = await connect.QueryFirstOrDefaultAsync<int?>(selectSql, new { Id = cultureId });

        if (parentCultureId.HasValue)
        {
            var growthTypeParentId = growthId.HasValue ? await connect.GetParentListItemIdAsync(growthId.Value) : (int?)null;
            _logWriter.LogInfo($"EditCultureCommand updating child cultures for parentCultureId={parentCultureId}, growthId={growthId?.ToString() ?? "null"}, growthTypeParentId={growthTypeParentId?.ToString() ?? "null"}", "EditCultureCommand", "ExecuteAsync");
            const string updateSql = @"update culture set growthId = @GrowthId, PositiveDate = @PositiveDate, PositiveTime = @PositiveTime where ParentCultureId = @ParentCultureId";
            var rowsUpdated = await connect.ExecuteAsync(updateSql, new { GrowthId = growthId, PositiveDate = positiveDate, PositiveTime = positiveTime, ParentCultureId = parentCultureId });
            _logWriter.LogInfo($"EditCultureCommand child culture update completed, rowsUpdated={rowsUpdated}", "EditCultureCommand", "ExecuteAsync");
        }
        else
        {
            _logWriter.LogInfo("EditCultureCommand no parent culture, skipping child update", "EditCultureCommand", "ExecuteAsync");
        }

        _logWriter.LogInfo("EditCultureCommand completing transaction scope", "EditCultureCommand", "ExecuteAsync");
        scope.Complete();

        _logWriter.LogInfo($"EditCultureCommand transaction committed successfully for culture id={id}", "EditCultureCommand", "ExecuteAsync");
        return 0;
    }
}
