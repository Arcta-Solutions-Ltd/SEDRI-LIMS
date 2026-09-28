using arc.common;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Command to add a new list (table) or revive/update an existing one by name.
    /// The Name value is derived from the Description entered by the user (see
    /// <c>AddTableEvent.FormatNameFromDescription</c>). When a matching name already
    /// exists in the database the record is un-deleted and its description and parent are updated.
    /// </summary>
    internal class AddTableCommand : ICommandReturningInteger
    {
        /// <summary>
        /// Inserts a new list row, or un-deletes and updates an existing one when the
        /// normalised name (case-insensitive trim) already exists.
        /// </summary>
        /// <param name="connect">An open <see cref="NpgsqlConnection"/> to execute against.</param>
        /// <param name="args">
        /// Positional arguments:
        /// <list type="bullet">
        ///   <item><description><c>args[0]</c> — Name: the formatted description used as the list's unique name (trimmed).</description></item>
        ///   <item><description><c>args[1]</c> — Description: the raw description text entered by the user (trimmed).</description></item>
        ///   <item><description><c>args[2]</c> — ParentId: integer string of the parent list id, or "0" / empty when there is no parent.</description></item>
        /// </list>
        /// </param>
        /// <returns>The Id of the newly inserted list, or the Id of the existing list that was revived.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
        {
            var returnId = new IdModel();

            var sql = "Select Id from List Where LOWER(TRIM(Name)) = @Name";
            var matchedEntry = await connect.QueryAsync<IdModel>(sql, new {Name = args[0].Trim().ToLower() });
            if (matchedEntry.Count() == 0)
            {
                sql = @"insert into List(name, grouping, parentid, common, description, lastmodifieddate, deleted) values(@Name, @Grouping, @ParentId, @Common, @Description, now(), false) returning id";
                if (string.IsNullOrWhiteSpace(args[2]) || args[2] == "0")
                {
                    sql = @"insert into List(name, grouping, common, description, lastmodifieddate, deleted) values(@Name, @Grouping, @Common, @Description, now(), false) returning id";
                }
                var id = await connect.QueryFirstAsync<IdModel>(sql, new { Name = args[0].Trim(), Grouping = "Custom", ParentId = int.Parse(args[2]), Common = true, Description = args[1].Trim() });
                returnId.Id = id.Id;
            }
            else
            {
                sql = "Update List set Deleted = false, ParentId = @ParentId, Description = @Description Where LOWER(TRIM(Name)) = @Name";
                await connect.ExecuteAsync(sql, new { Name = args[0].Trim().ToLower(), ParentId = int.Parse(args[2]), Description = args[1].Trim() });
                returnId.Id = matchedEntry.First().Id;
            }

            return int.Parse(returnId.Id);
        }
    }
}
