using arc.app.Laboratory;
using arc.app.Specimen;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Limits a culture (isolate) test form list to the tests applicable for the culture,
/// using the culture type and organism scope laboratory configuration. Uses the same resolver
/// as the AST screen so both surfaces offer the same isolate tests.
/// </summary>
public class CultureTestSelectionFilter : ICultureTestSelectionFilter
{
    private readonly ICultureRepository _cultureRepository;
    private readonly ILaboratoryConfigurationHandler _laboratoryConfigurationHandler;
    private readonly ILogger<CultureTestSelectionFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureTestSelectionFilter"/> class.
    /// </summary>
    /// <param name="cultureRepository">Repository used to read the culture type and organism for the culture.</param>
    /// <param name="laboratoryConfigurationHandler">Handler that resolves the applicable isolate tests from laboratory configuration.</param>
    /// <param name="logger">Logger used to record filtering diagnostics.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="cultureRepository"/>, <paramref name="laboratoryConfigurationHandler"/>, or <paramref name="logger"/> is null.
    /// </exception>
    public CultureTestSelectionFilter(ICultureRepository cultureRepository, ILaboratoryConfigurationHandler laboratoryConfigurationHandler, ILogger<CultureTestSelectionFilter> logger)
    {
        _cultureRepository = cultureRepository ?? throw new ArgumentNullException(nameof(cultureRepository));
        _laboratoryConfigurationHandler = laboratoryConfigurationHandler ?? throw new ArgumentNullException(nameof(laboratoryConfigurationHandler));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<List<FormListModel>> ApplyAsync(List<FormListModel> allTests, string cultureId)
    {
        if (allTests == null || allTests.Count == 0)
        {
            return allTests;
        }

        if (!int.TryParse(cultureId, out var cultureIdValue) || cultureIdValue <= 0)
        {
            _logger.LogWarning(
                "CultureTestSelectionFilter could not parse CultureId={CultureId}; returning all {TestCount} isolate test(s) unfiltered.",
                cultureId,
                allTests.Count);
            return allTests;
        }

        var cultureDetails = await _cultureRepository.GetASTCultureDataAsync(cultureId);

        if (cultureDetails == null)
        {
            _logger.LogWarning(
                "CultureTestSelectionFilter found no culture record for CultureId={CultureId}; returning all {TestCount} isolate test(s) unfiltered.",
                cultureId,
                allTests.Count);
            return allTests;
        }

        var applicableTests = await _laboratoryConfigurationHandler.GetApplicableIsolateTestsForSelectionAsync(
            cultureDetails.LaboratoryId,
            cultureDetails.CultureTypeId,
            cultureDetails.OrganismId,
            cultureDetails.OrgGroupCodingId);

        if (applicableTests == null)
        {
            _logger.LogInformation(
                "CultureTestSelectionFilter applied no restriction for CultureId={CultureId}, LaboratoryId={LaboratoryId}, CultureTypeId={CultureTypeId}, OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}. No isolate test configuration applies, so all {TestCount} isolate test(s) remain selectable.",
                cultureId,
                cultureDetails.LaboratoryId,
                cultureDetails.CultureTypeId,
                cultureDetails.OrganismId,
                cultureDetails.OrgGroupCodingId,
                allTests.Count);
            return allTests;
        }

        var applicableNames = applicableTests.ToList();
        var filteredTests = allTests.Where(t => applicableNames.ContainsFormName(t.Name)).ToList();

        _logger.LogInformation(
            "CultureTestSelectionFilter completed for CultureId={CultureId}, LaboratoryId={LaboratoryId}, CultureTypeId={CultureTypeId}, OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}. TestCountBefore={TestCountBefore}, TestCountAfter={TestCountAfter}, FilterApplied=true.",
            cultureId,
            cultureDetails.LaboratoryId,
            cultureDetails.CultureTypeId,
            cultureDetails.OrganismId,
            cultureDetails.OrgGroupCodingId,
            allTests.Count,
            filteredTests.Count);

        _logger.LogDebug(
            "CultureTestSelectionFilter names for CultureId={CultureId}: Applicable={ApplicableNames}, Retained={RetainedNames}, Excluded={ExcludedNames}.",
            cultureId,
            string.Join(", ", applicableNames),
            string.Join(", ", filteredTests.Select(t => t.Name)),
            string.Join(", ", allTests.Except(filteredTests).Select(t => t.Name)));

        return filteredTests;
    }
}
