using arc.app.Common;
using arc.app.Configuration;
using arc.common;
using arc.common.Data;
using arc.common.Models;
using arc.common.Utils;
using arc.data.Utils;
using arc.domain.Configuration.EventsConfig;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Updates specimen and related entity MoreData when editing a specimen form with multi-table pages.
/// </summary>
internal class EditSpecimenMultiTableCommand
{
    private readonly IGenerateMoreData _moreDataGenerator;
    private readonly IMoreDataRepository _moreDataRepository;
    private readonly IJsonUtils _jsonUtils;
    private readonly ILogWriter _logWriter;
    private readonly EditSpecimenCommand _editSpecimenCommand;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSpecimenMultiTableCommand"/> class.
    /// </summary>
    public EditSpecimenMultiTableCommand(
        IGenerateMoreData moreDataGenerator,
        IMoreDataRepository moreDataRepository,
        IJsonUtils jsonUtils,
        ILogWriter logWriter,
        EditSpecimenCommand editSpecimenCommand)
    {
        _moreDataGenerator = moreDataGenerator;
        _moreDataRepository = moreDataRepository;
        _jsonUtils = jsonUtils;
        _logWriter = logWriter;
        _editSpecimenCommand = editSpecimenCommand;
    }

    /// <summary>
    /// Persists specimen changes and merges MoreData on linked patient, admission and request rows.
    /// </summary>
    /// <param name="connect">Open database connection.</param>
    /// <param name="dataToSave">Serialised edit payload.</param>
    /// <param name="tableExceptions">Page-driven field routing.</param>
    /// <param name="newStateId">Target specimen workflow state id.</param>
    public async Task ExecuteAsync(
        NpgsqlConnection connect,
        string dataToSave,
        IReadOnlyList<arc.common.Models.Config.TableExceptionModel> tableExceptions,
        string newStateId)
    {
        await UpdateRelatedMoreDataAsync(connect, dataToSave, tableExceptions);
        await _editSpecimenCommand.ExecuteAsync(connect, dataToSave, "specimen", newStateId);
    }

    private async Task UpdateRelatedMoreDataAsync(
        NpgsqlConnection connect,
        string dataToSave,
        IReadOnlyList<arc.common.Models.Config.TableExceptionModel> tableExceptions)
    {
        var targets = new (string Table, string IdField)[]
        {
            ("patient", "PatientId"),
            ("admission", "AdmissionId"),
            ("request", "RequestId")
        };

        foreach (var (table, idField) in targets)
        {
            var entityId = _jsonUtils.GetSingleFieldValue(dataToSave, idField);
            if (string.IsNullOrWhiteSpace(entityId) || entityId == "0")
            {
                _logWriter.LogInfo(
                    $"Edit specimen: skipping {table} MoreData update — missing or zero {idField}",
                    nameof(EditSpecimenMultiTableCommand),
                    nameof(UpdateRelatedMoreDataAsync));
                continue;
            }

            var fragment = _moreDataGenerator.GetExceptionListString(table, dataToSave, tableExceptions?.ToList());
            if (string.IsNullOrWhiteSpace(fragment) || fragment == "{}")
            {
                _logWriter.LogInfo(
                    $"Edit specimen: skipping {table} MoreData update for id {entityId} — empty fragment",
                    nameof(EditSpecimenMultiTableCommand),
                    nameof(UpdateRelatedMoreDataAsync));
                continue;
            }

            var merged = await _moreDataRepository.CombineWithExistingFieldAsync(fragment, table, entityId, connect);
            _logWriter.LogInfo(
                $"Edit specimen: updating {table} MoreData for id {entityId}",
                nameof(EditSpecimenMultiTableCommand),
                nameof(UpdateRelatedMoreDataAsync));

            await connect.ExecuteAsync(
                $"update {table} set moredata = cast(@MoreData as json), lastmodifieddate = now() where id = @Id",
                new { MoreData = merged, Id = int.Parse(entityId) });
        }
    }
}
