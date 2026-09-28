using arc.app.Common;
using arc.common.Data;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.data.Instruments;
using arc.data.Utils;
using arc.domain.Tests;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Handles acknowledgement of a specimen by updating core fields, merging any generated
/// more-data, and creating/removing requested tests and cultures based on the crafted payload.
/// Also interacts with instrument interfaces to create pending results where appropriate.
/// </summary>
internal class AcknowledgeSpecimenCommand
{
    private IInstrumentInterfaceHandler _instrumentInterfaceHandler;
    private readonly IGenerateMoreData _moreDataGenerator;
    private readonly IMoreDataRepository _moreDataRepository;
    private readonly ILogWriter _logWriter;
    private readonly IJsonReplacer _jsonReplacer;
    private readonly IJsonElementRemover _jsonRemover;

    /// <summary>
    /// Initializes a new instance of the <see cref="AcknowledgeSpecimenCommand"/> class.
    /// </summary>
    /// <param name="instrumentInterfaceHandler">Creates pending instrument results for tests/cultures.</param>
    /// <param name="moreDataGenerator">Generates structured more-data blocks from payloads.</param>
    /// <param name="moreDataRepository">Persists and merges more-data with existing records.</param>
    /// <param name="logWriter">Writes structured log entries during processing.</param>
    /// <param name="jsonReplacer">Utility to mutate JSON payload values.</param>
    /// <param name="jsonRemover">Utility to remove JSON elements by value.</param>
    public AcknowledgeSpecimenCommand(IInstrumentInterfaceHandler instrumentInterfaceHandler, IGenerateMoreData moreDataGenerator, IMoreDataRepository moreDataRepository, ILogWriter logWriter, IJsonReplacer jsonReplacer, IJsonElementRemover jsonRemover)
    {
        _instrumentInterfaceHandler = instrumentInterfaceHandler;
        _moreDataGenerator = moreDataGenerator;
        _moreDataRepository = moreDataRepository;
        _logWriter = logWriter;
        _jsonReplacer = jsonReplacer;
        _jsonRemover = jsonRemover;
    }

    /// <summary>
    /// Applies acknowledgement updates to a specimen and processes crafted actions:
    /// test additions/removals and culture creation with default culture tests.
    /// After inserting initial cultures the <c>Specimen.NextCultureNumber</c> watermark is set to
    /// the highest number assigned, ensuring subsequent isolate additions never reuse a deleted number.
    /// </summary>
    /// <param name="connect">An open <see cref="NpgsqlConnection"/> used for all database operations.</param>
    /// <param name="command">Acknowledgement payload containing the specimen id, crafted pages, and barcode data.</param>
    /// <param name="cultureTestConfig">Configuration list used to resolve default culture tests for each culture type.</param>
    /// <param name="json">Original JSON payload; used to build the specimen <c>MoreData</c> blob and to strip system fields before the UPDATE.</param>
    /// <returns><c>0</c> on success.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ACKReceiptEventModel command, List<CultureTestConfigModel> cultureTestConfig, string json)
    {

        var receivedDateIso = ToIsoDateString(command.ReceivedDate);
        var dataToSave = _jsonReplacer.ChangeValueInJsonString(json, "ReceivedDate", receivedDateIso);
        dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "Crafted");
        dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "ApplyDefaults");
        dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "Action");
        dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "Event");
        dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "View");
        dataToSave = _jsonReplacer.AddNewStringValue(dataToSave, "StateId", command.StateId);
        if(command.BottleOnlyWeight == null)
        {
            dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "BottleOnlyWeight");
        }
        if (command.BloodandBottleWeight == null)
        {
            dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "BloodAndBottleWeight");
        }
        if (command.RejectionReason == null)
        {
            dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "RejectionReason");
        }
        if (command.SelectReasonId == null)
        {
            dataToSave = _jsonRemover.RemoveElementsByValue(dataToSave, "SelectReasonId");
        }
        var moreData = _moreDataGenerator.GetMoreDataJsonString("specimen", dataToSave);

        if (moreData != "{}")
        {
            _logWriter.LogInfo("More data found and being combined", "AcknowledgeSpecimenCommand", "Execute");
            moreData = await _moreDataRepository.CombineWithExistingFieldAsync(moreData, "specimen", command.Id.ToString(), connect);
            dataToSave = _moreDataGenerator.GetLeftOverData();
        };

        var builder = new SqlBuilder(dataToSave, "");
        _logWriter.LogInfo("Start building the parameterized sql", "AcknowledgeSpecimenCommand", "Execute");
        var sqlResult = builder.BuildUpdateSqlParameterized("specimen", command.Id.ToString(), moreData);

        if (sqlResult.ExcludedMetadataFields.Count > 0)
        {
            _logWriter.LogInfo($"WARN: Excluded client metadata fields from specimen SQL: {string.Join(", ", sqlResult.ExcludedMetadataFields)}", "AcknowledgeSpecimenCommand", "Execute");
        }

        if (!string.IsNullOrWhiteSpace(sqlResult.Sql))
        {
            _logWriter.LogInfo("Execute the sql statement with parameters", "AcknowledgeSpecimenCommand", "Execute");
            await connect.ExecuteAsync(sqlResult.Sql, sqlResult.Parameters);
        }

        string sql;
        var index = 0;
        if (command.Crafted != null)
        {
            foreach (var dataEntry in command.Crafted)
            {

                // Add Tests

                if (command.Crafted[index].Name == "testselectionpage")
                {
                    // Get existing tests.
                    sql = @"select * from Tests where SpecimenId = @Id";

                    var existingTests = await connect.QueryAsync<Test>(sql, new { Id = command.Id });

                    var testList = command.Crafted[index].Contents;

                    foreach (var test in testList)
                    {
                        var alreadyPresent = existingTests.FirstOrDefault(x => x.TestName.ToLower() == test.Key.ToLower());

                        var parameters = new { SpecimenId = command.Id, TestName = test.Key };
                        if (test.Allowed == "Yes")
                        {
                            if (alreadyPresent == null)
                            {
                                sql = @"insert into Tests(specimenid, testname, status, lastmodifieddate, requested)
                                        values(@SpecimenId, @TestName, 'Requested', now(), now())";

                                await connect.ExecuteAsync(sql, parameters);

                                await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForDirectTestId(parameters.SpecimenId, test.Key);
                            }

                        }
                        else
                        {
                            if (alreadyPresent != null)
                            {

                                var id = alreadyPresent.Id;

                                sql = @"delete from Tests where Id = @Id";

                                await connect.ExecuteAsync(sql, new { Id = id });
                            }
                        }
                    }
                }

                // Add Cultures

                if (command.Crafted[index].Name == "culturetypeselectionpage")
                {
                    var cultureList = command.Crafted[index].Contents;
                    command.ManufacturersBarcode = command.ManufacturersBarcode == null ? "" : command.ManufacturersBarcode;

                    int cultureNumber = 1;
                    foreach (var culture in cultureList)
                    {
                        var parameters = new
                        {
                            SpecimenId = command.Id,
                            TypeId = int.Parse(culture.Key),
                            DisplayOnReport = "Yes",
                            MoreData = "{\"ManufacturersBarcode\":\"" + command.ManufacturersBarcode + "\"}",
                            CultureNumber = cultureNumber
                        };
                        if (culture.Allowed == "Yes")
                        {
                            sql = @"insert into Culture(specimenid, typeid, culturenumber, displayonreport, moredata, lastmodifieddate)
                                                    values(@SpecimenId, @TypeId, @CultureNumber, @DisplayOnReport, cast(@MoreData as json), now()) returning id";

                            var cultureId = await connect.QueryFirstAsync<int>(sql, parameters);
                            cultureNumber++;

                            var cultureTestEntry = cultureTestConfig.FirstOrDefault(c => c.CultureType == parameters.TypeId);
                            if (cultureTestEntry != null)
                            {
                                foreach (var entry in cultureTestEntry.Values)
                                {
                                    sql = @"insert into CultureTests(cultureid, testname, status, lastmodifieddate, requested)
                                                            values(@CultureId, @TestName, 'Requested', now(), now())";

                                    await connect.ExecuteAsync(sql, new { CultureId = cultureId, testname = entry });

                                    await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureTestId(parameters.SpecimenId, cultureId, entry);
                                }
                            }

                            sql = "update culture set ParentCultureId = @CultureId Where Id = @CultureId and ParentCultureId is null";
                            await connect.ExecuteAsync(sql, new { cultureId });

                            await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureId(parameters.SpecimenId, cultureId);
                        }
                    }

                    // Sync the watermark so that subsequent isolate additions never reuse a
                    // culture number that was assigned during specimen acknowledgement.
                    // cultureNumber has already been incremented past the last assigned value,
                    // so the highest number actually used is cultureNumber - 1.
                    await connect.ExecuteAsync(
                        "UPDATE Specimen SET NextCultureNumber = @MaxCultureNumber WHERE Id = @SpecimenId",
                        new { MaxCultureNumber = cultureNumber - 1, SpecimenId = command.Id });
                }
                index++;
            }
        }

        return 0;
    }

    /// <summary>
    /// Normalizes a received-date string to invariant <c>yyyy-MM-dd</c> for reliable date-only SQL parameter binding.
    /// </summary>
    private static string ToIsoDateString(string receivedDate)
    {
        if (string.IsNullOrWhiteSpace(receivedDate))
        {
            return receivedDate ?? string.Empty;
        }

        var s = receivedDate.Trim();
        if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
        {
            return dt.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
        {
            return dt.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        if (DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
        {
            return dt.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        dt = Convert.ToDateTime(s, CultureInfo.CurrentCulture);
        return dt.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}
