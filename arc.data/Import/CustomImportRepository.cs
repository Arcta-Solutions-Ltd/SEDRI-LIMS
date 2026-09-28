using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using arc.app.Common;
using arc.app.Import;
using arc.data.Configuration;
using arc.common.Models.Instruments.CustomImport;
using arc.domain.Instruments;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;

namespace arc.data.Import
{
    /// <summary>
    /// Default <see cref="ICustomImportRepository"/>. Saves a whole Custom-interface record inside a single ambient
    /// <see cref="TransactionScope"/> and one <see cref="NpgsqlConnection"/> so all writes for the record commit or roll
    /// back together. All matching is by id / resolved reference value; list values are already resolved to ids upstream.
    /// </summary>
    public class CustomImportRepository : ICustomImportRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImportRepository"/> class.
        /// </summary>
        /// <param name="options">Data options providing the Arc connection string.</param>
        /// <param name="logWriter">Log writer for installed-system diagnostics.</param>
        public CustomImportRepository(IOptionsMonitor<DataOptions> options, ILogWriter logWriter)
        {
            _options = options;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<CustomImportSaveResult> SaveRecordAsync(CustomImportSaveModel model)
        {
            var result = new CustomImportSaveResult();
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    await connect.OpenAsync();

                    int patientId = 0;
                    if (model.CreatePatients && model.Patient != null)
                    {
                        var (id, created) = await UpsertAsync(connect, "patient",
                            BuildWhere(model.Patient.ReferenceColumn, model.Patient.ReferenceValue, out var pParams), pParams,
                            model.Patient.Columns, BuildMoreData(model.Patient.MoreData),
                            new Dictionary<string, object> { ["surname"] = " " });
                        patientId = id;
                        result.PatientId = id;
                        Tally(result, created);
                        _logWriter.LogInfo($"Custom import: patient {(created ? "created" : "updated")} id={id}", nameof(CustomImportRepository), nameof(SaveRecordAsync));
                    }

                    foreach (var specimen in model.Specimens)
                    {
                        var specimenId = await SaveSpecimenAsync(connect, model, specimen, patientId, result);
                        result.SpecimenIds.Add(specimenId);
                    }

                    scope.Complete();
                }

                result.Success = true;
                return result;
            }
            catch (CustomImportException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Custom import: record save failed: {ex.Message}", nameof(CustomImportRepository), nameof(SaveRecordAsync));
                throw;
            }
        }

        private async Task<int> SaveSpecimenAsync(NpgsqlConnection connect, CustomImportSaveModel model, CustomImportEntity specimen, int patientId, CustomImportSaveResult result)
        {
            var existingId = await connect.QueryFirstOrDefaultAsync<int?>(
                $"select id from specimen where {BuildWhere(specimen.ReferenceColumn, specimen.ReferenceValue, out var sParams)} limit 1", sParams);

            int specimenId;
            if (existingId.HasValue && existingId.Value > 0)
            {
                specimenId = existingId.Value;
                await UpdateColumnsAsync(connect, "specimen", specimenId, specimen.Columns, BuildMoreData(specimen.MoreData));
                Tally(result, false);
                _logWriter.LogInfo($"Custom import: specimen updated id={specimenId}", nameof(CustomImportRepository), nameof(SaveSpecimenAsync));
            }
            else
            {
                if (patientId <= 0)
                {
                    throw new CustomImportException("@InsCusPat@",
                        $"Specimen '{specimen.ReferenceValue}' cannot be created because its patient does not exist and the profile does not create patients.");
                }

                var insertOnly = new Dictionary<string, object>
                {
                    ["patientid"] = patientId,
                    ["collectiondate"] = DateTime.UtcNow.ToString("yyyy-MM-dd")
                };
                if (model.LaboratoryId > 0) insertOnly["laboratoryid"] = model.LaboratoryId;
                if (model.OrganisationId > 0) insertOnly["organisationid"] = model.OrganisationId;

                var (id, _) = await UpsertAsync(connect, "specimen",
                    BuildWhere(specimen.ReferenceColumn, specimen.ReferenceValue, out var sInsParams), sInsParams,
                    specimen.Columns, BuildMoreData(specimen.MoreData), insertOnly);
                specimenId = id;
                Tally(result, true);
                _logWriter.LogInfo($"Custom import: specimen created id={specimenId}", nameof(CustomImportRepository), nameof(SaveSpecimenAsync));
            }

            foreach (var test in specimen.ChildrenOfType(ImportEntityType.DirectTest))
            {
                await SaveTestAsync(connect, "tests", "specimenid", specimenId, test, result);
            }

            foreach (var culture in specimen.ChildrenOfType(ImportEntityType.Culture))
            {
                await SaveCultureAsync(connect, specimenId, culture, result);
            }

            return specimenId;
        }

        private async Task SaveCultureAsync(NpgsqlConnection connect, int specimenId, CustomImportEntity culture, CustomImportSaveResult result)
        {
            int cultureId;
            int? existingId = null;
            var cultureNumber = culture.ReferenceValue;

            if (!string.IsNullOrWhiteSpace(cultureNumber) && int.TryParse(cultureNumber, out var cnum))
            {
                existingId = await connect.QueryFirstOrDefaultAsync<int?>(
                    "select id from culture where specimenid = @sid and culturenumber = @cnum limit 1",
                    new { sid = specimenId, cnum });
            }

            if (existingId.HasValue && existingId.Value > 0)
            {
                cultureId = existingId.Value;
                await UpdateColumnsAsync(connect, "culture", cultureId, culture.Columns, BuildMoreData(culture.MoreData));
                Tally(result, false);
                _logWriter.LogInfo($"Custom import: culture updated id={cultureId}", nameof(CustomImportRepository), nameof(SaveCultureAsync));
            }
            else
            {
                var columns = new Dictionary<string, object>(culture.Columns, StringComparer.OrdinalIgnoreCase);
                if (!columns.ContainsKey("culturenumber") || string.IsNullOrWhiteSpace(columns["culturenumber"]?.ToString()))
                {
                    var next = await connect.QueryFirstAsync<int>(
                        "update specimen set nextculturenumber = coalesce(nextculturenumber,0) + 1 where id = @sid returning nextculturenumber",
                        new { sid = specimenId });
                    columns["culturenumber"] = next;
                }

                var cols = BuildColumns(columns, BuildMoreData(culture.MoreData), "c_");
                cols.InsertColumns.Add("specimenid");
                cols.InsertValues.Add("@c_specimenid");
                cols.Parameters.Add("c_specimenid", specimenId);
                cols.InsertColumns.Add("displayonreport");
                cols.InsertValues.Add("@c_displayonreport");
                cols.Parameters.Add("c_displayonreport", "Yes");

                var insertSql = $"insert into culture ({string.Join(", ", cols.InsertColumns.Append("lastmodifieddate"))}) " +
                                $"values ({string.Join(", ", cols.InsertValues.Append("now()"))}) returning id";
                cultureId = await connect.QueryFirstAsync<int>(insertSql, cols.Parameters);
                await connect.ExecuteAsync("update culture set parentcultureid = @id where id = @id and (parentcultureid is null or parentcultureid = 0)", new { id = cultureId });
                Tally(result, true);
                _logWriter.LogInfo($"Custom import: culture created id={cultureId}", nameof(CustomImportRepository), nameof(SaveCultureAsync));
            }

            foreach (var test in culture.ChildrenOfType(ImportEntityType.CultureTest))
            {
                await SaveTestAsync(connect, "culturetests", "cultureid", cultureId, test, result);
            }

            foreach (var ast in culture.ChildrenOfType(ImportEntityType.Ast))
            {
                await SaveAstAsync(connect, cultureId, ast, result);
            }
        }

        private async Task SaveTestAsync(NpgsqlConnection connect, string table, string parentColumn, int parentId, CustomImportEntity test, CustomImportSaveResult result)
        {
            var testName = test.ReferenceValue;
            if (string.IsNullOrWhiteSpace(testName))
            {
                return;
            }

            var testResultsJson = test.TestResults != null && test.TestResults.Count > 0
                ? JsonConvert.SerializeObject(test.TestResults)
                : null;

            var existingId = await connect.QueryFirstOrDefaultAsync<int?>(
                $"select id from {table} where {parentColumn} = @pid and lower(trim(testname)) = lower(trim(@tn)) limit 1",
                new { pid = parentId, tn = testName });

            if (existingId.HasValue && existingId.Value > 0)
            {
                await connect.ExecuteAsync(
                    $"update {table} set testresults = cast(@tr as jsonb), status = 'Complete', completed = now(), lastmodifieddate = now() where id = @id",
                    new { tr = testResultsJson, id = existingId.Value });
                Tally(result, false);
            }
            else
            {
                await connect.ExecuteAsync(
                    $"insert into {table} ({parentColumn}, testname, testresults, status, requested, completed, lastmodifieddate) " +
                    "values (@pid, @tn, cast(@tr as jsonb), 'Complete', now(), now(), now())",
                    new { pid = parentId, tn = testName, tr = testResultsJson });
                Tally(result, true);
            }
            _logWriter.LogInfo($"Custom import: {table} '{testName}' saved for {parentColumn}={parentId}", nameof(CustomImportRepository), nameof(SaveTestAsync));
        }

        private async Task SaveAstAsync(NpgsqlConnection connect, int cultureId, CustomImportEntity ast, CustomImportSaveResult result)
        {
            var antibioticId = ast.Columns.TryGetValue("antibioticid", out var abId) ? abId?.ToString() : null;

            int? existingId = null;
            if (!string.IsNullOrWhiteSpace(antibioticId) && int.TryParse(antibioticId, out var ab))
            {
                existingId = await connect.QueryFirstOrDefaultAsync<int?>(
                    "select id from ast where cultureid = @cid and antibioticid = @ab limit 1",
                    new { cid = cultureId, ab });
            }

            if (existingId.HasValue && existingId.Value > 0)
            {
                await UpdateColumnsAsync(connect, "ast", existingId.Value, ast.Columns, BuildMoreData(ast.MoreData));
                Tally(result, false);
            }
            else
            {
                var cols = BuildColumns(ast.Columns, BuildMoreData(ast.MoreData), "a_");
                cols.InsertColumns.Add("cultureid");
                cols.InsertValues.Add("@a_cultureid");
                cols.Parameters.Add("a_cultureid", cultureId);
                var insertSql = $"insert into ast ({string.Join(", ", cols.InsertColumns.Append("lastmodifieddate"))}) " +
                                $"values ({string.Join(", ", cols.InsertValues.Append("now()"))}) returning id";
                await connect.QueryFirstAsync<int>(insertSql, cols.Parameters);
                Tally(result, true);
            }
            _logWriter.LogInfo($"Custom import: ast saved for cultureid={cultureId}", nameof(CustomImportRepository), nameof(SaveAstAsync));
        }

        private static async Task<(int id, bool created)> UpsertAsync(NpgsqlConnection connect, string table,
            string whereSql, object whereParams, Dictionary<string, object> columns, string moreDataJson,
            Dictionary<string, object> insertOnly)
        {
            var existing = await connect.QueryFirstOrDefaultAsync<int?>($"select id from {table} where {whereSql} limit 1", whereParams);
            var cols = BuildColumns(columns, moreDataJson, "u_");

            if (existing.HasValue && existing.Value > 0)
            {
                if (cols.UpdateAssignments.Count > 0)
                {
                    cols.Parameters.Add("u_id", existing.Value);
                    await connect.ExecuteAsync($"update {table} set {string.Join(", ", cols.UpdateAssignments)}, lastmodifieddate = now() where id = @u_id", cols.Parameters);
                }
                return (existing.Value, false);
            }

            foreach (var kv in insertOnly ?? new Dictionary<string, object>())
            {
                var col = kv.Key.ToLowerInvariant();
                if (cols.InsertColumns.Contains(col)) continue;
                var val = kv.Value?.ToString();
                if (string.IsNullOrWhiteSpace(val)) continue;
                var p = "ui_" + col;
                var cast = CastType(col);
                cols.InsertColumns.Add(col);
                cols.InsertValues.Add(cast == null ? $"@{p}" : $"cast(@{p} as {cast})");
                cols.Parameters.Add(p, val);
            }

            var insertSql = $"insert into {table} ({string.Join(", ", cols.InsertColumns.Append("lastmodifieddate"))}) " +
                            $"values ({string.Join(", ", cols.InsertValues.Append("now()"))}) returning id";
            var newId = await connect.QueryFirstAsync<int>(insertSql, cols.Parameters);
            return (newId, true);
        }

        private static async Task UpdateColumnsAsync(NpgsqlConnection connect, string table, int id, Dictionary<string, object> columns, string moreDataJson)
        {
            var cols = BuildColumns(columns, moreDataJson, "up_");
            if (cols.UpdateAssignments.Count == 0)
            {
                return;
            }
            cols.Parameters.Add("up_id", id);
            await connect.ExecuteAsync($"update {table} set {string.Join(", ", cols.UpdateAssignments)}, lastmodifieddate = now() where id = @up_id", cols.Parameters);
        }

        private static string BuildWhere(string column, string value, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            var col = string.IsNullOrWhiteSpace(column) ? "id" : column.ToLowerInvariant();
            var cast = CastType(col);
            parameters.Add("ref", string.IsNullOrWhiteSpace(value) ? null : value);
            return cast == null ? $"{col} = @ref" : $"{col} = cast(@ref as {cast})";
        }

        private static string BuildMoreData(Dictionary<string, string> moreData)
        {
            if (moreData == null || moreData.Count == 0)
            {
                return null;
            }
            return JsonConvert.SerializeObject(moreData);
        }

        private static void Tally(CustomImportSaveResult result, bool created)
        {
            if (created) result.CreatedCount++;
            else result.UpdatedCount++;
        }

        private sealed class ColumnSql
        {
            public List<string> InsertColumns { get; } = new();
            public List<string> InsertValues { get; } = new();
            public List<string> UpdateAssignments { get; } = new();
            public DynamicParameters Parameters { get; } = new();
        }

        private static ColumnSql BuildColumns(Dictionary<string, object> columns, string moreDataJson, string prefix)
        {
            var result = new ColumnSql();
            foreach (var kv in columns ?? new Dictionary<string, object>())
            {
                var col = kv.Key.ToLowerInvariant();
                var val = kv.Value?.ToString();
                if (string.IsNullOrWhiteSpace(val))
                {
                    continue;
                }
                var p = prefix + col;
                var cast = CastType(col);
                var valueExpr = cast == null ? $"@{p}" : $"cast(@{p} as {cast})";
                result.InsertColumns.Add(col);
                result.InsertValues.Add(valueExpr);
                result.UpdateAssignments.Add($"{col} = {valueExpr}");
                result.Parameters.Add(p, val);
            }

            if (!string.IsNullOrWhiteSpace(moreDataJson) && moreDataJson != "{}")
            {
                result.InsertColumns.Add("moredata");
                result.InsertValues.Add($"cast(@{prefix}moredata as jsonb)");
                result.UpdateAssignments.Add($"moredata = cast(@{prefix}moredata as jsonb)");
                result.Parameters.Add(prefix + "moredata", moreDataJson);
            }

            return result;
        }

        private static string CastType(string column) => column.ToLowerInvariant() switch
        {
            "genderid" or "specimentypeid" or "typeid" or "growthid" or "specimenorganismid"
                or "patientid" or "specimenid" or "cultureid" or "parentcultureid" or "culturenumber"
                or "antibioticid" or "susceptibilityid" or "categoryid" or "testmethodid"
                or "laboratoryid" or "organisationid" or "stateid" or "locationid" or "alerttypeid" or "dosage" => "integer",
            "measurement" => "numeric",
            "dateofbirth" or "collectiondate" or "receiveddate" => "date",
            _ => null
        };
    }
}
