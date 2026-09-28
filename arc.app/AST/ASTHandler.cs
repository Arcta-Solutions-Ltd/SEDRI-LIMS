using arc.app.Common;
using arc.app.Coding;
using arc.app.Configuration;
using arc.app.Laboratory;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common;
using arc.common.Models;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Utils;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.data.model.Laboratory;
using System;

namespace arc.app.AST;

public class ASTHandler : IASTHandler
{
    private const int MicTestMethodId = 680;
    private const int DiskTestMethodId = 681;

    private readonly ISpecimenRepository _specimenRepository;
    private readonly ICultureRepository _cultureRepository;
    private readonly IASTRepository _ASTRepository;
    private readonly ILaboratoryConfigurationHandler _laboratoryConfigurationHandler;
    private readonly ITestPatternRepository _testPatternRepository;
    //private readonly IBreakpointRepository _breakpointRepository;
    private readonly ITestRepository _testRepository;
    private readonly IExpertRulePageDisplayService _expertRulePageDisplayService;
    private readonly IMapWithList<ASTModel, ASTRowModel> _astMapper;
    private readonly IMapWithList<TestPatternLineModel, ASTRowModel> _testPatternLineMapper;
    private readonly ISpecialConsiderationEnricher _specialConsiderationEnricher;
    private readonly IAstSusceptibilityOverrideRepository _overrideRepository;
    private readonly IGeneralRepository _generalRepository;
    private readonly ICopyProperties _copyProperties;
    private readonly ILogger<ASTHandler> _logger;

    public ASTHandler(ISpecimenRepository specimenRepository, ICultureRepository cultureRepository, IASTRepository ASTRepository, ITestPatternRepository testPatternRepository, ITestRepository testRepository,
        IExpertRulePageDisplayService expertRulePageDisplayService, IMapWithList<ASTModel, ASTRowModel> astMapper, IMapWithList<TestPatternLineModel, ASTRowModel> testPatternLineMapper,
        ISpecialConsiderationEnricher specialConsiderationEnricher, ILaboratoryConfigurationHandler laboratoryConfigurationHandler, ICopyProperties copyProperties,
        IAstSusceptibilityOverrideRepository overrideRepository, IGeneralRepository generalRepository, ILogger<ASTHandler> logger)
    {
        _specimenRepository = specimenRepository;
        _cultureRepository = cultureRepository;
        _ASTRepository = ASTRepository;
        _laboratoryConfigurationHandler = laboratoryConfigurationHandler;
        _testPatternRepository = testPatternRepository;
        //_breakpointRepository = breakpointRepository;
        _testRepository = testRepository;
        _expertRulePageDisplayService = expertRulePageDisplayService;
        _astMapper = astMapper;
        _testPatternLineMapper = testPatternLineMapper;
        _specialConsiderationEnricher = specialConsiderationEnricher;
        _overrideRepository = overrideRepository;
        _generalRepository = generalRepository;
        _copyProperties = copyProperties;
        _logger = logger;
    }

    /// <summary>
    /// Loads AST form data for a culture. When existing AST entries exist, DiskResults contains only entries with TestType "disk";
    /// MicResults contains only entries with TestType "strip". When no entries exist, loads the applicable test pattern and
    /// populates both sections from pattern lines (with breakpoints and expert rules).
    /// When <paramref name="queryFilters"/> includes <c>testpatternid</c>, loads the chosen pattern's lines and merges
    /// any persisted manual results onto matching lines by antibiotic, test method, guidelines, and dosage ids.
    /// CompletedDate and CompletedTime are loaded from Culture.ASTCompletedDate and Culture.ASTCompletedTime when present;
    /// the frontend defaults to current date/time when both are empty.
    /// </summary>
    public async Task<string> GetASTDataForCultureAsync(QueryFilterConfig queryFilters)
    {
        // Get all required AST data on entry to AST form.

        var cultureId = queryFilters.GetStringValue("id");
        var testPatternId = queryFilters.GetStringValue("testpatternid");

        // Extract relevant details from culture, including organism & specimen type.
        var cultureDetailsRetriever = new CultureDetailsRetriever(_cultureRepository, _specimenRepository, _testRepository);
        var cultureDetails = await cultureDetailsRetriever.Get(cultureId);

        var astQueryResponse = new ASTQueryModel();
        _copyProperties.CopyAll(cultureDetails, astQueryResponse);
        astQueryResponse.CultureId = int.Parse(cultureId);
        astQueryResponse.OrganismName = await _ASTRepository.GetOrganismNameByIdAsync(cultureDetails.OrganismId);

        var applicableIsolateTests = await _laboratoryConfigurationHandler.GetApplicableIsolateTestsForCultureAsync(cultureDetails.LaboratoryId, cultureDetails.CultureTypeId, cultureDetails.OrganismId, cultureDetails.OrgGroupCodingId);
        astQueryResponse.ApplicableIsolateTests = applicableIsolateTests?.ToList();

        // Get all applicable test patterns.
        var testPatternRepository = new TestPatternOptionsRetriever(_testPatternRepository);
        var testPatternOptions = await testPatternRepository.GetTestPatternOptionsForCulture(cultureDetails);
        astQueryResponse.TestPatternOptions = testPatternOptions;

        // Get existing AST test results (if any) with breakpoints.
        var astRows = new List<ASTRowModel>();
        var astEntriesFromDb = await _ASTRepository.GetASTTestResultsAsync(cultureId);
        var existingAstRows = _astMapper.MapList(astEntriesFromDb);

        var persistedAstRowCount = astEntriesFromDb?.Count ?? 0;
        var savedRuleIdsFromPersistence = existingAstRows
            .Where(r => r.ExpertRuleId != 0)
            .Select(r => r.ExpertRuleId)
            .Distinct()
            .ToHashSet();

        if (testPatternId != null)
        {
            var patternId = int.Parse(testPatternId);
            if (patternId != 0)
            {
                var testPatternLines = await GetTestPatternLinesAsync(patternId.ToString());
                var patternRows = _testPatternLineMapper.MapList(testPatternLines);
                var (mergedRows, patternMergeStats) = AstTestPatternResultMerger.MergeExistingOntoPatternLines(
                    patternRows,
                    existingAstRows);

                astRows = mergedRows;

                _logger.LogInformation(
                    "GetASTDataForCulture test pattern result merge: CultureId={CultureId}, TestPatternId={TestPatternId}, PatternLines={PatternLineCount}, ExistingRows={ExistingRowCount}, Merged={MergedCount}, UnmatchedExisting={UnmatchedExistingCount}",
                    cultureId,
                    patternId,
                    patternMergeStats.PatternLineCount,
                    patternMergeStats.ExistingRowCount,
                    patternMergeStats.MergedCount,
                    patternMergeStats.UnmatchedExistingCount);

                foreach (var row in mergedRows.Where(r => r.IsMergeableManualLine()))
                {
                    var key = row.BuildLineMatchKey();
                    if (existingAstRows.Any(e => e.IsMergeableManualLine() && e.BuildLineMatchKey() == key))
                    {
                        _logger.LogDebug(
                            "GetASTDataForCulture test pattern result merge line: CultureId={CultureId}, TestPatternId={TestPatternId}, TestMethodId={TestMethodId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, Dosage={Dosage}",
                            cultureId,
                            patternId,
                            row.TestMethod,
                            row.Antibiotic,
                            row.Guidelines,
                            row.Dosage);
                    }
                }
            }
        }
        else
        {
            astRows = existingAstRows;

            if (astRows.Count == 0)
            {
                var patternId = 0;
                if (testPatternOptions.Count() > 0)
                {
                    var selectionResult = TestPatternDefaultSelector.SelectDefault(testPatternOptions);
                    patternId = selectionResult.Selected?.Id ?? 0;
                    astQueryResponse.SelectedTestPatternId = patternId;

                    LogTestPatternSelection(cultureId, testPatternOptions, selectionResult);
                }

                if (patternId != 0)
                {
                    var testPatternLines = await GetTestPatternLinesAsync(patternId.ToString());
                    astRows = _testPatternLineMapper.MapList(testPatternLines);
                }
            }
        }

        // Conditionally add the ast rows to the return model
        astQueryResponse.MicResults.AddRange(astRows.Where(r => r.TestMethod == 680));
        astQueryResponse.DiskResults.AddRange(astRows.Where(r => r.TestMethod == 681));

        var expertEvalCtx = await _ASTRepository.GetExpertRuleEvalContextAsync(cultureId);
        var mergeStats = ExpertRuleEvalContextMerge.MergeIntoDiskMicLists(
            astQueryResponse.DiskResults,
            astQueryResponse.MicResults,
            expertEvalCtx);
        if (expertEvalCtx != null)
        {
            _logger.LogInformation(
                "GetASTDataForCulture ExpertRuleEvalContext merge: CultureId={CultureId}, diskAdded={DiskAdded}, diskSkipped={DiskSkipped}, micAdded={MicAdded}, micSkipped={MicSkipped}",
                cultureId,
                mergeStats.diskAdded,
                mergeStats.diskSkipped,
                mergeStats.micAdded,
                mergeStats.micSkipped);
        }

        astQueryResponse.ResistanceMechanisms = await _ASTRepository.GetResistanceMechanismsByCultureIdAsync(astQueryResponse.CultureId);

        var laboratoryRecord = await _generalRepository.GetByIdAsync<LaboratoryDataModel>("laboratory", cultureDetails.LaboratoryId);
        astQueryResponse.RecordSusceptibilityChangeAudit = laboratoryRecord?.RecordSusceptibilityChangeAudit ?? "No";

        var overrideRows = await _overrideRepository.GetByCultureIdAsync(astQueryResponse.CultureId);
        AstSusceptibilityOverrideMerger.MergeOntoAstQuery(astQueryResponse, overrideRows, _logger);

        foreach (var row in astQueryResponse.DiskResults.Concat(astQueryResponse.MicResults))
        {
            row.OrganismId = cultureDetails.OrganismId;
            if (!row.ExpertRuleLine && row.Antibiotic.HasValue && row.Antibiotic.Value != 0 && row.Guidelines != 0)
            {
                row.EmbeddedASTRows = await _specialConsiderationEnricher.EnrichAsync(
                    cultureDetails.OrganismId,
                    row.Antibiotic.Value,
                    row.Guidelines,
                    row.Dosage,
                    row.TestMethod,
                    row.DrugCategory,
                    row.EmbeddedASTRows,
                    cultureId,
                    retainStoredWhenNoMatch: true);
                if (row.EmbeddedASTRows?.Count > 0)
                {
                    _logger.LogDebug(
                        "GetASTDataForCulture special considerations (existing): CultureId={CultureId}, OrganismId={OrganismId}, AntibioticId={AntibioticId}, TestMethodId={TestMethodId}, EmbeddedRowCount={Count}",
                        cultureId, cultureDetails.OrganismId, row.Antibiotic, row.TestMethod, row.EmbeddedASTRows.Count);
                }
            }
        }

        var expertDisplay = await _expertRulePageDisplayService.BuildDisplayAsync(
            cultureId,
            cultureDetails,
            astQueryResponse.DiskResults,
            astQueryResponse.MicResults,
            persistedAstRowCount,
            savedRuleIdsFromPersistence.ToList());
        astQueryResponse.ExpertRuleGroups = expertDisplay.ExpertRuleGroups;
        astQueryResponse.ExpertRuleResults = expertDisplay.ExpertRuleResults;
        astQueryResponse.ExpertRuleCommentAlerts = expertDisplay.ExpertRuleCommentAlerts;

        // Package reply data into crafted structure.
        var AVPairList = new List<JsonFieldModel>();
        AVPairList.Add(new JsonFieldModel { Key = "ast", Value = JsonConvert.SerializeObject(astQueryResponse) });

        var craftedList = new List<CraftedModel>();
        craftedList.Add(new CraftedModel { Name = "ast", Contents = JsonConvert.SerializeObject(AVPairList) });

        var dataToReturn = new CraftedWithIdForEventModel<CraftedModel>();
        dataToReturn.Crafted = craftedList;
        dataToReturn.Id = cultureId;

        return JsonConvert.SerializeObject(dataToReturn);
    }

    /// <summary>
    /// Discovers special-consideration embedded rows from breakpoints (by organism/antibiotic/guideline/dosage ids)
    /// and resolves breakpoint susceptibilities for the main AST row and its embeds when measurement allows.
    /// Expert rule lines are not returned here; use <see cref="GetExpertRulesForPageAsync"/> for the expert rules section.
    /// </summary>
    public async Task<ASTRowModel> GetSusceptibilitiesAndExpertRules(ASTRowModel row)
    {
            if (row.Guidelines == 0)
            {
                return row;
            }

            var embeddedCountBefore = row.EmbeddedASTRows?.Count ?? 0;
            if (ShouldEnrichSpecialConsiderations(row, out var enrichSkipReason))
            {
                var testMethodId = NormalizeTestMethodId(row);
                var antibioticId = row.Antibiotic!.Value;
                row.EmbeddedASTRows = await _specialConsiderationEnricher.EnrichAsync(
                    row.OrganismId,
                    antibioticId,
                    row.Guidelines,
                    row.Dosage,
                    testMethodId,
                    row.DrugCategory,
                    row.EmbeddedASTRows,
                    cultureId: null,
                    retainStoredWhenNoMatch: false);

                var embeddedCountAfter = row.EmbeddedASTRows?.Count ?? 0;
                _logger.LogInformation(
                    "AST GetSusceptibilitiesAndExpertRules special consideration enrich: OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, Dosage={Dosage}, TestMethodId={TestMethodId}, EmbeddedBefore={EmbeddedBefore}, EmbeddedAfter={EmbeddedAfter}",
                    row.OrganismId,
                    antibioticId,
                    row.Guidelines,
                    row.Dosage,
                    testMethodId,
                    embeddedCountBefore,
                    embeddedCountAfter);

                if (embeddedCountAfter == 0)
                {
                    _logger.LogInformation(
                        "AST GetSusceptibilitiesAndExpertRules: No special consideration breakpoints matched OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, Dosage={Dosage}, TestMethodId={TestMethodId}",
                        row.OrganismId,
                        antibioticId,
                        row.Guidelines,
                        row.Dosage,
                        testMethodId);
                }
            }
            else
            {
                _logger.LogDebug(
                    "AST GetSusceptibilitiesAndExpertRules: Skipped special consideration enrich — {Reason}",
                    enrichSkipReason);
            }

            var queryFilter = new QueryFilterConfig
            {
                Parameters = row.GetQueryValues().Select(kvp => new QueryValuesConfig
                {
                    Key = kvp.Key,
                    Value = kvp.Value
                }).ToList()
            };

            var hasMeasurement = (row.ZoneDiameter.HasValue && row.ZoneDiameter.Value != 0)
                || (row.Mic.HasValue && row.Mic.Value != 0);
            if (hasMeasurement && (row.ForceRecalculateSusceptibility || !row.IsManuallySetSusceptibility()))
            {
                ClearSusceptibilityBeforeLookup(row);
            }

            // Retrieve susceptibilities
            var susceptibilities = await GetSusceptibilitiesAsync(queryFilter);

            if (susceptibilities != null && susceptibilities.Any())
            {
                var susceptibilityByKey = susceptibilities
                    .GroupBy(s => ((int)(s.AntibioticId ?? 0), s.SpecialConsiderationId))
                    .ToDictionary(g => g.Key, g => g.First());

                var allRows = new List<ASTRowModel> { row };
                row.EmbeddedASTRows ??= new List<ASTRowModel>();
                allRows.AddRange(row.EmbeddedASTRows);

                foreach (var r in allRows)
                {
                    if (!row.ForceRecalculateSusceptibility && r.IsManuallySetSusceptibility())
                    {
                        _logger.LogInformation(
                            "AST GetSusceptibilitiesAndExpertRules: Preserved manually set susceptibility. OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, TestMethodId={TestMethodId}, SpecialConsiderationId={SpecialConsiderationId}, TestResult={TestResult}",
                            row.OrganismId,
                            r.Antibiotic,
                            row.Guidelines,
                            NormalizeTestMethodId(row),
                            r.SpecialConsiderationId,
                            r.TestResult);
                        continue;
                    }

                    var key = (r.Antibiotic ?? 0, r.SpecialConsiderationId);
                    if (susceptibilityByKey.TryGetValue(key, out var matched))
                    {
                        r.TestResult = matched.SusceptibilityId;
                        r.AppliedBreakpointId = matched.BreakpointId;
                        if (matched.BreakpointId != 0)
                        {
                            _logger.LogInformation(
                                "AST GetSusceptibilitiesAndExpertRules: AppliedBreakpointId={AppliedBreakpointId} for OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, TestMethodId={TestMethodId}, SpecialConsiderationId={SpecialConsiderationId}",
                                matched.BreakpointId,
                                row.OrganismId,
                                r.Antibiotic,
                                row.Guidelines,
                                NormalizeTestMethodId(row),
                                r.SpecialConsiderationId);
                        }
                    }
                    else if (key.Item2 == 0)
                    {
                        const int noSpecialConsiderationId = 973;
                        if (susceptibilityByKey.TryGetValue((key.Item1, noSpecialConsiderationId), out matched))
                        {
                            r.TestResult = matched.SusceptibilityId;
                            r.AppliedBreakpointId = matched.BreakpointId;
                        }
                    }
                }
            }
            else if (row.OrganismId != 0 && ShouldEnrichSpecialConsiderations(row, out _))
            {
                if (hasMeasurement)
                {
                    var testMethodIdForLog = NormalizeTestMethodId(row);
                    _logger.LogWarning(
                        "AST GetSusceptibilitiesAndExpertRules: No breakpoints matched susceptibility lookup with measurement present. OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, Dosage={Dosage}, TestMethodId={TestMethodId}",
                        row.OrganismId,
                        row.Antibiotic,
                        row.Guidelines,
                        row.Dosage,
                        testMethodIdForLog);
                }
            }

            // Update queryFilter.Parameters based on updated TestResult values
            var updatedRows = new List<ASTRowModel> { row };
            row.EmbeddedASTRows ??= new List<ASTRowModel>();
            updatedRows.AddRange(row.EmbeddedASTRows);

            foreach (var r in updatedRows)
            {
                var antibioticId = r.Antibiotic.ToString();
                var specialConsiderationId = r.SpecialConsiderationId.ToString();
                var testResult = r.TestResult.ToString();

                if (!string.IsNullOrEmpty(testResult))
                {
                    var matchingAntibioticParam = queryFilter.Parameters.FirstOrDefault(p => p.Key == "Antibiotic" && p.Value == antibioticId);
                    var matchingSpecialConsiderationParam = queryFilter.Parameters.FirstOrDefault(p => p.Key == "SpecialConsiderationId" && p.Value == specialConsiderationId);

                    if (matchingAntibioticParam != null && matchingSpecialConsiderationParam != null)
                    {
                        var existingTestResultParam = queryFilter.Parameters.FirstOrDefault(p => p.Key == "TestResult");
                        if (existingTestResultParam != null)
                        {
                            existingTestResultParam.Value = testResult;
                        }
                        else
                        {
                            queryFilter.Parameters.Add(new QueryValuesConfig { Key = "TestResult", Value = testResult });
                        }
                    }
                }
            }

        _logger.LogInformation(
            "AST GetSusceptibilitiesAndExpertRules: AntibioticId={AntibioticId}, EmbeddedRowCount={EmbeddedCount}, SusceptibilityRowsApplied={SusApplied}",
            row.Antibiotic,
            row.EmbeddedASTRows?.Count ?? 0,
            susceptibilities?.Count ?? 0);
        return row;
    }

    /// <inheritdoc />
    public async Task<ExpertRulePageDisplayResult> GetExpertRulesForPageAsync(ExpertRulesForPageRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var cultureId = request.CultureId.ToString();
        var cultureDetailsRetriever = new CultureDetailsRetriever(_cultureRepository, _specimenRepository, _testRepository);
        var cultureDetails = await cultureDetailsRetriever.Get(cultureId);

        var astEntriesFromDb = await _ASTRepository.GetASTTestResultsAsync(cultureId);
        var persistedAstRowCount = astEntriesFromDb?.Count ?? 0;
        var astRows = _astMapper.MapList(astEntriesFromDb ?? new List<ASTModel>());
        var savedRuleIdsFromPersistence = astRows
            .Where(r => r.ExpertRuleId != 0)
            .Select(r => r.ExpertRuleId)
            .Distinct()
            .ToList();

        var clientDiskRowCount = request.DiskResults?.Count ?? 0;
        var clientMicRowCount = request.MicResults?.Count ?? 0;

        var diskForRules = request.DiskResults?.ToList() ?? new List<ASTRowModel>();
        var micForRules = request.MicResults?.ToList() ?? new List<ASTRowModel>();
        var expertEvalCtx = await _ASTRepository.GetExpertRuleEvalContextAsync(cultureId);
        var mergeStats = ExpertRuleEvalContextMerge.MergeIntoDiskMicLists(diskForRules, micForRules, expertEvalCtx);
        if (expertEvalCtx != null)
        {
            _logger.LogInformation(
                "GetExpertRulesForPage ExpertRuleEvalContext merge: CultureId={CultureId}, diskAdded={DiskAdded}, diskSkipped={DiskSkipped}, micAdded={MicAdded}, micSkipped={MicSkipped}",
                request.CultureId,
                mergeStats.diskAdded,
                mergeStats.diskSkipped,
                mergeStats.micAdded,
                mergeStats.micSkipped);
        }

        foreach (var row in diskForRules.Concat(micForRules))
        {
            row.OrganismId = cultureDetails.OrganismId;
            row.SpecimenTypeId = cultureDetails.SpecimenTypeId;
            row.LaboratoryId = cultureDetails.LaboratoryId;
        }

        ExpertRuleEvalContextMerge.NormalizeDefaultTestMethods(diskForRules, micForRules);

        var flattenedRowCount =
            diskForRules.Count
            + micForRules.Count
            + diskForRules.Sum(r => r.EmbeddedASTRows?.Count ?? 0)
            + micForRules.Sum(r => r.EmbeddedASTRows?.Count ?? 0);

        var display = await _expertRulePageDisplayService.BuildDisplayAsync(
            cultureId,
            cultureDetails,
            diskForRules,
            micForRules,
            persistedAstRowCount,
            savedRuleIdsFromPersistence);

        var printOnlyActionCount = display.ExpertRuleResults?.Count(r => r.IsPrintOnReportOnlyExpertAction) ?? 0;
        if (printOnlyActionCount > 0)
        {
            _logger.LogDebug(
                "AST GetExpertRulesForPage: CultureId={CultureId}, OrganismId={OrganismId}, PrintOnlyExpertActionCount={Count}",
                request.CultureId,
                cultureDetails.OrganismId,
                printOnlyActionCount);
        }

        _logger.LogInformation(
            "AST GetExpertRulesForPage: CultureId={CultureId}, OrganismId={OrganismId}, PersistedAstRowCount={Persisted}, ClientDiskRowCount={ClientDisk}, ClientMicRowCount={ClientMic}, FlattenedRowCount={Flattened}, ExpertRuleGroupCount={GroupCount}",
            request.CultureId,
            cultureDetails.OrganismId,
            persistedAstRowCount,
            clientDiskRowCount,
            clientMicRowCount,
            flattenedRowCount,
            display.ExpertRuleGroups.Count);

        return display;
    }

    /// <summary>
    /// Retrieves a test pattern with antibiotic lines, enriched with special consideration rows.
    /// Special considerations are breakpoints matching Organism, Antibiotic, Guideline and Dosage (Disk) or Organism, Antibiotic, Guideline (MIC).
    /// They appear as EmbeddedASTRows directly under each test pattern line on the AST screen.
    /// </summary>
    //public async Task<string> GetTestPatternWithBreakpointsAsync(QueryFilterConfig queryFilters)
    //{
    //    var cultureId = queryFilters.GetStringValue("cultureid");
    //    var testPatternId = queryFilters.GetStringValue("testpatternid");

    //    // Extract organism info from culture.
    //    var cultureDetailsRetriever = new CultureDetailsRetriever(_cultureRepository, _specimenRepository, _testRepository);
    //    var cultureDetails = await cultureDetailsRetriever.Get(cultureId);

    //    var testPatternResolver = new TestPatternRetriever(_testPatternRepository);
    //    var testPatternWithBreakpoints = await testPatternResolver.GetTestPatternWithBreakpoints(testPatternId, cultureDetails);

    //    // Enrich each antibiotic line with special consideration rows via enricher.
    //    foreach (var line in testPatternWithBreakpoints.AntibioticGrid)
    //    {
    //        if (line.AntibioticId == null) continue;

    //        var dosage = 0;
    //        if (!string.IsNullOrEmpty(line.Dosage) && int.TryParse(line.Dosage, out var parsedDosage))
    //        {
    //            dosage = parsedDosage;
    //        }

    //        line.EmbeddedASTRows = await _specialConsiderationEnricher.EnrichAsync(
    //            cultureDetails.OrganismId,
    //            line.AntibioticId.Value,
    //            line.GuidelinesId,
    //            dosage,
    //            line.TestMethodId,
    //            line.CategoryId,
    //            null,
    //            cultureId);

    //        // Conditionally add the ast rows to the return model
    //        var byMic = line.EmbeddedASTRows.ToLookup(r => r.Mic != null);
    //        //line.EmbeddedASTRows.MicResults.AddRange(byMic[true]);
    //        //line.EmbeddedASTRows.DiskResults.AddRange(byMic[false]);

    //        _logger.LogInformation(
    //            "GetTestPatternWithBreakpoints: CultureId={CultureId}, TestPatternId={TestPatternId}, AntibioticId={AntibioticId}, TestMethodId={TestMethodId}, SpecialConsiderationCount={Count}",
    //            cultureId, testPatternId, line.AntibioticId, line.TestMethodId, line.EmbeddedASTRows?.Count ?? 0);
    //        if (line.EmbeddedASTRows?.Count > 0)
    //        {
    //            _logger.LogDebug(
    //                "GetTestPatternWithBreakpoints special considerations: CultureId={CultureId}, OrganismId={OrganismId}, AntibioticId={AntibioticId}, TestMethodId={TestMethodId}, EmbeddedRowCount={Count}",
    //                cultureId, cultureDetails.OrganismId, line.AntibioticId, line.TestMethodId, line.EmbeddedASTRows.Count);
    //        }
    //    }

    //    return JsonConvert.SerializeObject(testPatternWithBreakpoints);
    //}

    private void LogTestPatternSelection(
        string cultureId,
        List<TestPatternScopeModel> options,
        TestPatternDefaultSelector.SelectionResult selectionResult)
    {
        foreach (var option in options)
        {
            _logger.LogInformation(
                "GetASTDataForCulture test pattern option: CultureId={CultureId}, PatternId={PatternId}, PatternName={PatternName}, IsOrganismGroupMatch={IsOrganismGroupMatch}, MakeDefault={MakeDefault}, OrderId={OrderId}, FamilyId={FamilyId}, GenusId={GenusId}, SpeciesId={SpeciesId}, SubSpeciesId={SubSpeciesId}, SerotypeId={SerotypeId}, OrgGroupCodingId={OrgGroupCodingId}",
                cultureId,
                option.Id,
                option.TestPatternName,
                option.IsOrganismGroupMatch,
                option.MakeDefault,
                option.OrderId,
                option.FamilyId,
                option.GenusId,
                option.SpeciesId,
                option.SubSpeciesId,
                option.SerotypeId,
                option.OrgGroupCodingId);
        }

        if (selectionResult.Selected != null)
        {
            _logger.LogInformation(
                "GetASTDataForCulture test pattern selected: CultureId={CultureId}, SelectedPatternId={SelectedPatternId}, SelectedPatternName={SelectedPatternName}, Reason={Reason}",
                cultureId,
                selectionResult.Selected.Id,
                selectionResult.Selected.TestPatternName,
                selectionResult.Reason);
        }
        else
        {
            _logger.LogInformation(
                "GetASTDataForCulture test pattern not auto-selected: CultureId={CultureId}, OptionCount={OptionCount}, Reason={Reason}",
                cultureId,
                options.Count,
                selectionResult.Reason);
        }
    }

    /// <summary>
    /// Returns whether the row has enough id-based criteria to look up special-consideration breakpoints
    /// (organism, antibiotic, guideline; disk also requires a dosage value for matching).
    /// </summary>
    private static bool ShouldEnrichSpecialConsiderations(ASTRowModel row, out string enrichSkipReason)
    {
        enrichSkipReason = null;
        if (row.OrganismId == 0)
        {
            enrichSkipReason = "OrganismId missing";
            return false;
        }

        if (!row.Antibiotic.HasValue || row.Antibiotic.Value == 0)
        {
            enrichSkipReason = "AntibioticId missing";
            return false;
        }

        if (row.Guidelines == 0)
        {
            enrichSkipReason = "GuidelinesId missing";
            return false;
        }

        var testMethodId = NormalizeTestMethodId(row);
        if (testMethodId == DiskTestMethodId && row.Dosage == 0)
        {
            enrichSkipReason = "Disk row requires non-zero Dosage for breakpoint lookup";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Clears breakpoint-derived susceptibility on the parent row and embeds before re-applying lookup results.
    /// Only runs when a measurement is present so criteria-only POSTs do not wipe manual susceptibility.
    /// </summary>
    private void ClearSusceptibilityBeforeLookup(ASTRowModel row)
    {
        row.EmbeddedASTRows ??= new List<ASTRowModel>();
        var allRows = new List<ASTRowModel> { row };
        allRows.AddRange(row.EmbeddedASTRows);

        foreach (var r in allRows)
        {
            if (!row.ForceRecalculateSusceptibility && r.IsManuallySetSusceptibility())
            {
                continue;
            }

            if (r.TestResult == 0 && r.AppliedBreakpointId == 0)
            {
                continue;
            }

            _logger.LogInformation(
                "AST GetSusceptibilitiesAndExpertRules: Cleared TestResult before lookup. OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, SpecialConsiderationId={SpecialConsiderationId}, PriorTestResult={PriorTestResult}",
                row.OrganismId,
                r.Antibiotic,
                row.Guidelines,
                r.SpecialConsiderationId,
                r.TestResult);

            r.TestResult = 0;
            r.AppliedBreakpointId = 0;
        }
    }

    /// <summary>
    /// Resolves test method id for susceptibility and special-consideration queries (681 disk, 680 MIC).
    /// </summary>
    private static int NormalizeTestMethodId(ASTRowModel row)
    {
        if (row.TestMethod == MicTestMethodId || row.TestMethod == DiskTestMethodId)
        {
            return row.TestMethod;
        }

        return row.ZoneDiameter.HasValue && row.ZoneDiameter.Value != 0
            ? DiskTestMethodId
            : MicTestMethodId;
    }

    public async Task<List<SusceptibilityModel>> GetSusceptibilitiesAsync(QueryFilterConfig queryFilters)
    {
        return await _ASTRepository.RetrieveSusceptibilitiesAsync(queryFilters);
    }

    public async Task<List<TestPatternLineModel>> GetTestPatternLinesAsync(string id)
    {
        var parameters = new QueryFilterConfig().AddString("Id", id);
        return await _testPatternRepository.GetTestPatternLinesAsync(parameters);
    }
}
