using arc.app.Common;
using arc.app.Configuration;
using arc.common.Models.Specimen;
using arc.data.Specimen;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Repository for managing comment-related operations, including retrieval and deletion.
/// </summary>
public class CommentRepository : ICommentRepository
{
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentRepository"/> class.
    /// </summary>
    /// <param name="sqlCommand">Service for executing SQL commands.</param>
    /// <param name="sqlQuery">Service for executing SQL queries.</param>
    /// <param name="logWriter">Service for logging operations.</param>
    public CommentRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
    {
        _sqlCommand = sqlCommand;
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Deletes a comment by its identifier.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    public async Task DeleteCommentAsync(string id)
    {
        _logWriter.LogInfo("Run Delete Comment Command", "CommentRepository", "DeleteCommentAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteCommentCommand(), "Delete Culture", id);
    }

    /// <summary>
    /// Retrieves report comments based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">Filters to apply to the query.</param>
    /// <returns>A string containing the report comments.</returns>
    public async Task<string> GetReportCommentsAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run Get Report Comments Query", "CommentRepository", "GetReportCommentsAsync");
        return await _sqlQuery.QueryReturningStringAsync(new GetReportCommentsQuery(), "Get Report Comments", queryFilters);
    }

    /// <summary>
    /// Retrieves canned comments for specimens.
    /// </summary>
    /// <returns>A collection of specimen comment options.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetCannedSpecimenCommentsAsync()
    {
        _logWriter.LogInfo("Run Get Canned Specimen Comments Query", "CommentRepository", "GetCannedSpecimenCommentsAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetCannedSpecimenCommentsQuery(), "Get Canned Specimen Comments", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves canned comments for cultures.
    /// </summary>
    /// <returns>A collection of culture comment options.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetCannedCultureCommentsAsync()
    {
        _logWriter.LogInfo("Run Get Canned Culture Comments Query", "CommentRepository", "GetCannedCultureCommentsAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetCannedCultureCommentsQuery(), "Get Canned Culture Comments", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves canned comments for AST (Antibiotic Susceptibility Testing).
    /// </summary>
    /// <returns>A collection of AST comment options.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetCannedASTCommentsAsync()
    {
        _logWriter.LogInfo("Run Get Canned AST Comments Query", "CommentRepository", "GetCannedASTCommentsAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetCannedASTCommentsQuery(), "Get Canned AST Comments", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves canned comments for AST manual susceptibility override audit reasons.
    /// </summary>
    /// <returns>A collection of override canned comment options.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetCannedASTSusceptibilityOverrideCommentsAsync()
    {
        _logWriter.LogInfo("Run Get Canned AST Susceptibility Override Comments Query", "CommentRepository", "GetCannedASTSusceptibilityOverrideCommentsAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetCannedASTSusceptibilityOverrideCommentsQuery(), "Get Canned AST Susceptibility Override Comments", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves a list of specimen comments based on the provided filters.
    /// </summary>
    /// <param name="queryFilters">Filters to apply to the query.</param>
    /// <returns>A list of specimen comments.</returns>
    public async Task<List<CommentListModel>> GetSpecimenCommentListByIdAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run Get Specimen Comments for Selector Query", "CommentRepository", "GetCommentsForSelectorAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetCommentsForSelectorQuery(), "Get Selector Comment List For Specimen", queryFilters);
    }

    /// <summary>
    /// Retrieves a list of culture comments based on the provided filters.
    /// </summary>
    /// <param name="queryFilters">Filters to apply to the query.</param>
    /// <returns>A list of culture comments.</returns>
    public async Task<List<CommentListModel>> GetCultureCommentListByIdAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run Get Culture Comments for Selector Query", "CommentRepository", "GetCultureCommentsForSelectorAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetCultureCommentsForSelectorQuery(), "Get Selector Comment List For Culture", queryFilters);
    }

    /// <summary>
    /// Retrieves comments relevant for ordering based on the provided filters.
    /// </summary>
    /// <param name="queryFilters">Filters to apply to the query.</param>
    /// <returns>A string containing ordering-related comments.</returns>
    public async Task<string> GetCommentsForOrderingAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run Get Comments for Ordering Query", "CommentRepository", "GetCommentsForOrderingAsync");
        return await _sqlQuery.QueryReturningStringAsync(new GetCommentsForOrderingQuery(), "Get Comments For Ordering", queryFilters);
    }
}
