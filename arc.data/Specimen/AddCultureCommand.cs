using arc.app.Common;
using arc.common;
using arc.common.Data;
using arc.common.ExtensionMethods;
using arc.common.Models.Common;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.data.Configuration;
using arc.data.Instruments;
using arc.data.Utils;
using arc.domain.Configuration.EventsConfig;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Specimen;

/// <summary>
/// Handles adding a new culture record to a specimen, including culture tests and comments.
/// Growth is optional; when omitted, the culture is saved with null GrowthId.
/// </summary>
public class AddCultureCommand
{
    private readonly IOptionsMonitor<DataOptions> _options;
    private ILogWriter _logWriter;
    private readonly IGenerateMoreData _moreDataGenerator;
    private readonly IMoreDataRepository _moreDataRepository;
    private readonly IJsonElementRemover _jsonElementRemover;
    private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;

    /// <summary>
    /// Creates a new instance of <see cref="AddCultureCommand"/>.
    /// </summary>
    public AddCultureCommand(IOptionsMonitor<DataOptions> options, ILogWriter logWriter, IGenerateMoreData moreDataGenerator, IMoreDataRepository moreDataRepository, IJsonElementRemover jsonElementRemover, IInstrumentInterfaceHandler instrumentInterfaceHandler)
    {
        _options = options;
        _logWriter = logWriter;
        _moreDataGenerator = moreDataGenerator;
        _moreDataRepository = moreDataRepository;
        _jsonElementRemover = jsonElementRemover;
        _instrumentInterfaceHandler = instrumentInterfaceHandler;
    }

    /// <summary>
    /// Adds a new culture to the specimen. GrowthId is optional; when omitted, the culture is saved with null growth.
    /// </summary>
    /// <param name="dataToSave">Payload containing culture fields (SpecimenId, CultureType, optional GrowthId, etc.).</param>
    /// <param name="command">Event command metadata.</param>
    /// <param name="eventData">Event configuration.</param>
    /// <param name="laboratoryConfiguration">Laboratory configuration for culture test defaults.</param>
    /// <param name="username">Current user for comment attribution.</param>
    /// <returns>The ID of the newly created culture.</returns>
    public async Task<int> ExecuteAsync(string dataToSave, EventModel command, EventConfig eventData, LaboratoryConfigurationListModel laboratoryConfiguration, string username)
    {
        var payload = JObject.Parse(dataToSave);
        var growthId = payload.GetNullableInt32("GrowthId");
        if (growthId == null)
        {
            _logWriter.LogInfo("AddCultureCommand: GrowthId omitted or empty, culture will be saved with null growth", "AddCultureCommand", "ExecuteAsync");
        }

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            dataToSave = await AppendCultureIdAsync(dataToSave);

            var genericRepository = new SqlGenericRepository(_options, _logWriter, _moreDataGenerator, _moreDataRepository, _jsonElementRemover,_instrumentInterfaceHandler);
            genericRepository.AddConfiguration("Culture");
            var comments = JsonConvert.DeserializeObject<AddCultureCommentModel>(dataToSave);

            dataToSave = dataToSave.RemoveElements("CommentOneId", "CommentTwoId", "AdditionalNotes");

            var cultureId = await genericRepository.AddWithoutScopeAsync(dataToSave, command, eventData.StringFields);
            var typeId = JsonConvert.DeserializeObject<TypeIdModel>(dataToSave);
            var sql = "";

            var cultureTestConfig = laboratoryConfiguration.GetConfigurationsForFirstEntryInList("culturetypeculturetestdefault");

            using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
            {
                if (growthId.HasValue)
                {
                    var growthTypeParentId = await connect.GetParentListItemIdAsync(growthId.Value);
                    _logWriter.LogInfo($"AddCultureCommand: GrowthId={growthId}, GrowthTypeParentId={growthTypeParentId?.ToString() ?? "null"} (427=Growth, 428=No Growth)", "AddCultureCommand", "ExecuteAsync");
                    if (growthTypeParentId == null && !IsKnownNoGrowthLeafId(growthId.Value))
                    {
                        _logWriter.LogWarning(
                            $"AddCultureCommand: GrowthId={growthId} has no listitemparentchild parent; verify Specimen Growth hierarchy (427/428) so culture list AST menu resolves growthTypeParentId",
                            "AddCultureCommand",
                            "ExecuteAsync");
                    }
                }
                sql = "update culture set ParentCultureId = @CultureId Where Id = @CultureId and ParentCultureId is null";
                await connect.ExecuteAsync(sql, new { cultureId });


                var cultureTestEntry = cultureTestConfig.FirstOrDefault(c => int.Parse(c.GroupId) == typeId.TypeId);
                if (cultureTestEntry != null)
                {
                    var configIds = cultureTestEntry.AssociatedListId.Split(',').ToArray<string>();

                    if (configIds.Any())
                    {
                        sql = @"INSERT INTO CultureTests (cultureid, testname, status, lastmodifieddate, requested)
                                SELECT @CultureId, configname, 'Requested', NOW(), NOW()
                                FROM configs
                                WHERE configname = ANY(@ConfigIds)";

                        await connect.ExecuteAsync(sql, new { CultureId = cultureId, ConfigIds = configIds });
                    }
                }

                sql = @"select s.id As SpecimenId, patientid, accessionnumber from specimen s
                                inner join culture c on s.id = c.specimenid
                                where c.id = @Id;";

                var specimenData = await connect.QueryFirstAsync<SpecimenPatientModel>(sql, new { Id = cultureId });

                if(comments.CommentOneId != null && comments.CommentOneId != "")
                {
                    sql = @"insert into SpecimenComment(specimenid, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, cannedcommentid, fieldid)
                                                            values(@SpecimenId, now(), 1232, 'Yes', @CultureId, @Username, @Comment, 'comment1')";
                    await connect.ExecuteAsync(sql, new { specimenData.SpecimenId, CultureId = cultureId, Username = username, Comment = int.Parse(comments.CommentOneId) });
                }
                if (comments.CommentTwoId != null && comments.CommentTwoId != "")
                {
                    sql = @"insert into SpecimenComment(specimenid, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, cannedcommentid, fieldid)
                                                            values(@SpecimenId, now(), 1232, 'Yes', @CultureId, @Username, @Comment, 'comment2')";

                    await connect.ExecuteAsync(sql, new { specimenData.SpecimenId, CultureId = cultureId, Username = username, Comment = int.Parse(comments.CommentTwoId) });
                }
                if (comments.AdditionalNotes != null && comments.AdditionalNotes != "")
                {
                    sql = @"insert into SpecimenComment(specimenid, comment, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, fieldid)
                                                            values(@SpecimenId, @Comment, now(), 1232, 'Yes', @CultureId, @Username, 'additionalnotes')";
                    await connect.ExecuteAsync(sql, new { specimenData.SpecimenId, CultureId = cultureId, Username = username, Comment = comments.AdditionalNotes });
                }

                await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureId(specimenData.SpecimenId, cultureId);
            }

            scope.Complete();

            return cultureId;
        };
    }

    /// <summary>
    /// Deserialises <paramref name="dataToSave"/>, injects the next unique culture number
    /// for the specimen, and returns the updated JSON payload.
    /// </summary>
    /// <param name="dataToSave">JSON payload containing at least a <c>SpecimenId</c> property.</param>
    /// <returns>The original JSON payload with a <c>CultureNumber</c> property appended.</returns>
    private async Task<string> AppendCultureIdAsync(string dataToSave)
    {
        dynamic data = JsonConvert.DeserializeObject(dataToSave);
        int specimenId = data.SpecimenId;
        data.CultureNumber = await GetNextCultureNumberAsync(specimenId);
        return JsonConvert.SerializeObject(data);
    }

    /// <summary>
    /// Atomically increments the <c>NextCultureNumber</c> watermark on the
    /// <c>Specimen</c> row and returns the newly allocated number.
    /// <para>
    /// Using an UPDATE…RETURNING rather than <c>MAX(culturenumber)+1</c> ensures the
    /// allocated number is never reused, even when the previously highest-numbered
    /// isolate has been deleted.
    /// </para>
    /// </summary>
    /// <param name="specimenId">The primary key of the specimen for which a new culture number is required.</param>
    /// <returns>The next culture number to assign to the new isolate.</returns>
    /// <summary>
    /// Returns true when <paramref name="growthId"/> is a known SpecimenGrowth no-growth leaf (parent 428).
    /// </summary>
    private static bool IsKnownNoGrowthLeafId(int growthId) =>
        growthId is 177 or 1050 or 1086 or 125;

    private async Task<int> GetNextCultureNumberAsync(int specimenId)
    {
        const string sql = @"UPDATE Specimen
                             SET    NextCultureNumber = NextCultureNumber + 1
                             WHERE  Id = @specimenId
                             RETURNING NextCultureNumber;";
        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        return await connect.QueryFirstAsync<int>(sql, new { specimenId });
    }
}
