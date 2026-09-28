using arc.common.Models.Coding;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    /// <summary>
    /// Command that deletes an organism coding entry and, when applicable,
    /// removes the associated organism and additional records.
    /// </summary>
    internal class DeleteOrganismCommand : ICommand
    {
        /// <summary>
        /// Executes the delete sequence for an organism coding entry.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="args">
        /// Command arguments where <c>args[0]</c> is the coding <c>Id</c> to remove.
        /// </param>
        /// <returns>
        /// The number of rows affected by the final delete on <c>organismcoding</c>.
        /// </returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var sql = @"select o.additionalid, oc.organismid from organism o
                        inner join organismcoding oc on oc.organismid = o.id
                        where oc.id = @Id";
            var result = await connect.QueryAsync<OrgTypeModel>(sql, new { Id = int.Parse(args[0]) });
            var customOrgIds = result.FirstOrDefault();

            if (customOrgIds.AdditionalId != 0)
            {
                sql = @"delete from additional where id = @Id";
                await connect.ExecuteAsync(sql, new { Id = customOrgIds.AdditionalId });
                sql = @"delete from organism where id = @Id";
                await connect.ExecuteAsync(sql, new { Id = customOrgIds.OrganismId });
            }

            sql = @"delete from organismcoding where id = @Id";
            return await connect.ExecuteAsync(sql, new { Id = int.Parse(args[0]) });
        }
    }
}
