using arc.app.Common;
using arc.app.Specimen;
using arc.common;
using arc.common.Data;
using arc.common.Models.AST;
using arc.common.Models.Instruments;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.data.Configuration;
using arc.data.Instruments;
using arc.data.model.Culture;
using arc.data.Utils;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    public class CultureRepository : ICultureRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;
        private readonly ISqlQuery _sqlQuery;
        private readonly ISqlCommand _sqlCommand;
        private ILogWriter _logWriter;
        private readonly IGenerateMoreData _moreDataGenerator;
        private readonly IMoreDataRepository _moreDataRepository;
        private readonly IJsonElementRemover _jsonElementRemover;
        private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;

        public CultureRepository(IOptionsMonitor<DataOptions> options, ILogger logger, ISqlQuery sqlQuery, ILogWriter logWriter, IGenerateMoreData moreDataGenerator, IMoreDataRepository moreDataRepository, IJsonElementRemover jsonElementRemover, ISqlCommand sqlCommand, IInstrumentInterfaceHandler instrumentInterfaceHandler)
        {
            _options = options;
            _logger = logger;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
            _moreDataGenerator = moreDataGenerator;
            _moreDataRepository = moreDataRepository;
            _jsonElementRemover = jsonElementRemover;
            _sqlCommand = sqlCommand;
            _instrumentInterfaceHandler = instrumentInterfaceHandler;
        }

        /// <summary>
        /// Retrieves AST culture data for the given culture, including AST comments and AST recorded date/time.
        /// ASTCompletedDate and ASTCompletedTime from the Culture table are returned as CompletedDate and CompletedTime.
        /// </summary>
        /// <param name="id">The culture ID.</param>
        /// <returns>AST culture model with organism, specimen type, comments, and completed date/time.</returns>
        public async Task<ASTCultureModel> GetASTCultureDataAsync(string id)
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
//                    var id = queryFilters.Parameters[0].Value;

                    var sql = @"select c.Id, c.SpecimenId, c.ASTCommentOneId, c.ASTCommentTwoId, c.ASTAdditionalNotes, c.OrgGroupCodingId, c.specimenorganismid As organismid, c.TypeId As CultureTypeId, x.LaboratoryId,
                                to_char(c.ASTCompletedDate::DATE, 'yyyy-mm-dd') As CompletedDate,
                                c.ASTCompletedTime As CompletedTime
                                from Culture c
                                inner join Specimen x on x.Id = c.SpecimenId
                                where c.Id = @Id";

                    var queryResult = await connect.QueryAsync<ASTCultureModel>(sql, new { Id = int.Parse(id) });
                    var cultureResult = queryResult.FirstOrDefault();

                    sql = @"select c.Id, c.Comment, c.CultureId, c.CommentTypeId, c.CannedCommentId, c.FieldId, l.Value
                            from SpecimenComment c left outer join listitem l on c.CannedCommentId = l.Id
                            where c.cultureid = @Id";

                    var commentQueryResult = await connect.QueryAsync<CommentModel>(sql, new { Id = int.Parse(id)});

                    var commentsToUse = new List<CommentModel>();

                    foreach (var comment in commentQueryResult)
                    {
                        if (comment.FieldId == "astcommentoneid" || comment.FieldId == "astcommenttwoid" || comment.FieldId == "astadditionalnotes")
                        {
                            commentsToUse.Add(comment);
                        }
                    }

                    foreach (var comment in commentsToUse)
                    {
                        if (comment.FieldId == "astcommentoneid")
                        {
                            cultureResult.ASTCommentOne = comment.CannedCommentId.ToString();
                            cultureResult.ASTCommentOneId = comment.CannedCommentId;
                        }
                        else if (comment.FieldId == "astcommenttwoid")
                        {
                            cultureResult.ASTCommentTwo = comment.CannedCommentId.ToString();
                            cultureResult.ASTCommentTwoId = comment.CannedCommentId;
                        }
                        else if (comment.FieldId == "astadditionalnotes")
                        {
                            cultureResult.ASTAdditionalNotes = comment.Comment;
                        }
                    }

                    return cultureResult;
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Getting AST culture data caused ngSQL exception : {0}", e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a single specimen-patient record by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the SpecimenPatientModel object.</returns>
        public async Task<SpecimenPatientModel> GetSingleAsync(int id)
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var sql = @"select s.id As SpecimenId, patientid, accessionnumber from specimen s
                        inner join culture c on s.id = c.specimenid
                        where c.id = @Id;";

                    return await connect.QueryFirstAsync<SpecimenPatientModel>(sql, new { Id = id });
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Getting single specimen caused ngSQL exception: {0}", e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Updates the culture result asynchronously.
        /// </summary>
        /// <param name="result">The BloodCultureResultsModel object containing the result data to update.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an integer representing the result of the update operation.</returns>
        public async Task<int> UpdateCultureResultAsync(BloodCultureResultsModel result)
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    if (result.Id != 0)
                    {
                        var sql = @"update culture set specimenquantityid = " + int.Parse(result.SpecimenQuantity);
                        sql += !string.IsNullOrEmpty(result.PositiveDate) ? ", positivedate = '" + result.PositiveDate + "'" : "";
                        sql += !string.IsNullOrEmpty(result.PositiveTime) ? ", positivetime = '" + result.PositiveTime + "'" : "";
                        sql += " where id = " + result.Id;
                        await connect.ExecuteAsync(sql);
                    }
                    return result.Id;
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Updating culture data caused ngSQL exception: {0}", e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the culture list by specimen ID asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains a list of CultureListModel objects.</returns>
        public async Task<List<CultureListModel>> GetCultureListBySpecimenIdAsync(QueryFilterConfig queryFilters)
        {
            var cultures = await _sqlQuery.QueryReturningTypeAsync(new CultureListBySpecimenIdQuery(), "Get Culture List For Specimen", queryFilters);

            foreach (var culture in cultures.Where(c =>
                !string.IsNullOrWhiteSpace(c.GrowthId) &&
                string.IsNullOrWhiteSpace(c.GrowthTypeParentId)))
            {
                _logWriter.LogInfo(
                    $"CultureListBySpecimenId: culture Id={culture.Id} has GrowthId={culture.GrowthId} but no GrowthTypeParentId (listitemparentchild missing for growth leaf); AST menu may rely on growthid fallback",
                    nameof(CultureRepository),
                    nameof(GetCultureListBySpecimenIdAsync));
            }

            return cultures;
        }

        /// <summary>
        /// Retrieves the culture view by ID asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the CultureViewModel object.</returns>
        public async Task<CultureViewModel> GetCultureViewByIdAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new CultureViewByIdQuery(), "Get Culture View by id", queryFilters);
        }

        /// <summary>
        /// Retrieves the culture by ID asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the CultureModel object.</returns>
        public async Task<CultureModel> GetCultureByIdAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new CultureByIdQuery(), "Get Culture by id", queryFilters);
        }

        /// <summary>
        /// Retrieves the culture by accession number and culture number asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the CultureModel object.</returns>
        public async Task<CultureDataModel> GetCultureByAccessionNumberAndCultureNumberAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new CultureByAccessionNumberAndCultureNumber(), "Get Culture by accession number and culture number", queryFilters);
        }

        /// <summary>
        /// Retrieves the culture by barcode asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the CultureModel object.</returns>
        public async Task<CultureModel> GetCultureByBarcodeAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new CultureByBarcodeQuery(), "Get Culture by id", queryFilters);
        }


        /// <summary>
        /// Adds a new culture entry using the provided data and event configuration.
        /// </summary>
        /// <param name="dataToSave">The serialized data to be saved.</param>
        /// <param name="command">The event command model.</param>
        /// <param name="eventData">The configuration details related to the event.</param>
        /// <param name="laboratoryConfigList">The list of laboratory configurations.</param>
        /// <param name="username">The username of the person initiating the operation.</param>
        /// <returns>A Task resolving to an integer indicating the result of the operation.</returns>
        public async Task<int> AddCultureAsync(string dataToSave, EventModel command, EventConfig eventData, LaboratoryConfigurationListModel laboratoryConfigList, string username)
        {
            var addCommand = new AddCultureCommand(_options,_logWriter,_moreDataGenerator,_moreDataRepository,_jsonElementRemover,_instrumentInterfaceHandler);
            return await addCommand.ExecuteAsync(dataToSave, command, eventData, laboratoryConfigList, username);
        }

        /// <summary>
        /// Executes the culture editing workflow by initializing an <see cref="EditCultureCommand"/>
        /// and invoking its asynchronous execution logic.
        /// </summary>
        /// <param name="dataToSave">Serialized culture data to persist.</param>
        /// <param name="command">The event model containing context for the edit operation.</param>
        /// <param name="eventData">Configuration details specific to the current event.</param>
        /// <param name="laboratoryConfigList">List of laboratory configurations relevant to the operation.</param>
        /// <param name="username">The user initiating the edit, used for audit or tracking purposes.</param>
        /// <returns>
        /// A task that resolves to an integer status code indicating the result of the operation.
        /// </returns>
        public async Task<int> EditCultureAsync(string dataToSave, EventModel command, EventConfig eventData, string id)
        {
            var addCommand = new EditCultureCommand(_options, _logWriter, _moreDataGenerator, _moreDataRepository, _jsonElementRemover, _instrumentInterfaceHandler);
            return await addCommand.ExecuteAsync(dataToSave, command, eventData, id);
        }

        /// <summary>
        /// Executes the growth for all cultures in a specimen asynchronously.
        /// </summary>
        /// <param name="cultureList">The list of culture models.</param>
        /// <param name="specimenQuantityId">The specimen quantity ID.</param>
        /// <param name="newStateId">The new state ID.</param>
        /// <param name="specimenId">The specimen ID.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task GrowthForAllCulturesInSpecimenAsync(List<CultureListModel> cultureList, int specimenQuantityId, int newStateId, int specimenId)
        {
            var command = new GrowthForAllCulturesInSpecimenCommand(_options);
            await command.ExecuteAsync(cultureList, specimenQuantityId, specimenId, newStateId);
        }

        /// <summary>
        /// Deletes a culture asynchronously based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the culture to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteCultureAsync(string id)
        {
            _logWriter.LogInfo("Run delete culture command", "CultureRepository", "DeleteCultureAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteCultureCommand(), "Delete Culture", id);
        }

        /// <summary>
        /// Deletes an isolate asynchronously based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the culture to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteIsolateAsync(string id)
        {
            _logWriter.LogInfo("Run delete isolate command", "CultureRepository", "DeleteIsolateAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteIsolateCommand(), "Delete Isolate", id);
        }

        /// <summary>
        /// Replaces all culture file attachments with the given list of file attachment IDs.
        /// Deletes existing links for the culture, then inserts the new set.
        /// </summary>
        public async Task<int> SetCultureFileAttachmentsAsync(int cultureId, IEnumerable<int> fileAttachmentIds)
        {
            var model = new SetCultureFileAttachmentsModel
            {
                CultureId = cultureId,
                FileAttachmentIds = fileAttachmentIds
            };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new SetCultureFileAttachmentsCommand(),
                "Set Culture File Attachments",
                model,
                _logWriter);
        }

        /// <inheritdoc />
        public async Task<int> CreateCultureForSpecimenAndTypeAsync(int specimenId, int typeId)
        {
            var model = new CreateCultureForSpecimenAndTypeModel { SpecimenId = specimenId, TypeId = typeId };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new CreateCultureForSpecimenAndTypeCommand(),
                "Create culture for specimen and type",
                model,
                _logWriter);
        }
    }
}
