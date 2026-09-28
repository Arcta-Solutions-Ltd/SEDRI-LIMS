using arc.common;
using arc.common.Models.AST;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.AST;

/// <summary>
/// Enriches AST rows with special consideration rows derived from breakpoints.
/// Merges breakpoint-derived rows with stored special rows so that all special consideration types
/// for an antibiotic/guideline/dosage are displayed, with stored values taking precedence.
/// </summary>
public class SpecialConsiderationEnricher : ISpecialConsiderationEnricher
{
    private const int NoSpecialConsiderationId = 973;

    private readonly IASTRepository _astRepository;
    private readonly IMapWithList<SusceptibilityModel, ASTRowModel> _susceptibilityMapper;
    private readonly ILogger<SpecialConsiderationEnricher> _logger;

    public SpecialConsiderationEnricher(
        IASTRepository astRepository,
        IMapWithList<SusceptibilityModel, ASTRowModel> susceptibilityMapper,
        ILogger<SpecialConsiderationEnricher> logger)
    {
        _astRepository = astRepository;
        _susceptibilityMapper = susceptibilityMapper;
        _logger = logger;
    }

    /// <summary>
    /// Enriches a parent AST row with special consideration rows. Retrieves breakpoints for the given
    /// antibiotic/guideline/dosage/organism, filters to those with special considerations, and merges
    /// with any stored embedded rows so stored values (TestResult, IncludeOnReport) take precedence.
    /// </summary>
    /// <param name="organismId">Organism ID for breakpoint lookup.</param>
    /// <param name="antibioticId">Antibiotic ID.</param>
    /// <param name="guidelinesId">Guidelines/source ID.</param>
    /// <param name="dosage">Dosage (0 for MIC).</param>
    /// <param name="testMethodId">Test method ID (681 Disk, 680 MIC).</param>
    /// <param name="drugCategory">Drug category for embedded rows.</param>
    /// <param name="storedEmbeddedRows">Optional stored special rows from specialastrow; stored values take precedence.</param>
    /// <param name="cultureId">Culture ID for logging.</param>
    /// <param name="retainStoredWhenNoMatch">When false, stale stored embeds are not returned when breakpoints no longer match (live criteria change).</param>
    /// <returns>List of embedded AST rows (special considerations only), with stored values merged where applicable.</returns>
    public async Task<List<ASTRowModel>> EnrichAsync(
        int organismId,
        int antibioticId,
        int guidelinesId,
        int dosage,
        int testMethodId,
        int drugCategory,
        List<ASTRowModel> storedEmbeddedRows,
        string cultureId = null,
        bool retainStoredWhenNoMatch = true)
    {
        if (guidelinesId == 0)
        {
            return retainStoredWhenNoMatch
                ? (storedEmbeddedRows ?? new List<ASTRowModel>())
                : new List<ASTRowModel>();
        }

        var susceptibilityQueryFilters = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig>
            {
                new QueryValuesConfig() { Key = "OrganismId", Value = organismId.ToString() },
                new QueryValuesConfig() { Key = "AntibioticId", Value = antibioticId.ToString() },
                new QueryValuesConfig() { Key = "TestMethodId", Value = testMethodId.ToString() },
                new QueryValuesConfig() { Key = "SourceId", Value = guidelinesId.ToString() },
                new QueryValuesConfig() { Key = "Dosage", Value = dosage.ToString() }
            }
        };

        var retrievedBreakpoints = await _astRepository.RetrieveSusceptibilitiesAsync(susceptibilityQueryFilters);
        var specialConsiderationBreakpoints = retrievedBreakpoints
            .Where(b => b.SpecialConsiderationId != 0 && b.SpecialConsiderationId != NoSpecialConsiderationId)
            .ToList();

        if (specialConsiderationBreakpoints.Count == 0)
        {
            var noMatchStoredCount = storedEmbeddedRows?.Count ?? 0;
            if (retainStoredWhenNoMatch)
            {
                if (noMatchStoredCount > 0)
                {
                    _logger.LogInformation(
                        "SpecialConsiderationEnricher: No special consideration breakpoints; retaining {StoredCount} stored embed(s) for load/edit. OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}",
                        noMatchStoredCount, organismId, antibioticId, guidelinesId);
                }
                else
                {
                    _logger.LogDebug(
                        "SpecialConsiderationEnricher: No special consideration breakpoints for OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}",
                        organismId, antibioticId, guidelinesId);
                }

                return storedEmbeddedRows ?? new List<ASTRowModel>();
            }

            _logger.LogInformation(
                "SpecialConsiderationEnricher: No special consideration breakpoints; clearing embed list for criteria change. OrganismId={OrganismId}, AntibioticId={AntibioticId}, GuidelinesId={GuidelinesId}, PriorStoredCount={StoredCount}",
                organismId, antibioticId, guidelinesId, noMatchStoredCount);
            return new List<ASTRowModel>();
        }

        foreach (var bp in specialConsiderationBreakpoints)
        {
            bp.CategoryId = drugCategory;
        }

        var breakpointDerivedRows = _susceptibilityMapper.MapList(specialConsiderationBreakpoints);
        var storedBySpecialId = (storedEmbeddedRows ?? new List<ASTRowModel>())
            .Where(r => r.SpecialConsiderationId != 0 && r.SpecialConsiderationId != NoSpecialConsiderationId)
            .ToDictionary(r => r.SpecialConsiderationId);

        var mergedRows = new List<ASTRowModel>();
        foreach (var bpRow in breakpointDerivedRows)
        {
            var embeddedRow = new ASTRowModel
            {
                Antibiotic = antibioticId,
                Guidelines = guidelinesId,
                Dosage = dosage,
                TestMethod = testMethodId,
                DrugCategory = drugCategory,
                SpecialConsiderationId = bpRow.SpecialConsiderationId,
                SpecialConsideration = bpRow.SpecialConsideration,
                OrganismId = organismId,
                IncludeOnReport = "No",
                TestResult = 0
            };

            if (storedBySpecialId.TryGetValue(bpRow.SpecialConsiderationId, out var stored))
            {
                embeddedRow.TestResult = stored.TestResult;
                embeddedRow.IncludeOnReport = stored.IncludeOnReport ?? "No";
                embeddedRow.AppliedBreakpointId = stored.AppliedBreakpointId != 0
                    ? stored.AppliedBreakpointId
                    : bpRow.AppliedBreakpointId;
            }
            else
            {
                embeddedRow.TestResult = bpRow.TestResult;
                embeddedRow.IncludeOnReport = bpRow.IncludeOnReport ?? "No";
                embeddedRow.AppliedBreakpointId = bpRow.AppliedBreakpointId;
            }

            mergedRows.Add(embeddedRow);
        }

        var storedCount = storedBySpecialId.Count;
        var breakpointCount = breakpointDerivedRows.Count;
        _logger.LogDebug(
            "SpecialConsiderationEnricher: CultureId={CultureId}, AntibioticId={AntibioticId}, StoredRows={StoredCount}, BreakpointRows={BreakpointCount}, MergedCount={MergedCount}",
            cultureId ?? "(n/a)", antibioticId, storedCount, breakpointCount, mergedRows.Count);

        return mergedRows;
    }
}
