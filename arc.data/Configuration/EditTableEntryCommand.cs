using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Command to edit an existing list item and refresh its parent relationships.
    /// </summary>
    internal class EditTableEntryCommand : ICommandReturningInteger
    {
        /// <summary>
        /// Updates value/enabled on the list item and replaces rows in listitemparentchild
        /// using the CSV of parent IDs provided.
        /// </summary>
        /// <param name="connect">Open Npgsql connection.</param>
        /// <param name="args">args[0]=Value, args[1]=Id, args[2]=ParentIds CSV, args[3]=Enabled (Yes/No).</param>
        /// <returns>Edited item Id.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var enabled = args[3].ToLower() == "yes" ? true : false;
            var sql = @"Update ListItem set value = @Value, lastmodifieddate = now(), enabled = @Enabled Where Id = @Id";
            await connect.ExecuteAsync(sql, new { Value = args[0], Id = int.Parse(args[1]), Enabled = enabled });

            // Refresh parent relations
            var itemId = int.Parse(args[1]);
            var parentIdsCsv = args[2];
            await connect.ExecuteAsync("delete from listitemparentchild where childid = @ChildId", new { ChildId = itemId });
            if (!string.IsNullOrWhiteSpace(parentIdsCsv) && parentIdsCsv.Trim() != "0")
            {
                var parentIds = parentIdsCsv.Split(',').Select(p => p.Trim()).Where(p => int.TryParse(p, out _)).Select(int.Parse);
                foreach (var parentId in parentIds)
                {
                    var relSql = @"insert into listitemparentchild(parentid, childid, lastmodifieddate) values(@ParentId, @ChildId, now())";
                    await connect.ExecuteAsync(relSql, new { ParentId = parentId, ChildId = itemId });
                }
            }

            return int.Parse(args[1]);
        }
    }
}
