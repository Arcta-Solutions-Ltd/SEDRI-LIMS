using arc.app.AST;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.AST;
using arc.common.Models.Instruments;
using arc.common.Models.Specimen;
using arc.common.Models.Tests;
using arc.data.AST;
using arc.data.Configuration;
using arc.data.model.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Specimen
{
    public class ASTRepository : IASTRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;
        private readonly ISqlQuery _sqlQuery;
        private readonly ISqlCommand _sqlCommand;
        private readonly IAstSusceptibilityOverrideRepository _overrideRepository;


        public ASTRepository(IOptionsMonitor<DataOptions> options, ILogger logger, ISqlQuery sqlQuery, ISqlCommand sqlCommand,
            IAstSusceptibilityOverrideRepository overrideRepository)
        {
            _options = options;
            _logger = logger;
            _sqlQuery = sqlQuery;
            _sqlCommand = sqlCommand;
            _overrideRepository = overrideRepository;
        }

        /// <summary>
        /// Saves AST results for a culture. Persists CompletedDate and CompletedTime to the Culture table
        /// (ASTCompletedDate, ASTCompletedTime) so they are redisplayed when the AST form is reloaded.
        /// Expert-rule AST rows (<see cref="ASTModel.ExpertRuleLine"/> with non-zero <see cref="ASTModel.ExpertRuleId"/>)
        /// are skipped when the rule is missing or not enabled in <c>expertrule.enabled</c> (logged at Information level).
        /// Optional <see cref="ASTUpdateEventModel.ExpertRuleEvalContext"/> is upserted into <c>cultureastexpertruleevalcontext</c>
        /// (suppressed manual rows for expert display only; not written to the <c>AST</c> table).
        /// When <c>culture.specimenorganismid</c> is null, organism id is treated as 0 and automatic
        /// <c>AppliedBreakpointId</c> / special-row breakpoint resolution is skipped; manual susceptibilities still persist.
        /// </summary>
        /// <param name="dataToSave">JSON payload containing AST craft data including CompletedDate, CompletedTime, and ASTResults.</param>
        /// <param name="username">Username for audit fields.</param>
        /// <returns>The culture ID on success; 0 on failure.</returns>
        public async Task<int> UpdateASTAsync(string dataToSave, string username)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<ASTCraftedModel>>(dataToSave);
                var ASTData = data.Crafted[0].Contents[0].Value;
                var returnId = 0;
                var cultureId = int.Parse(data.Id);
                List<AstSusceptibilityOverrideDataModel> overridesToSync = null;

                _logger.LogInformation(
                    "AST save: CultureId={CultureId}, CompletedDate={CompletedDate}, CompletedTime={CompletedTime}, ASTResultsCount={ASTResultsCount}",
                    cultureId, ASTData.CompletedDate ?? "(empty)", ASTData.CompletedTime ?? "(empty)", ASTData.ASTResults?.Count ?? 0);

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                    {
                        var Id = cultureId;
                        var parameters = new { CultureId = Id };

                        var sql = "select id from ast where cultureid = @cultureId";
                        var ASTIds = await connect.QueryAsync<int>(sql, parameters);
                        var ASTIdList = ASTIds.ToList();

                        foreach (var id in ASTIdList)
                        {
                            sql = @"delete from specialastrow where astid = @astId";
                            await connect.ExecuteAsync(sql, new { astid = id });
                        }

                        sql = @"delete from ast where cultureid = @cultureId";
                        await connect.ExecuteAsync(sql, parameters);


                        sql = @"select specimenid from culture where id = @CultureId";
                        var specimenId = await connect.QueryAsync<int>(sql, parameters);
                        var specimenIdToUse = specimenId.First();

                        sql = @"select Id, CultureId, CommentTypeId, CannedCommentId, FieldId
                            from SpecimenComment where CultureId = @cultureId and (FieldId = 'astcommentoneid' or FieldId = 'astcommenttwoid' or FieldId = 'astadditionalnotes')";

                        var ASTCommentResults = await connect.QueryAsync<CommentModel>(sql, parameters);

                        var dbComment1 = ASTCommentResults.FirstOrDefault(p => p.FieldId == "astcommentoneid");
                        var dbComment1Id = dbComment1 != null ? dbComment1.Id : 0;

                        var dbComment2 = ASTCommentResults.FirstOrDefault(p => p.FieldId == "astcommenttwoid");
                        var dbComment2Id = dbComment2 != null ? dbComment2.Id : 0;

                        var dbComment3 = ASTCommentResults.FirstOrDefault(p => p.FieldId == "astadditionalnotes");
                        var dbComment3Id = dbComment3 != null ? dbComment3.Id : 0;

                        var commentParameters = new
                        {
                            Id = Id,
                            ASTAdditionalNotes = ASTData.ASTAdditionalNotes,
                            ASTCommentOneId = ASTData.ASTCommentOne != null && ASTData.ASTCommentOne != "" ? int.Parse(ASTData.ASTCommentOne) : 0,
                            ASTCommentTwoId = ASTData.ASTCommentTwo != null && ASTData.ASTCommentTwo != "" ? int.Parse(ASTData.ASTCommentTwo) : 0,
                            dbComment1Id = dbComment1Id,
                            dbComment2Id = dbComment2Id,
                            dbComment3Id = dbComment3Id,
                            specimenId = specimenIdToUse,
                            username = username,
                        };

                        if (ASTData.ASTCommentOne != "" && ASTData.ASTCommentOne != null)
                        {
                            if (dbComment1 != null)
                            {
                                sql = @"update SpecimenComment set cannedcommentid = @ASTCommentOneId, lastmodifieddate = now() where id = @dbComment1Id";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                            else
                            {
                                sql = @"insert into SpecimenComment(specimenid, lastmodifieddate, commenttypeid, displayonreport, cultureid, 
                                        addedby, cannedcommentid, fieldid)
                                        values (@SpecimenId, now(), 1233, 'Yes', @Id, @username, @ASTCommentOneId, 'astcommentoneid')";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                        }
                        else
                        {
                            if (dbComment1 != null)
                            {
                                sql = @"delete from SpecimenComment where id = @dbComment1Id";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                        }

                        if ((ASTData.ASTCommentTwo != "" && ASTData.ASTCommentTwo != null))
                        {
                            var comment = ASTCommentResults.FirstOrDefault(p => p.FieldId == "astcommenttwoid");
                            if (comment != null)
                            {
                                sql = @"update SpecimenComment set cannedcommentid = @ASTCommentTwoId, lastmodifieddate = now() where id = @dbComment2Id";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                            else
                            {
                                sql = @"insert into SpecimenComment(specimenid, lastmodifieddate, commenttypeid, displayonreport, cultureid, 
                                        addedby, cannedcommentid, fieldid)
                                        values (@SpecimenId, now(), 1233, 'Yes', @Id, @username, @ASTCommentTwoId, 'astcommenttwoid') ";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                        }
                        else
                        {
                            if (dbComment2 != null)
                            {
                                sql = @"delete from SpecimenComment where id = @dbComment2Id";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                        }

                        if ((ASTData.ASTAdditionalNotes != "" && ASTData.ASTAdditionalNotes != null))
                        {
                            var comment = ASTCommentResults.FirstOrDefault(p => p.FieldId == "astadditionalnotes");
                            if (comment != null)
                            {
                                sql = @"update SpecimenComment set comment = @ASTAdditionalNotes, lastmodifieddate = now() where id = @dbComment3Id";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                            else
                            {
                                sql = @"insert into SpecimenComment(specimenid, comment, lastmodifieddate, commenttypeid, displayonreport, cultureid, 
                                        addedby, fieldid)
                                        values (@SpecimenId, @ASTAdditionalNotes, now(), 1233, 'Yes', @Id, @username, 'astadditionalnotes') ";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                        }
                        else
                        {
                            if (dbComment3 != null)
                            {
                                sql = @"delete from SpecimenComment where id = @dbComment3Id";
                                await connect.ExecuteAsync(sql, commentParameters);
                            }
                        }

                        object astCompletedDateParam = DBNull.Value;
                        if (!string.IsNullOrEmpty(ASTData.CompletedDate) && DateTime.TryParse(ASTData.CompletedDate, out var parsedDate))
                        {
                            astCompletedDateParam = parsedDate.Date;
                        }
                        var astCompletedTimeParam = string.IsNullOrEmpty(ASTData.CompletedTime) ? (object)DBNull.Value : ASTData.CompletedTime;
                        sql = @"update Culture set ASTCompletedDate = @ASTCompletedDate, ASTCompletedTime = @ASTCompletedTime, lastmodifieddate = now() where Id = @CultureId";
                        await connect.ExecuteAsync(sql, new { ASTCompletedDate = astCompletedDateParam, ASTCompletedTime = astCompletedTimeParam, CultureId = Id });
                        _logger.LogInformation(
                            "AST save: CultureId={CultureId}, ASTCompletedDate={ASTCompletedDate}, ASTCompletedTime={ASTCompletedTime} persisted to Culture",
                            Id, ASTData.CompletedDate ?? "(empty)", ASTData.CompletedTime ?? "(empty)");

                        var expertRuleIdsInPayload = ASTData.ASTResults
                            .Where(e => e.ExpertRuleLine && e.ExpertRuleId > 0)
                            .Select(e => e.ExpertRuleId)
                            .Distinct()
                            .ToArray();
                        HashSet<int> enabledExpertRuleIds = null;
                        if (expertRuleIdsInPayload.Length > 0)
                        {
                            const string enabledExpertSql = "select id from expertrule where id = any(@expertRuleIdsInPayload) and enabled = 'Yes'";
                            var enabledIds = await connect.QueryAsync<int>(enabledExpertSql, new { expertRuleIdsInPayload });
                            enabledExpertRuleIds = enabledIds.ToHashSet();
                        }

                        var skippedExpertDisabled = 0;
                        var insertedAstRowCount = 0;
                        var appliedBreakpointRowCount = 0;

                        var nullableOrganismId = await connect.QueryFirstOrDefaultAsync<int?>(
                            "select specimenorganismid from culture where id = @CultureId",
                            new { CultureId = Id });
                        var organismId = nullableOrganismId ?? 0;
                        if (organismId == 0 && ASTData.ASTResults?.Count > 0)
                        {
                            _logger.LogInformation(
                                "AST save: CultureId={CultureId}, SpecimenOrganismId missing; breakpoint auto-resolution skipped, ASTResultsCount={ASTResultsCount}",
                                Id, ASTData.ASTResults.Count);
                        }

                        foreach (var ASTEntry in ASTData.ASTResults)
                        {
                            if (!ASTEntry.TryGetTestMethodId(out var testMethodId) ||
                                !ASTEntry.TryGetAntibioticId(out var antibioticId))
                            {
                                _logger.LogWarning(
                                    "AST save skipped row with unresolved ids: CultureId={CultureId}, TestMethod={TestMethod}, Antibiotic={Antibiotic}, TestMethodId={TestMethodId}, AntibioticId={AntibioticId}",
                                    Id,
                                    ASTEntry?.TestMethod ?? "(null)",
                                    ASTEntry?.Antibiotic ?? "(null)",
                                    ASTEntry?.TestMethodId ?? 0,
                                    ASTEntry?.AntibioticId ?? 0);
                                continue;
                            }

                            var guidelinesId = ASTEntry.GuidelinesId;
                            if (guidelinesId == 0 &&
                                !string.IsNullOrWhiteSpace(ASTEntry.Guidelines) &&
                                int.TryParse(ASTEntry.Guidelines, out var parsedGuidelines))
                            {
                                guidelinesId = parsedGuidelines;
                            }

                            var dosage = ASTEntry.Dosage;
                            var measurementValue = MicMeasurementExtensions.ParseMeasurementNumericForLookup(ASTEntry.Measurement);
                            if (!MicMeasurementExtensions.TryParseMicMeasurement(ASTEntry.Measurement, out var parsedMeasurement)
                                || (!parsedMeasurement.IsBlank && !parsedMeasurement.HasNumericValue))
                            {
                                _logger.LogWarning(
                                    "AST save unparseable measurement reached persist: CultureId={CultureId}, TestMethodId={TestMethodId}, Measurement={Measurement}",
                                    Id, testMethodId, ASTEntry.Measurement ?? "(null)");
                            }

                            var persistMeasurement = MicMeasurementExtensions.ParseMicNumericForPersist(ASTEntry.Measurement);
                            var micComparison = MicMeasurementExtensions.GetMicComparisonOperator(ASTEntry.Measurement);

                            if (testMethodId == 680 && !string.IsNullOrEmpty(micComparison))
                            {
                                _logger.LogInformation(
                                    "AST MIC persist with comparison operator: CultureId={CultureId}, AntibioticId={AntibioticId}, MicComparison={MicComparison}, Measurement={Measurement}",
                                    Id, antibioticId, micComparison, persistMeasurement);
                            }

                            var appliedBreakpointId = ASTEntry.AppliedBreakpointId;
                            if (!ASTEntry.ExpertRuleLine && appliedBreakpointId == 0 && organismId != 0 && guidelinesId != 0 && antibioticId != 0)
                            {
                                appliedBreakpointId = await ResolveAppliedBreakpointIdAsync(
                                    organismId,
                                    antibioticId,
                                    guidelinesId,
                                    testMethodId,
                                    dosage,
                                    0,
                                    measurementValue);
                            }

                            ASTEntry.TryGetSusceptibilityId(out var susceptibilityId);
                            var categoryId = ASTEntry.CategoryId;
                            if (categoryId == 0 &&
                                !string.IsNullOrWhiteSpace(ASTEntry.Category) &&
                                int.TryParse(ASTEntry.Category, out var parsedCategory))
                            {
                                categoryId = parsedCategory;
                            }

                            var parameters3 = new
                            {
                                CultureId = Id,
                                TestType = ASTEntry.TestType,
                                TestMethodId = testMethodId,
                                EntryType = ASTEntry.EntryType,
                                AntibioticId = antibioticId,
                                Dosage = ASTEntry.Dosage,
                                GuidelinesId = guidelinesId,
                                Measurement = persistMeasurement,
                                MicComparison = micComparison,
                                SusceptibilityId = susceptibilityId,
                                AppliedBreakpointId = appliedBreakpointId,
                                CategoryId = categoryId,
                                DisplayOnReport = ASTEntry.IncludeInReport,
                                ExpertRuleLine = ASTEntry.ExpertRuleLine,
                                ExpertRuleId = ASTEntry.ExpertRuleId 
                            };

                            if (ASTData.DeleteBlankRows == "Yes" && parameters3.Measurement == -1 && parameters3.SusceptibilityId == 0)
                            {
                                if (ASTEntry.SpecialRows != null && ASTEntry.SpecialRows.Count > 0)
                                {
                                    var blank = true;
                                    foreach (var row in ASTEntry.SpecialRows)
                                    {
                                        if (ResolveSpecialRowSusceptibilityId(row) != 0)
                                        {
                                            blank = false;
                                            break;
                                        }
                                    }
                                    if (blank) { continue; }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (ASTEntry.ExpertRuleLine && ASTEntry.ExpertRuleId > 0)
                            {
                                if (enabledExpertRuleIds == null || !enabledExpertRuleIds.Contains(ASTEntry.ExpertRuleId))
                                {
                                    skippedExpertDisabled++;
                                    _logger.LogInformation(
                                        "AST save skipped expert AST row: CultureId={CultureId}, ExpertRuleId={ExpertRuleId}, reason=disabled_or_missing",
                                        Id, ASTEntry.ExpertRuleId);
                                    continue;
                                }
                            }

                            insertedAstRowCount++;
                            if (appliedBreakpointId != 0)
                            {
                                appliedBreakpointRowCount++;
                                _logger.LogInformation(
                                    "AST save: CultureId={CultureId}, AntibioticId={AntibioticId}, AppliedBreakpointId={AppliedBreakpointId}",
                                    Id, antibioticId, appliedBreakpointId);
                            }

                            sql = @"insert into AST(cultureid, testtype, testmethodid, entrytype, antibioticid, dosage, guidelinesid, measurement, susceptibilityid, appliedbreakpointid, categoryid, displayonreport, miccomparison, lastmodifieddate, expertruleline, expertruleid)
                                             values(@CultureId, @TestType, @TestMethodId, @EntryType, @AntibioticId, @Dosage, @GuidelinesId, @Measurement, @SusceptibilityId, @AppliedBreakpointId, @CategoryId, @DisplayOnReport, @MicComparison, now(), @ExpertRuleLine, @ExpertRuleId) returning id";

                            var returnValue = await connect.QueryAsync(sql, parameters3);
                            //await connect.ExecuteAsync(sql, parameters3);
                            returnId = returnValue.First().id;

                            var ASTId = returnId;

                            if (ASTEntry.SpecialRows != null && ASTEntry.SpecialRows.Count > 0)
                            {
                                foreach (var row in ASTEntry.SpecialRows)
                                {
                                    var specialBreakpointId = row.BreakpointId;
                                    if (specialBreakpointId == 0 && organismId != 0 && guidelinesId != 0 && antibioticId != 0)
                                    {
                                        specialBreakpointId = await ResolveAppliedBreakpointIdAsync(
                                            organismId,
                                            antibioticId,
                                            guidelinesId,
                                            testMethodId,
                                            dosage,
                                            row.SpecialTypeId,
                                            measurementValue);
                                    }

                                    var parameters4 = new
                                    {
                                        ASTId = ASTId,
                                        SpecialTypeId = row.SpecialTypeId,
                                        SusceptibilityId = ResolveSpecialRowSusceptibilityId(row),
                                        DisplayOnReport = row.IncludeInReport,
                                        BreakpointId = specialBreakpointId
                                    };
                                    sql = @"insert into specialastrow(astid, specialtypeid, susceptibilityid, displayonreport, breakpointid, lastmodifieddate)
                                            values(@ASTId, @SpecialTypeId, @SusceptibilityId, @DisplayOnReport, @BreakpointId, now()) returning id";

                                    returnValue = await connect.QueryAsync(sql, parameters4);
                                    if (specialBreakpointId != 0)
                                    {
                                        _logger.LogInformation(
                                            "AST save: CultureId={CultureId}, AntibioticId={AntibioticId}, SpecialTypeId={SpecialTypeId}, BreakpointId={BreakpointId}",
                                            Id, antibioticId, row.SpecialTypeId, specialBreakpointId);
                                    }
                                    //returnId = returnValue.First().id;
                                }
                            }
                        }

                        _logger.LogInformation(
                            "AST save: CultureId={CultureId}, astRowsInserted={Inserted}, astRowsWithAppliedBreakpoint={AppliedBreakpointCount}, expertAstRowsSkippedDisabled={Skipped}",
                            Id, insertedAstRowCount, appliedBreakpointRowCount, skippedExpertDisabled);

                        var astTyped = data.Crafted[0].Contents[0].Value as ASTUpdateEventModel;
                        var expertRuleEvalCtx = astTyped?.ExpertRuleEvalContext;
                        var hasEvalCtx =
                            expertRuleEvalCtx != null &&
                            ((expertRuleEvalCtx.DiskResults?.Count ?? 0) > 0 ||
                             (expertRuleEvalCtx.MicResults?.Count ?? 0) > 0);
                        if (hasEvalCtx)
                        {
                            var payloadJson = JsonConvert.SerializeObject(expertRuleEvalCtx);
                            sql = @"insert into cultureastexpertruleevalcontext (cultureid, payload, lastmodifieddate)
                                    values (@cultureId, @payload::jsonb, now())
                                    on conflict (cultureid) do update set payload = excluded.payload, lastmodifieddate = now()";
                            await connect.ExecuteAsync(sql, new { cultureId = Id, payload = payloadJson });
                            _logger.LogInformation(
                                "AST save: CultureId={CultureId}, ExpertRuleEvalContext persisted (DiskRows={Disk}, MicRows={Mic})",
                                Id,
                                expertRuleEvalCtx!.DiskResults?.Count ?? 0,
                                expertRuleEvalCtx.MicResults?.Count ?? 0);
                        }
                        else
                        {
                            sql = @"delete from cultureastexpertruleevalcontext where cultureid = @cultureId";
                            var removed = await connect.ExecuteAsync(sql, new { cultureId = Id });
                            if (removed > 0)
                            {
                                _logger.LogInformation(
                                    "AST save: CultureId={CultureId}, ExpertRuleEvalContext removed (no suppressed manual rows in payload)",
                                    Id);
                            }
                        }

                        overridesToSync = AstSusceptibilityOverrideCollector.CollectFromSavePayload(
                            Id,
                            ASTData.ASTResults,
                            username);
                    }
                    scope.Complete();
                }

                await _overrideRepository.SyncForCultureAsync(
                    cultureId,
                    overridesToSync ?? new List<AstSusceptibilityOverrideDataModel>(),
                    username);
                return returnId;
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogError("Update AST transaction aborted : {0}", ex.Message);
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError("Update AST sql execution error : {0}", e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Retrieves AST test results for a culture, including expert rule metadata (ExpertRuleId, ExpertRuleLine)
        /// and rule details (ExpertRuleName, ExpertRuleText) when the row was generated by an expert rule.
        /// </summary>
        /// <param name="id">The culture ID.</param>
        /// <returns>List of AST results with special rows and expert rule information.</returns>
        public async Task<List<ASTModel>> GetASTTestResultsAsync(string id)
        {
            using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
            {
                var sql = @"select a.Id, a.TestType, a.TestMethodId, a.EntryType, a.AntibioticId, a.Dosage, a.GuidelinesId, a.Measurement, a.SusceptibilityId, a.AppliedBreakpointId, a.MicComparison, a.CategoryId, a.DisplayOnReport AS IncludeInReport, a.ExpertRuleId, (a.ExpertRuleId IS NOT NULL AND a.ExpertRuleId != 0) as ExpertRuleLine from AST a where CultureId = @Id order by Id";
                var ASTResult = await connect.QueryAsync<ASTModel>(sql, new { Id = int.Parse(id) });

                foreach (var item in ASTResult)
                {
                    var temp = item.Measurement.Split('.');
                    item.Measurement = temp[1] == "000" ? item.Measurement.Remove(item.Measurement.Length - 4) : item.Measurement;
                    var comp = item.MicComparison == null ? "" : item.MicComparison.Trim();
                    item.Measurement = comp + item.Measurement.Trim();
                    item.TestMethod = item.TestMethodId.ToString();
                    item.Antibiotic = item.AntibioticId.ToString();
                    item.Guidelines = item.GuidelinesId.ToString();
                    item.Susceptibility = item.SusceptibilityId.ToString();
                    item.Category = item.CategoryId.ToString();
                    item.SpecialRows = new List<ASTSpecialRowModel>();

                    sql = @"select li.value as name, s.specialtypeid, s.SusceptibilityId, s.DisplayOnReport as IncludeInReport, s.breakpointid as BreakpointId from specialastrow s
                            inner join ListItem li on li.id = s.specialtypeid where ASTId = " + item.Id;
                    var result = await connect.QueryAsync<ASTSpecialRowModel>(sql);
                    var specialRows = result.ToList();
                    if (specialRows.Count > 0)
                    {
                        foreach (var row in specialRows)
                        {
                            var rowToInclude = new ASTSpecialRowModel
                            {
                                Name = row.Name,
                                SpecialTypeId = row.SpecialTypeId,
                                SusceptibilityId = row.SusceptibilityId,
                                Susceptibility = row.SusceptibilityId.ToString(),
                                IncludeInReport = row.IncludeInReport ?? "No",
                                BreakpointId = row.BreakpointId
                            };
                            item.SpecialRows.Add(rowToInclude);
                        }
                    }
                    else
                    {
                        item.SpecialRows = null;
                    }

                    //if (item.ExpertRuleLine == true && item.ExpertRuleId != 0)
                    //{
                    //    sql = @"select * from expertrule where Id = @ExpertRuleId";
                    //    var ruleResultList = await connect.QueryAsync<ExpertRuleDetailsModel>(sql, new { ExpertRuleId = item.ExpertRuleId });
                    //    var ruleResult = ruleResultList.FirstOrDefault();

                    //    if (ruleResult != null)
                    //    {
                    //        item.ExpertRuleName = ruleResult.ExpertRuleName;
                    //        item.ExpertRuleText = ruleResult.RuleText;
                    //    }
                    //}

                }
                return ASTResult.ToList();
            }
        }

        /// <summary>
        /// Resolves special-consideration susceptibility id from numeric JSON fields or string payload values.
        /// </summary>
        private static int ResolveSpecialRowSusceptibilityId(ASTSpecialRowModel row)
        {
            if (row == null)
            {
                return 0;
            }

            if (row.SusceptibilityId > 0)
            {
                return row.SusceptibilityId;
            }

            if (!string.IsNullOrWhiteSpace(row.Susceptibility) &&
                int.TryParse(row.Susceptibility, out var parsed) &&
                parsed > 0)
            {
                return parsed;
            }

            return 0;
        }

        /// <summary>
        /// Resolves the winning enabled breakpoint id for AST persistence when the client did not supply one.
        /// Matching uses ids only (organism, antibiotic, guideline, test method, dosage, special consideration, measurement).
        /// </summary>
        private async Task<int> ResolveAppliedBreakpointIdAsync(
            int organismId,
            int antibioticId,
            int guidelinesId,
            int testMethodId,
            int dosage,
            int specialConsiderationId,
            decimal measurement)
        {
            if (organismId == 0 || antibioticId == 0 || guidelinesId == 0)
            {
                return 0;
            }

            const int micTestMethodId = 680;
            const int noSpecialConsiderationId = 973;

            var parameters = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig>
                {
                    new QueryValuesConfig { Key = "OrganismId", Value = organismId.ToString() },
                    new QueryValuesConfig { Key = "AntibioticId", Value = antibioticId.ToString() },
                    new QueryValuesConfig { Key = "TestMethodId", Value = testMethodId.ToString() },
                    new QueryValuesConfig { Key = "SourceId", Value = guidelinesId.ToString() },
                    new QueryValuesConfig { Key = "Dosage", Value = dosage.ToString() }
                }
            };

            if (measurement != 0)
            {
                if (testMethodId == micTestMethodId)
                {
                    parameters.Parameters.Add(new QueryValuesConfig { Key = "Mic", Value = measurement.ToString() });
                }
                else
                {
                    parameters.Parameters.Add(new QueryValuesConfig { Key = "ZoneDiameter", Value = ((int)measurement).ToString() });
                }
            }

            var susceptibilities = await RetrieveSusceptibilitiesAsync(parameters);
            if (susceptibilities == null || susceptibilities.Count == 0)
            {
                return 0;
            }

            var lookupId = specialConsiderationId == 0 ? noSpecialConsiderationId : specialConsiderationId;
            var match = susceptibilities.FirstOrDefault(s => s.SpecialConsiderationId == lookupId);
            if (match == null && specialConsiderationId == 0)
            {
                match = susceptibilities.FirstOrDefault(s => s.SpecialConsiderationId == 0);
            }

            return match?.BreakpointId ?? 0;
        }

        public async Task<SpecimenPatientModel> GetSpecimenAndPatientAsync(int id)
        {
            if (id == 0) return null;
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var sql = @"select s.id As SpecimenId, patientid, accessionnumber from specimen s
                                inner join culture c on s.id = c.specimenid
                                inner join ast a on c.id = a.cultureid
                                where a.id = @Id;";

                    return await connect.QueryFirstAsync<SpecimenPatientModel>(sql, new { Id = id });
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Getting single ast record caused ngSQL exception : {0}", e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<List<AntibioticListForReportModel>> GetAstAntibioticListAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new AntibioticListForSpecimenReportQuery(), "Get antibiotic list for culture", queryFilters);
        }

        public async Task<List<ASTListByCultureIdModel>> GetAstListByCultureIdAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new ASTListByCultureIdQuery(), "Get ast list for culture", queryFilters);
        }

        private async Task<string> GetPrintOnReportAsync(NpgsqlConnection connect, int cultureId, string testName)
        {
            var sql = "select testresults from culturetests where cultureid = @CultureId and testname = @TestName";
            var results = await connect.QueryAsync<string>(sql, new { CultureId = cultureId, TestName = testName });
            if (results.Count() > 0 && results.ToList()[0] != null)
            {
                var testResults = JsonConvert.DeserializeObject<PrintOnReportModel>(results.ToList()[0]);
                return testResults.PrintOnReport;
            }
            return null;
        }

        private async Task<int> AddOrUpdateCultureTestAsync(NpgsqlConnection connect, int cultureId, TestListResultModel test, bool valueEntered)
        {

            // Check for existing test instance in culturetest table and create or update accordingly.
            var sql = "select id from culturetests where cultureid = @CultureId and testname = @TestName";
            var testIds = await connect.QueryAsync<int>(sql, new { CultureId = cultureId, TestName = test.TestName });

            // Don't create or update an entry if a value hasn't been selected and there is no test result.
            if (!valueEntered && testIds.Count() == 0) return 0;

            // Create a new entry if a value has been selected and there is no result.
            else if (testIds.Count() == 0 && valueEntered)
            {
                sql = @"insert into CultureTests(cultureid, testname, testresults, status, requested, completed, lastmodifieddate)
                                          values(@CultureId, @TestName, to_json(@TestResults::jsonb), @Status, now(), now(), now())";

                await connect.ExecuteAsync(sql, new { CultureId = cultureId, TestName = test.TestName, TestResults = test.TestResults, Status = "Complete" });
                _logger.LogInformation("Culture test saved from AST: CultureId={CultureId}, TestName={TestName}, Status=Complete, action=insert", cultureId, test.TestName);
            }
            // Update the status to requested and remove the result if the value has been nulled out.
            else if (!valueEntered && testIds.Count() > 0)
            {
                var testId = testIds.ToList()[0];
                sql = @"update CultureTests set testresults = null, status = @Status, completed = null, lastmodifieddate = now() where id = @Id";
                await connect.ExecuteAsync(sql, new { Status = "Requested", Id = testId });
                _logger.LogInformation("Culture test saved from AST: CultureId={CultureId}, CultureTestId={CultureTestId}, TestName={TestName}, Status=Requested, action=update", cultureId, testId, test.TestName);
            }
            else
            {
                var testId = testIds.ToList()[0];
                sql = @"update CultureTests set testresults = to_json(@TestResults::jsonb), status = @Status, completed = now(), lastmodifieddate = now() where id = @Id";
                await connect.ExecuteAsync(sql, new { TestResults = test.TestResults, Status = "Complete", Id = testId });
                _logger.LogInformation("Culture test saved from AST: CultureId={CultureId}, CultureTestId={CultureTestId}, TestName={TestName}, Status=Complete, action=update", cultureId, testId, test.TestName);
            }
            return 0;
        }

        public async Task<List<SusceptibilityModel>> RetrieveSusceptibilitiesAsync(QueryFilterConfig parameters)
        {
            var organismId = parameters.Parameters.FirstOrDefault(p => p.Key.Equals("OrganismId", StringComparison.OrdinalIgnoreCase))?.Value ?? "?";
            var antibioticId = parameters.Parameters.FirstOrDefault(p => p.Key.Equals("AntibioticId", StringComparison.OrdinalIgnoreCase))?.Value ?? parameters.Parameters.FirstOrDefault(p => p.Key.Equals("Antibiotic", StringComparison.OrdinalIgnoreCase))?.Value ?? "?";
            var testMethodId = parameters.Parameters.FirstOrDefault(p => p.Key.Equals("TestMethodId", StringComparison.OrdinalIgnoreCase))?.Value ?? parameters.Parameters.FirstOrDefault(p => p.Key.Equals("TestMethod", StringComparison.OrdinalIgnoreCase))?.Value ?? "?";
            var dosage = parameters.Parameters.FirstOrDefault(p => p.Key.Equals("Dosage", StringComparison.OrdinalIgnoreCase))?.Value ?? "?";
            var sourceId = parameters.Parameters.FirstOrDefault(p => p.Key.Equals("SourceId", StringComparison.OrdinalIgnoreCase))?.Value ?? parameters.Parameters.FirstOrDefault(p => p.Key.Equals("Guidelines", StringComparison.OrdinalIgnoreCase))?.Value ?? "?";

            var result = await _sqlQuery.QueryReturningTypeAsync(new SusceptibilityQuery(), "Get susceptibilities", parameters);
            var count = result?.Count ?? 0;

            if (count == 0)
            {
                _logger.LogInformation(
                    "RetrieveSusceptibilities: No breakpoints matched susceptibility criteria. OrganismId={OrganismId}, AntibioticId={AntibioticId}, TestMethodId={TestMethodId}, Dosage={Dosage}, SourceId={SourceId}",
                    organismId, antibioticId, testMethodId, dosage, sourceId);
            }
            else
            {
                _logger.LogInformation(
                    "RetrieveSusceptibilities: OrganismId={OrganismId}, AntibioticId={AntibioticId}, TestMethodId={TestMethodId}, Dosage={Dosage}, SourceId={SourceId}, ResultCount={Count}",
                    organismId, antibioticId, testMethodId, dosage, sourceId, count);
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<List<ResistanceMechanismModel>> GetResistanceMechanismsByCultureIdAsync(int cultureId)
        {
            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            const string sql = @"select drugfamily as DrugFamily, phenotype as PhenoType
                from cultureresistancemechanism where cultureid = @CultureId order by id";
            var rows = await connect.QueryAsync<ResistanceMechanismModel>(sql, new { CultureId = cultureId });
            return rows.ToList();
        }

        /// <inheritdoc />
        public async Task<string> GetOrganismNameByIdAsync(int organismId)
        {
            if (organismId == 0)
            {
                return string.Empty;
            }

            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            const string sql = @"
                select
                    case
                        when os.synonym is null then TRIM(CONCAT(
                            g.name,
                            case when g.name is not null and s.name is null then ' spp.' else '' end,
                            ' ', s.name,
                            ' ', ss.name,
                            ' ', TRIM(se.name)
                        ))
                        else os.synonym
                    end as organismname
                from organism og
                left outer join organismsynonyms os on og.Id = os.organismId and os.PreferredName = true
                left outer join genus g on g.Id = og.genusId
                left outer join species s on s.Id = og.speciesId
                left outer join subspecies ss on ss.Id = og.subspeciesId
                left outer join serotype se on se.Id = og.serotypeId
                where og.Id = @organismId";
            var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { organismId });
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
        }

        /// <inheritdoc />
        public async Task<ExpertRuleEvalContextModel?> GetExpertRuleEvalContextAsync(string cultureId)
        {
            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            const string sql = "select payload::text as payload from cultureastexpertruleevalcontext where cultureid = @cultureId";
            var payload = await connect.QueryFirstOrDefaultAsync<string>(sql, new { cultureId = int.Parse(cultureId) });
            if (string.IsNullOrWhiteSpace(payload))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ExpertRuleEvalContextModel>(payload);
        }

    }
}
