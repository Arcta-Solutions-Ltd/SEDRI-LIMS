using arc.app.Coding;
using arc.app.Common;
using arc.common;
using arc.common.Models.Coding;
using arc.data.Coding.TestPatterns;
using arc.domain.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace arc.data.Coding;

public class TestPatternRepository : ITestPatternRepository
{
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;
    private readonly IMapType<TestPatternModel, TestPattern> _testPatternMapper;

    public TestPatternRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter, IMapType<TestPatternModel, TestPattern> testPatternMapper)
    {
        _sqlCommand = sqlCommand;
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
        _testPatternMapper = testPatternMapper;
    }

    public async Task<int> AddTestPatternAsync(TestPatternModel dataToSave)
    {
        var data = GetTestPatternData(dataToSave);
        _logWriter.LogInfo("Run add test pattern command", "TestPatternRepository", "AddTestPatternAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddTestPatternCommand(), "Insert Test Pattern", data);
    }

    public async Task<int> EditTestPatternAsync(TestPatternModel dataToSave)
    {
        var data = GetTestPatternData(dataToSave);
        _logWriter.LogInfo("Run edit test pattern command", "TestPatternRepository", "EditTestPatternAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditTestPatternCommand(), "Edit Test Pattern", data);
    }

    public async Task<TestPattern> EditTestPatternQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run edit test pattern query", "TestPatternRepository", "EditTestPatternQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new EditTestPatternQuery(), "Edit Test Pattern Query", queryFilters);
    }

    public async Task DeleteTestPatternAsync(string id)
    {
        _logWriter.LogInfo("Run delete test pattern command", "TestPatternRepository", "DeleteTestPatternAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteTestPatternCommand(), "Delete Test Pattern", id);
    }

    public async Task<IEnumerable<TestPatternListModel>> GetTestPatternListAsync(QueryFilterConfig parameters)
    {
        _logWriter.LogInfo("Run get test pattern list query", "TestPatternRepository", "GetTestPatternListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new TestPatternListQuery(), "Get Test Pattern List", parameters);
    }

    public async Task<IEnumerable<OptionsConfig>> GetTestPatternNamesListAsync()
    {
        _logWriter.LogInfo("Run get test pattern names list query", "TestPatternRepository", "GetTestPatternNamesListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new TestPatternNamesListQuery(), "Get Test Pattern Names List", new QueryFilterConfig());
    }

    public async Task<IEnumerable<TestPatternScopeModel>> GetTestPatternsForOrganismListAsync(QueryFilterConfig parameters)
    {
        _logWriter.LogInfo("Run get test patterns for organism list query", "TestPatternRepository", "GetTestPatternsForOrganismListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new TestPatternsForOrganismListQuery(), "Get Test Patterns for Organisms List", parameters);
    }

    private TestPattern GetTestPatternData(TestPatternModel dataToSave)
    {
        var data = _testPatternMapper.Map(dataToSave);
        return data;
    }

    public async Task <List<TestPatternLineModel>> GetTestPatternLinesAsync(QueryFilterConfig parameters)
    {
        _logWriter.LogInfo("Run get test pattern lines query", "TestPatternRepository", "GetTestPatternLinesAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new TestPatternLinesQuery(), "Get Test Pattern lines", parameters);
    }
}

