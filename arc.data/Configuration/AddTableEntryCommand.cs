using arc.common;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Command to add a new list item and create parent relations in listitemparentchild
    /// from a CSV of parent IDs.
    /// </summary>
    internal class AddTableEntryCommand : ICommandReturningInteger
    {
        /// <summary>
        /// Inserts the list item, then inserts one row per parentId in listitemparentchild.
        /// </summary>
        /// <param name="connect">Open Npgsql connection.</param>
        /// <param name="args">args[0]=Value, args[1]=ListId, args[2]=ParentIds CSV, args[3]=Enabled (Yes/No), args[4]=Fixed (Yes/No).</param>
        /// <returns>Newly created list item Id.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var enabled = args[3].ToLower() == "yes";
            var isFixed = args[4].ToLower() == "yes";
            var returnId = new IdModel();

            var sql = "Select Id from ListItem Where LOWER(TRIM(Value)) = @Value and ListId = @ListId";
            var matchedEntry = await connect.QueryAsync<IdModel>(sql, new { Value = args[0].Trim().ToLower(), ListId = int.Parse(args[1]) });
            if (matchedEntry.Count() == 0)
            {
                // Insert list item (no parentid column anymore)
                sql = @"insert into ListItem(value, listid, lastmodifieddate, enabled, fixed, DisplayOrder, Deleted)
                         values(@Value, @ListId, now(), @Enabled, @isFixed, 1, false) returning id";
                var id = await connect.QueryFirstAsync<IdModel>(sql, new { Value = args[0].Trim(), ListId = int.Parse(args[1]), Enabled = enabled, isFixed });
                returnId.Id = id.Id;

                // Insert parent relations if provided (CSV of ids in args[2])
                var parentIdsCsv = args[2];
                if (!string.IsNullOrWhiteSpace(parentIdsCsv) && parentIdsCsv.Trim() != "0")
                {
                    var parentIds = parentIdsCsv.Split(',').Select(p => p.Trim()).Where(p => int.TryParse(p, out _)).Select(int.Parse);
                    foreach (var parentId in parentIds)
                    {
                        var relSql = @"insert into listitemparentchild(parentid, childid, lastmodifieddate) values(@ParentId, @ChildId, now())";
                        await connect.ExecuteAsync(relSql, new { ParentId = parentId, ChildId = int.Parse(returnId.Id) });
                    }
                }
            }
            else
            {
                sql = "Update ListItem set Deleted = false, DisplayOrder = 1, Enabled = @Enabled Where LOWER(TRIM(Value)) = @Value and ListId = @ListId";
                await connect.ExecuteAsync(sql, new { Value = args[0].Trim().ToLower(), ListId = int.Parse(args[1]), Enabled = enabled });
                returnId.Id = matchedEntry.First().Id;

                // Refresh parent relations for existing record
                var parentIdsCsv = args[2];
                // Remove existing relations
                await connect.ExecuteAsync("delete from listitemparentchild where childid = @ChildId", new { ChildId = int.Parse(returnId.Id) });
                if (!string.IsNullOrWhiteSpace(parentIdsCsv) && parentIdsCsv.Trim() != "0")
                {
                    var parentIds = parentIdsCsv.Split(',').Select(p => p.Trim()).Where(p => int.TryParse(p, out _)).Select(int.Parse);
                    foreach (var parentId in parentIds)
                    {
                        var relSql = @"insert into listitemparentchild(parentid, childid, lastmodifieddate) values(@ParentId, @ChildId, now())";
                        await connect.ExecuteAsync(relSql, new { ParentId = parentId, ChildId = int.Parse(returnId.Id) });
                    }
                }
            }

            return int.Parse(returnId.Id);
        }
    }
}
