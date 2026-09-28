using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Utils;
using arc.data.model;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    /// <summary>
    /// Imports the list of string from a configuration export file.
    /// Author: Arcta Solutions Limited.
    /// </summary>
    internal class ImportConfigurationCommand : ICommandWithTypeReturningInteger<List<string>>
    {
        /// <summary>
        /// Executes the command to import the list of string from a configuration export file.
        /// </summary>
        /// <param name="connect">Database connection</param>
        /// <param name="command">List of strings to import, must be in the format of the lines in a configuration export file</param>
        /// <param name="logWriter">Logwriter object</param>
        /// <returns>The number of rows affected</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, List<string> command, ILogWriter logWriter)
        {
            await DeleteContentsOfTablesBeingImportedAsync(connect, command, logWriter);

            await ReplaceContentsOfTablesAsync(connect, command, logWriter);

            return 0;
        }

        /// <summary>
        /// Deletes the contents of tables being imported asynchronously.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection instance used to connect to the database.</param>
        /// <param name="contents">The list of table content strings to be processed.</param>
        /// <param name="logWriter">The log writer instance for logging information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static async Task DeleteContentsOfTablesBeingImportedAsync(NpgsqlConnection connect, List<string> contents, ILogWriter logWriter)
        {
            if (contents == null || contents.Count == 0)
            {
                logWriter.LogError("No contents provided for deletion.", "ImportConfigurationCommand", "DeleteContentsOfTablesBeingImportedAsync");
                return;
            }

            var currentTable = string.Empty;

            foreach (var line in contents)
            {
                var lineSplit = line.Split(new[] { "<|>" }, StringSplitOptions.None);
                if (lineSplit.Length == 0)
                {
                    logWriter.LogError("Invalid content line format.", "ImportConfigurationCommand", "DeleteContentsOfTablesBeingImportedAsync");
                    continue;
                }

                var newTable = lineSplit[0].ToLowerInvariant();

                if (newTable != currentTable)
                {
                    logWriter.LogInfo($"Deleting contents of table {newTable}", "ImportConfigurationCommand", "DeleteContentsOfTablesBeingImportedAsync");
                    var deleteSql = $"DELETE FROM {newTable}";

                    try
                    {
                        await connect.ExecuteAsync(deleteSql);
                        currentTable = newTable;
                    }
                    catch (Exception ex)
                    {
                        logWriter.LogError($"Failed to delete contents of table {newTable}. Error: {ex.Message}", "ImportConfigurationCommand", "DeleteContentsOfTablesBeingImportedAsync");
                    }
                }
            }
        }


        /// <summary>
        /// Replaces the contents of tables asynchronously.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection instance used to connect to the database.</param>
        /// <param name="contents">The list of table content strings to be processed.</param>
        /// <param name="logWriter">The log writer instance for logging information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static async Task ReplaceContentsOfTablesAsync(NpgsqlConnection connect, List<string> contents, ILogWriter logWriter)
        {
            if (contents == null || contents.Count == 0)
            {
                logWriter.LogError("No contents provided for replacement.", "ImportConfigurationCommand", "ReplaceContentsOfTablesAsync");
                return;
            }

            var currentTable = string.Empty;
            contents.Reverse();

            foreach (var line in contents)
            {
                var lineSplit = line.Split(new[] { "<|>" }, StringSplitOptions.None);
                if (lineSplit.Length == 0)
                {
                    logWriter.LogError("Invalid content line format.", "ImportConfigurationCommand", "ReplaceContentsOfTablesAsync");
                    continue;
                }

                var newTable = lineSplit[0].ToLowerInvariant();

                var tablesToCopy = DataModelUtils.DataModelList();
                var table = tablesToCopy.FirstOrDefault(d => d.Value == newTable);

                if (table.Key == null)
                {
                    logWriter.LogError($"Table {newTable} not found in the data model list.", "ImportConfigurationCommand", "ReplaceContentsOfTablesAsync");
                    continue;
                }   

                logWriter.LogInfo($"Start loading the data for table {newTable}", "ImportConfigurationCommand", "ReplaceContentsOfTablesAsync");
                var newObject = ObjectHelper.GetPopulatedObjectFromDelimitedString(table, line, "<|>");
                var sql = newObject.GenerateInsertStatement(table.Value);

                try
                {
                    await connect.ExecuteAsync(sql, newObject);
                    logWriter.LogInfo($"Finished loading the data for table {newTable}", "ImportConfigurationCommand", "ReplaceContentsOfTablesAsync");
                }
                catch (Exception ex)
                {
                    logWriter.LogError($"Failed to load data for table {newTable}. Error: {ex.Message}", "ImportConfigurationCommand", "ReplaceContentsOfTablesAsync");
                }
            }
        }
    }
}



