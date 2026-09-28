using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Deletes a culture and all direct dependent rows such as AST entries,
    /// instrument results, culture tests, and specimen comments referencing it.
    /// </summary>
    internal class DeleteCultureCommand : ICommand
    {
        /// <summary>
        /// Executes the cascading delete for a culture by Id.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="args">Arguments where <c>args[0]</c> is the culture Id.</param>
        /// <returns>The number of rows affected by the final delete on <c>culture</c>.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var id = int.Parse(args[0]);

            var sql = @"delete from ast a
                        using culture x, culture c
                        where x.id = @Id
                          and c.parentcultureid = x.parentcultureid
                          and a.cultureid = c.id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from instrumentresults i
                     using culture x, culture c
                     where x.id = @Id
                       and c.parentcultureid = x.parentcultureid
                       and i.cultureid = c.id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from culturetests t
                     using culture x, culture c
                     where x.id = @Id
                       and c.parentcultureid = x.parentcultureid
                       and t.cultureid = c.id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from specimencomment s
                     using culture x, culture c
                     where x.id = @Id
                       and c.parentcultureid = x.parentcultureid
                       and s.cultureid = c.id";
            await connect.ExecuteAsync(sql, new { Id = id });

            sql = @"delete from culture c
                     using culture x
                     where x.id = @Id
                       and c.parentcultureid = x.parentcultureid";
            return await connect.ExecuteAsync(sql, new { Id = id });
        }
    }
}
