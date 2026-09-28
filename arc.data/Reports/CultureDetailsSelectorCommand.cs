using arc.app.Common;
using arc.app.Reports.InclusionSelectors;
using arc.common.ExtensionMethods;
using arc.common.Utils;
using arc.domain.Tests;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Reports
{
    /// <summary>
    /// Persists culture print selector toggles. AST parent rows update <c>AST.displayonreport</c>;
    /// special consideration rows update <c>specialastrow.displayonreport</c> matched by ast id and special type id.
    /// </summary>
    internal class CultureDetailsSelectorCommand : ICommandWithTypeReturningInteger<CultureDetailsSelectorModel>
    {
        /// <summary>
        /// Saves culture test, AST, and comment print-on-report selections from the culture print selector form.
        /// </summary>
        /// <param name="connect">Open database connection.</param>
        /// <param name="command">Culture print selector payload including AstGrid rows keyed by id and special consideration id.</param>
        /// <param name="logWriter">Optional logger for production diagnostics.</param>
        /// <returns>Zero on success.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, CultureDetailsSelectorModel command, ILogWriter logWriter = null)
        {
            var jsonElementRemover = new JsonElementRemover();
            var jsonReplacer = new JsonReplacer(jsonElementRemover);

            if (command.AstGrid != null)
            {
                foreach (var item in command.AstGrid)
                {
                    if (AntibioticListForReportExtensions.IsSpecialConsiderationReportRow(item.SpecialConsiderationId))
                    {
                        const string specialSql =
                            @"update specialastrow set displayonreport = @Display where astid = @AstId and specialtypeid = @SpecialTypeId";
                        var rowsAffected = await connect.ExecuteAsync(specialSql, new
                        {
                            AstId = item.Id,
                            SpecialTypeId = item.SpecialConsiderationId,
                            Display = item.PrintOnReport
                        });
                        logWriter?.LogInfo(
                            $"Culture print selector AST special row saved: cultureId={command.Id}, astId={item.Id}, specialConsiderationId={item.SpecialConsiderationId}, printOnReport={item.PrintOnReport}, rowsAffected={rowsAffected}",
                            nameof(CultureDetailsSelectorCommand),
                            nameof(ExecuteAsync));
                        if (rowsAffected == 0)
                        {
                            logWriter?.LogInfo(
                                $"WARNING: Culture print selector AST special row update matched no rows: cultureId={command.Id}, astId={item.Id}, specialConsiderationId={item.SpecialConsiderationId}",
                                nameof(CultureDetailsSelectorCommand),
                                nameof(ExecuteAsync));
                        }
                    }
                    else
                    {
                        const string parentSql = @"update ast set displayonreport = @Display where id = @Id";
                        var rowsAffected = await connect.ExecuteAsync(parentSql, new { item.Id, Display = item.PrintOnReport });
                        logWriter?.LogInfo(
                            $"Culture print selector AST parent row saved: cultureId={command.Id}, astId={item.Id}, printOnReport={item.PrintOnReport}, rowsAffected={rowsAffected}",
                            nameof(CultureDetailsSelectorCommand),
                            nameof(ExecuteAsync));
                        if (rowsAffected == 0)
                        {
                            logWriter?.LogInfo(
                                $"WARNING: Culture print selector AST parent row update matched no rows: cultureId={command.Id}, astId={item.Id}",
                                nameof(CultureDetailsSelectorCommand),
                                nameof(ExecuteAsync));
                        }
                    }
                }
            }

            if (command.CultureTestGrid != null)
            {
                foreach (var test in command.CultureTestGrid)
                {
                    var sql = "select * from culturetests where Id = @Id";
                    var testRecord = await connect.QueryFirstAsync<CultureTest>(sql, new { Id = int.Parse(test.Id) });
                    var contents = jsonReplacer.ChangeValueInJsonString(testRecord.TestResults, "printonreport", test.PrintOnReport);

                    sql = @"update culturetests set TestResults = cast(@Contents as json) where Id = @Id";
                    await connect.ExecuteAsync(sql, new { Id = int.Parse(test.Id), contents });
                }
            }

            if (command.CultureCommentGrid != null)
            {
                foreach (var comment in command.CultureCommentGrid)
                {
                    var sql = @"update specimencomment set DisplayOnReport = @Display where Id = @Id";
                    await connect.ExecuteAsync(sql, new { Id = int.Parse(comment.Id), Display = comment.PrintOnReport });
                }
            }

            return 0;
        }
    }
}
