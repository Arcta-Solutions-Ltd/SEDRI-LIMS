using arc.app.Common;
using arc.app.Config;
using arc.app.Configuration;
using arc.app.Patient;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Role;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Handles test selection operations by coordinating with various repositories and services.
/// Implements the ITestSelectionHandler interface.
/// </summary>
public class TestSelectionHandler : ITestSelectionHandler
{
    private readonly ITestRepository _testRepository;
    private readonly IFormHandler _formHandler;
    private readonly IListRepository _listRepository;
    private readonly IListViewConfigFactory _listViewConfigFactory;
    private readonly IPatientRepository _patientRepository;
    private readonly ICultureTestSelectionFilter _cultureTestSelectionFilter;
    private readonly ILogger<TestSelectionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the TestSelectionHandler class with dependencies.
    /// </summary>
    /// <param name="testRepository">Injected test repository.</param>
    /// <param name="formHandler">Injected form handler service.</param>
    /// <param name="listRepository">Injected list repository.</param>
    /// <param name="listViewConfigFactory">Injected factory for list view configs.</param>
    /// <param name="patientRepository">Injected patient repository.</param>
    /// <param name="cultureTestSelectionFilter">Injected filter limiting isolate tests to those configured for the culture.</param>
    /// <param name="logger">Injected logger used to record isolate test selection diagnostics.</param>
    public TestSelectionHandler(ITestRepository testRepository, IFormHandler formHandler, IListRepository listRepository, IListViewConfigFactory listViewConfigFactory, IPatientRepository patientRepository, ICultureTestSelectionFilter cultureTestSelectionFilter, ILogger<TestSelectionHandler> logger)
    {
        _testRepository = testRepository;
        _formHandler = formHandler;
        _listRepository = listRepository;
        _listViewConfigFactory = listViewConfigFactory;
        _patientRepository = patientRepository;
        _cultureTestSelectionFilter = cultureTestSelectionFilter;
        _logger = logger;

    }

    /// <summary>
    /// Asynchronously retrieves the list of tests for a given specimen based on query filters,
    /// determines which are allowed, and returns the result as a serialized JSON string.
    /// </summary>
    /// <param name="queryFilters">Query parameters containing the specimen ID.</param>
    /// <returns>JSON string containing test selections and their allowed status.</returns>
    public async Task<string> GetTestsForSpecimenAsync(QueryFilterConfig queryFilters)
    {
        var testDetails = await _testRepository.GetTestsForSpecimenAsync(int.Parse(queryFilters.Parameters[0].Value));

        var allTestListAsString = await _formHandler.GetFormListAsync(true);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);

        var testListToReturn = new List<CraftedSelectionsModel>();

        foreach (var test in allTestList)
        {
            var found = testDetails.Where(t => t.TestName.Equals(test.Title, System.StringComparison.OrdinalIgnoreCase));

            var newItem = new CraftedSelectionsModel
            {
                Name = test.Title,
                Key = test.Title,
                Allowed = found.Any() ? "Yes" : "No"
            };

            testListToReturn.Add(newItem);
        }

        return JsonConvert.SerializeObject(CreateReturnModel(testListToReturn, "tests"));
    }

    /// <summary>
    /// Asynchronously retrieves a list of available tests, excluding any that have already been used,
    /// based on a provided ID and test type, then returns the result as a JSON string.
    /// </summary>
    /// <param name="parameters">Filter configuration containing the parameters for test selection (expects an "id").</param>
    /// <param name="testType">A string indicating the type of tests to retrieve.</param>
    /// <param name="token">Token information used for authorization or context.</param>
    /// <returns>Serialized JSON string of the filtered test list for the specified test type.</returns>
    public async Task<string> GetTestsAsync(QueryFilterConfig parameters, string testType, TokenInfoModel token)
    {
        var id = parameters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var testListToReturn = await GetTestListWithUsedTestsRemovedAsync(id.Value, testType, token);

        return JsonConvert.SerializeObject(CreateReturnModel(testListToReturn, testType));
    }

    /// <summary>
    /// Asynchronously retrieves all tests of a specific type, filters out those not in view,
    /// and returns a serialized JSON string of the resulting list with 'Allowed' set to "No".
    /// </summary>
    /// <param name="testType">The category or type of test to retrieve.</param>
    /// <param name="token">Token information for authorization and context.</param>
    /// <returns>Serialized JSON string of test selections with default 'Allowed' status.</returns>
    public async Task<string> GetAllTestsAsync(string testType, TokenInfoModel token)
    {
        var allTestListAsString = await _formHandler.GetFormListAsync(true, testType, token);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);
        allTestList = await RemoveTestsNotInViewAsync(allTestList);
        var testListToReturn = allTestList
            .Select(t => new CraftedSelectionsModel
            {
                Name = t.Title,
                Key = t.Name,
                Allowed = "No"
            }).ToList();

        return JsonConvert.SerializeObject(CreateReturnModel(testListToReturn, testType));
    }

    /// <summary>
    /// Asynchronously retrieves all tests of a specified type, filters out any not in view,
    /// retrieves the patient's reference ID, and returns the result as a JSON string.
    /// </summary>
    /// <param name="parameters">Query filter configuration that includes patient ID.</param>
    /// <param name="testType">The type/category of tests to retrieve.</param>
    /// <param name="token">Token information for authorization and context.</param>
    /// <returns>Serialized JSON string of test selections along with the patient's reference data.</returns>
    public async Task<string> GetAllTestsWithPatientRefAsync(QueryFilterConfig parameters, string testType, TokenInfoModel token)
    {
        var allTestListAsString = await _formHandler.GetFormListAsync(true, testType, token);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);
        allTestList = await RemoveTestsNotInViewAsync(allTestList);
        var patientId = parameters.Parameters[0].Value;
        var patientRef = await _patientRepository.GetPatientRefFromIdAsync(int.Parse(patientId));
        var dateOfBirth = await _patientRepository.GetDateOfBirthFromIdAsync(int.Parse(patientId));

        var testListToReturn = allTestList
            .Select(t => new CraftedSelectionsModel
            {
                Name = t.Title,
                Key = t.Name,
                Allowed = "No"
            }).ToList();

        return JsonConvert.SerializeObject(CreatePatientReturnModel(testListToReturn, testType, patientId, patientRef, dateOfBirth));
    }

    /// <summary>
    /// Asynchronously retrieves test options and culture type selections for a given test type,
    /// filters out irrelevant tests, and returns a combined JSON model containing both sets of data.
    /// </summary>
    /// <param name="testType">Specifies the type of tests to retrieve.</param>
    /// <param name="token">Token used for authorization and context.</param>
    /// <returns>A JSON string containing both test selections and sorted culture type selections.</returns>
    public async Task<string> GetTestsAndCultureTypesAsync(string testType, TokenInfoModel token)
    {
        var allTestListAsString = await _formHandler.GetFormListAsync(true, testType, token);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);
        allTestList = await RemoveTestsNotInViewAsync(allTestList);

        var testListToReturn = allTestList
            .Select(t => new CraftedSelectionsModel
            {
                Name = t.Title,
                Key = t.Name,
                Allowed = "No"
            }).ToList();

        var cultureListToReturn = await GetCultureTypeListAsync();

        var returnModel = CreateReturnModel(testListToReturn, testType);
        returnModel.Crafted.Add(new CraftedModel
        {
            Name = "culturetypeselectionpage",
            Contents = JsonConvert.SerializeObject(cultureListToReturn.OrderBy(o => o.Name))
        });

        return JsonConvert.SerializeObject(returnModel);
    }

    /// <summary>
    /// Asynchronously retrieves test options and culture type selections for a given test type,
    /// includes patient reference information, and returns the result as a JSON-serialized model.
    /// </summary>
    /// <param name="parameters">Filter configuration that contains patient ID.</param>
    /// <param name="testType">The type or category of tests to be fetched.</param>
    /// <param name="token">Token carrying authentication and contextual information.</param>
    /// <returns>A JSON string representing test selections, patient reference, and culture type list.</returns>
    public async Task<string> GetTestsAndCultureTypesWithPatientRefAsync(QueryFilterConfig parameters, string testType, TokenInfoModel token)
    {
        var allTestListAsString = await _formHandler.GetFormListAsync(true, testType, token);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);
        allTestList = await RemoveTestsNotInViewAsync(allTestList);
        var patientId = parameters.Parameters[0].Value;
        var testListToReturn = allTestList.Select(t => new CraftedSelectionsModel { Name = t.Title, Key = t.Name, Allowed = "No" }).ToList();
        var cultureListToReturn = await GetCultureTypeListAsync();
        var patientRef = await _patientRepository.GetPatientRefFromIdAsync(int.Parse(patientId));
        var dateOfBirth = await _patientRepository.GetDateOfBirthFromIdAsync(int.Parse(patientId));

        var returnModel = CreatePatientReturnModel(testListToReturn, testType, patientId, patientRef, dateOfBirth);
        returnModel.Crafted.Add(new CraftedModel { Name = "culturetypeselectionpage", Contents = JsonConvert.SerializeObject(cultureListToReturn.OrderBy(o => o.Name)) });
        return JsonConvert.SerializeObject(returnModel);
    }

    /// <summary>
    /// Creates a return model containing a single crafted test or culture selection page.
    /// </summary>
    /// <param name="testList">List of test selections.</param>
    /// <param name="testType">Type of test ("tests" or another category).</param>
    /// <returns>A JustCraftedPages object containing the serialized test list.</returns>
    public JustCraftedPages CreateReturnModel(List<CraftedSelectionsModel> testList, string testType)
    {
        var craftedModels = new List<CraftedModel>();
        var name = testType == "tests" ? "testselectionpage" : "testcultureselectionpage";
        craftedModels.Add(new CraftedModel { Name = name, Contents = JsonConvert.SerializeObject(testList.OrderBy(o => o.Name)) });
        return new JustCraftedPages { Crafted = craftedModels };
    }

    /// <summary>
    /// Creates a return model including patient ID and reference along with a crafted test or culture selection page.
    /// </summary>
    /// <param name="testList">List of test selections.</param>
    /// <param name="testType">Type of test ("tests" or another category).</param>
    /// <param name="id">Patient ID.</param>
    /// <param name="patientRef">Reference string for the patient.</param>
    /// <param name="dateOfBirth">Patient date of birth (ISO format) for specimen age auto-calculation.</param>
    /// <returns>PatientWithCraftedModel containing test data and patient details.</returns>
    public PatientWithCraftedModel CreatePatientReturnModel(List<CraftedSelectionsModel> testList, string testType, string id, string patientRef, string dateOfBirth = null)
    {
        var craftedModels = new List<CraftedModel>();
        var name = testType == "tests" ? "testselectionpage" : "testcultureselectionpage";
        craftedModels.Add(new CraftedModel { Name = name, Contents = JsonConvert.SerializeObject(testList.OrderBy(o => o.Name)) });
        return new PatientWithCraftedModel { PatientId = id, PatientRef = patientRef, DateOfBirth = dateOfBirth, Crafted = craftedModels };
    }

    /// <summary>
    /// Returns test list with tests already used removed, based on patient and test type.
    /// </summary>
    /// <param name="id">Patient or culture identifier.</param>
    /// <param name="testType">The type of test ("tests" or "culture").</param>
    /// <param name="token">Authentication or context token.</param>
    /// <returns>Filtered list of test selections.</returns>
    public async Task<List<CraftedSelectionsModel>> GetTestListWithUsedTestsRemovedAsync(string id, string testType, TokenInfoModel token)
    {
        var allTestListAsString = await _formHandler.GetFormListAsync(true, testType, token);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);
        allTestList = await RemoveTestsNotInViewAsync(allTestList);
        allTestList = await RemoveIsolateTestsNotApplicableToCultureAsync(allTestList, id, testType);

        var currentTests = testType == "tests"
            ? await _testRepository.GetTestsForSpecimenAsync(int.Parse(id))
            : await _testRepository.GetTestsForCultureAsync(id);

        foreach (var test in currentTests)
        {
            allTestList.RemoveAll(x => x.Name.Equals(test.TestName, System.StringComparison.OrdinalIgnoreCase));
        }

        return allTestList.Select(t => new CraftedSelectionsModel { Name = t.Title, Key = t.Name, Allowed = "No" }).ToList();
    }

    /// <summary>
    /// Returns test list with used tests enabled (Allowed = "Yes") if not completed.
    /// </summary>
    /// <param name="id">Identifier used to retrieve related tests.</param>
    /// <param name="testType">The test type ("tests" or "culture").</param>
    /// <param name="token">Authorization or session token.</param>
    /// <returns>Test list including previously used but incomplete tests.</returns>
    public async Task<List<CraftedSelectionsModel>> GetTestListWithUsedTestsEnabledAsync(string id, string testType, TokenInfoModel token)
    {
        var allTestListAsString = await _formHandler.GetFormListAsync(true, testType, token);
        var allTestList = JsonConvert.DeserializeObject<List<FormListModel>>(allTestListAsString);
        allTestList = await RemoveTestsNotInViewAsync(allTestList);
        allTestList = await RemoveIsolateTestsNotApplicableToCultureAsync(allTestList, id, testType);

        var currentTests = testType == "tests"
            ? await _testRepository.GetTestsForSpecimenAsync(int.Parse(id))
            : await _testRepository.GetTestsForCultureAsync(id);

        var listToReturn = new List<CraftedSelectionsModel>();

        foreach (var test in allTestList)
        {
            var isAlreadySelected = currentTests.FirstOrDefault(x =>
                x.TestName.ToLower() == test.Name.ToLower() && x.Status != "Completed");

            listToReturn.Add(new CraftedSelectionsModel
            {
                Name = test.Title,
                Key = test.Name,
                Allowed = isAlreadySelected == null ? "No" : "Yes"
            });
        }

        return listToReturn;
    }

    /// <summary>
    /// Retrieves all enabled culture types from the configured list source.
    /// </summary>
    /// <returns>List of enabled culture type selections.</returns>
    public async Task<List<CraftedSelectionsModel>> GetCultureTypeListAsync()
    {
        var queryFilter = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig>
        {
            new() { Key = "ListId", Value = "102" }
        }
        };

        var allCultureTypesAsString = await _listRepository.GetListContentsAsync(queryFilter);
        return allCultureTypesAsString
            .Where(c => c.Enabled == "Yes")
            .Select(t => new CraftedSelectionsModel
            {
                Name = t.Value,
                Key = t.Id.ToString(),
                Allowed = "No"
            }).ToList();
    }

    /// <summary>
    /// Fetches the list of allowed tests configured for the "specimens" view.
    /// </summary>
    /// <returns>List of allowed test names.</returns>
    private async Task<List<string>> GetListOfTestsForSpecimenViewAsync()
    {
        var view = await _listViewConfigFactory.GetViewAsync("specimens");
        return view.Tests.Select(s => s.Replace("uievent", "form")).ToList();
    }

    /// <summary>
    /// Removes any tests from the list that are not configured to be shown in the "specimens" view.
    /// </summary>
    /// <param name="currentTestList">The full list of test definitions.</param>
    /// <returns>A filtered list containing only visible tests.</returns>
    private async Task<List<FormListModel>> RemoveTestsNotInViewAsync(List<FormListModel> currentTestList)
    {
        var returnList = new List<FormListModel>();
        var testsInView = await GetListOfTestsForSpecimenViewAsync();

        foreach (var test in currentTestList)
        {
            if (testsInView.Any(s => s.Contains(test.Name)))
            {
                returnList.Add(test);
            }
        }

        return returnList;
    }

    /// <summary>
    /// Limits an isolate test list to the tests configured against the culture's culture type and organism scope.
    /// Direct test lists ("tests") are returned unchanged because they are limited by the specimen type instead.
    /// </summary>
    /// <param name="currentTestList">The test list to filter.</param>
    /// <param name="id">The culture identifier the tests will be requested against.</param>
    /// <param name="testType">The test type being selected; only "culturetests" is filtered.</param>
    /// <returns>The applicable test list.</returns>
    private async Task<List<FormListModel>> RemoveIsolateTestsNotApplicableToCultureAsync(List<FormListModel> currentTestList, string id, string testType)
    {
        if (testType != "culturetests")
        {
            return currentTestList;
        }

        _logger.LogDebug(
            "Applying isolate test configuration filter for CultureId={CultureId} with {TestCount} candidate test(s).",
            id,
            currentTestList?.Count ?? 0);

        return await _cultureTestSelectionFilter.ApplyAsync(currentTestList, id);
    }
}
