using arc.app.Tests;
using arc.common;
using arc.common.Models;
using arc.common.Models.Role;
using arc.common.Models.Tests;
using arc.data.Configuration;
using arc.data.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Tests;

/// <summary>
/// Repository for creating, retrieving, and listing direct tests and culture tests,
/// and for initiating instrument interface actions associated with tests.
/// </summary>
public class TestRepository : ITestRepository
{

    private readonly IOptionsMonitor<DataOptions> _options;
    private readonly ILogger _logger;
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;

    /// <summary>
    /// Creates a new instance of <see cref="TestRepository"/>.
    /// </summary>
    public TestRepository(IOptionsMonitor<DataOptions> options, ILogger logger, ISqlCommand sqlCommand, ISqlQuery sqlQuery, IInstrumentInterfaceHandler instrumentInterfaceHandler)
    {
        _options = options;
        _logger = logger;
        _sqlCommand = sqlCommand;
        _sqlQuery = sqlQuery;
        _instrumentInterfaceHandler = instrumentInterfaceHandler;
    }

    /// <summary>
    /// Retrieves a direct test by Id.
    /// </summary>
    public async Task<Test> GetTestAsync(int id)
    {
        using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
        {
            var sql = @"select * from Tests t
                            where t.id = @Id;";

            return await connect.QueryFirstAsync<Test>(sql, new { Id = id });
        }
    }

    /// <summary>
    /// Retrieves a culture test by Id.
    /// </summary>
    public async Task<Test> GetCultureTestAsync(int id)
    {
        using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
        {
            var sql = @"select * from CultureTests t
                            where t.id = @Id;";

            return await connect.QueryFirstAsync<Test>(sql, new { Id = id });
        }
    }

    /// <summary>
    /// Lists direct tests linked to a specimen.
    /// </summary>
    public async Task<IEnumerable<Test>> GetTestsForSpecimenAsync(int specimenId)
    {
        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        var sql = @"select * from Tests where SpecimenId = @specimenId";

        return await connect.QueryAsync<Test>(sql, new { specimenId });
    }

    /// <summary>
    /// Lists culture tests linked to a culture.
    /// </summary>
    public async Task<IEnumerable<Test>> GetTestsForCultureAsync(string cultureId)
    {
        using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
        {
            var sql = @"select * from CultureTests where CultureId = @CultureId";

            var result = await connect.QueryAsync<Test>(sql, new { CultureId = int.Parse(cultureId) });

            return result;
        }
    }

    /// <summary>
    /// Lists culture tests linked to a culture using a filter bag.
    /// </summary>
    public async Task<List<CultureTest>> GetTestsForCultureAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new TestsForCultureQuery(), "Get Culture Test List", queryFilters);
    }

    /// <summary>
    /// Returns the active direct test list.
    /// </summary>
    public async Task<List<TestListResultModel>> GetActiveTestListAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new ActiveTestListQuery(), "Get Direct Test List", queryFilters);
    }

    /// <summary>
    /// Returns the direct test list for a specimen, including LaboratoryId for TAT enrichment.
    /// </summary>
    public async Task<List<TestListResultModel>> GetTestListForSpecimenAsync(int specimenId)
    {
        var queryFilters = new QueryFilterConfig().AddInteger("SpecimenId", specimenId);
        return await _sqlQuery.QueryReturningTypeAsync(new TestListForSpecimenListQuery(), "Get Test List for Specimen", queryFilters);
    }

    /// <summary>
    /// Returns the culture test list for a culture, including LaboratoryId for TAT enrichment.
    /// </summary>
    public async Task<List<TestListResultModel>> GetTestListForCultureAsync(int cultureId)
    {
        var queryFilters = new QueryFilterConfig().AddInteger("CultureId", cultureId);
        return await _sqlQuery.QueryReturningTypeAsync(new TestListForCultureListQuery(), "Get Test List for Culture", queryFilters);
    }

    /// <summary>
    /// Returns a single active direct test by Id.
    /// </summary>
    public async Task<TestListResultModel> GetActiveTestByIdForTestListAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new ActiveTestByIdForTestListQuery(), "Get active direct test by id", queryFilters);
    }

    /// <summary>
    /// Returns a single active culture test by Id.
    /// </summary>
    public async Task<TestListResultModel> GetActiveCultureTestByIdForTestListAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new ActiveCultureTestByIdForTestListQuery(), "Get active culture test by id", queryFilters);
    }

    /// <summary>
    /// Adds direct tests based on the crafted payload and triggers instrument pending results.
    /// Optionally updates a state on a provided table/field pair.
    /// </summary>
    public async Task AddTestsAsync(string dataToSave, string id, EventModel command)
    {
        var stopwatch = Stopwatch.StartNew();
        var allowedCount = 0;
        try
        {
            var newObject = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<MenuPermissionEventModel>>(dataToSave);
            var testList = newObject.Crafted[0].Contents;
            allowedCount = testList.Count(t => t.Allowed == "Yes");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    foreach (var test in testList)
                    {
                        var parameters = new { SpecimenId = int.Parse(id), TestName = test.Key };
                        var sql = "";
                        if (test.Allowed == "Yes")
                        {
                            sql = @"insert into Tests(specimenid, testname, status, lastmodifieddate, requested)
                                        values(@SpecimenId, @TestName, 'Requested', now(), now())";

                            await connect.ExecuteAsync(sql, parameters);

                            await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForDirectTestId(parameters.SpecimenId, test.Key);
                        }
                    }
                    if (command.Table != null && command.Field != null)
                    {
                        var updateStateSql = @"update " + command.Table + " set " + command.Field + " = @StateId, lastmodifieddate = now() where id = @Id";
                        await connect.ExecuteAsync(updateStateSql, new { StateId = int.Parse(command.NewStateId), Id = int.Parse(id) });
                    }
                }

                scope.Complete();
            }

            stopwatch.Stop();
            _logger.LogInformation(
                "AddTestsAsync complete specimenId={SpecimenId} allowedTestCount={AllowedCount} elapsedMs={ElapsedMs}",
                id,
                allowedCount,
                stopwatch.ElapsedMilliseconds);
        }
        catch (TransactionAbortedException ex)
        {
            _logger.LogError("Creating Test Records for Specimen Transaction aborted : {0}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while adding ome tests : {0}", ex.Message);
        }
    }

    /// <summary>
    /// Adds culture tests from a serialized payload.
    /// </summary>
    public async Task<int> AddCultureTestsAsync(string dataToSave, string id)
    {
        return await _sqlCommand.CarryOutCommandReturningIntegerAsync(new AddCultureTestsCommand(_instrumentInterfaceHandler), "Add Culture Tests", dataToSave, id);
    }


    /// <summary>
    /// Returns direct test usage count for the provided filters.
    /// </summary>
    public async Task<int> DirectTestUsageCountAsync(QueryFilterConfig parameters)
    {
        return await _sqlQuery.QueryReturningIntegerAsync(new DirectTestUsageCountQuery(), "Direct Text Usage Count query", parameters);
    }

    /// <summary>
    /// Returns culture test usage count for the provided filters.
    /// </summary>
    public async Task<int> CultureTestUsageCountAsync(QueryFilterConfig parameters)
    {
        return await _sqlQuery.QueryReturningIntegerAsync(new CultureTestUsageCountQuery(), "Culture Text Usage Count query", parameters);
    }

    /// <summary>
    /// Gets the culturetest ID for the given culture and test name. Creates the record if it does not exist.
    /// </summary>
    public async Task<int> GetOrCreateCultureTestIdAsync(int cultureId, string testName)
    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddInteger("cultureid", cultureId);
        queryFilter.AddString("testname", testName ?? "");
        var result = await _sqlQuery.QueryReturningTypeAsync(new EnsureCultureTestExistsQuery(), "Get or create culture test", queryFilter);
        if (result.WasCreated)
        {
            _logger.LogDebug("GetOrCreateCultureTestId: CultureId={CultureId}, TestName={TestName}, created new Id={Id}", cultureId, testName, result.Id);
            await _instrumentInterfaceHandler.CreateInstrumentPendingResultsForCultureTestId(0, cultureId, testName);
        }
        else
            _logger.LogDebug("GetOrCreateCultureTestId: CultureId={CultureId}, TestName={TestName}, returned existing Id={Id}", cultureId, testName, result.Id);
        return result.Id;
    }

    /// <inheritdoc />
    public async Task<int> EnsureDirectTestExistsAsync(int specimenId, string testName)
    {
        var queryFilter = new QueryFilterConfig()
            .AddInteger("specimenid", specimenId)
            .AddString("testname", testName ?? "");
        var result = await _sqlQuery.QueryReturningTypeAsync(new EnsureDirectTestExistsQuery(), "Ensure direct test exists", queryFilter);
        _logger.LogInformation("EnsureDirectTestExistsAsync: Tests id={Id} specimenId={SpecimenId} testName={TestName}", result.Id, specimenId, testName);
        return result.Id;
    }

    /// <inheritdoc />
    public async Task<int> EnsureCultureTestExistsAsync(int cultureId, string testName)
    {
        var queryFilter = new QueryFilterConfig()
            .AddInteger("cultureid", cultureId)
            .AddString("testname", testName ?? "");
        var result = await _sqlQuery.QueryReturningTypeAsync(new EnsureCultureTestExistsQuery(), "Ensure culture test exists", queryFilter);
        if (result.WasCreated)
            _logger.LogInformation("EnsureCultureTestExistsAsync: created CultureTests id={Id} cultureId={CultureId} testName={TestName}", result.Id, cultureId, testName);
        else
            _logger.LogDebug("EnsureCultureTestExistsAsync: existing CultureTests id={Id} cultureId={CultureId} testName={TestName}", result.Id, cultureId, testName);
        return result.Id;
    }

}
