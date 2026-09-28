using arc.app.Common;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class RunIqcTestCommand : ICommandWithTypeReturningInteger<IqcTest>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, IqcTest iqcTest, ILogWriter logWriter)
        {
            foreach (var result in iqcTest.Results)
            {
                var sql = "UPDATE iqcresults SET value=@Value WHERE id=@Id;";
                await connection.QueryAsync(sql, result);
            }

            var startedListItemResult = await connection.QueryFirstAsync("select li.id as \"Id\" from listitem li left join list l on l.id = li.listid where l.name = 'IqcTestState' and li.value = 'Started'");
            var startedListItemId = startedListItemResult.Id;

            var completedListItemResult = await connection.QueryFirstAsync("select li.id as \"Id\" from listitem li left join list l on l.id = li.listid where l.name = 'IqcTestState' and li.value = 'Completed'");
            var completedListItemId = completedListItemResult.Id;

            var setCompletedDateSql = @"
                        UPDATE iqctests 
                        SET 
                        stateid =
                            case when (select count(id) from iqcresults where value is not null and iqctestid = @Id) > 0 and stateid != @completedListItemId
                            then @startedListItemId 
                            else stateid end 
                        WHERE id = @Id";

            await connection.QueryAsync(setCompletedDateSql,
                new
                {
                    iqcTest.Id,
                    startedListItemId,
                    completedListItemId
                });

            return iqcTest.Id;
        }
    }
}
