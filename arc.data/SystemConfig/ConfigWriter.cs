using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    /// <summary>
    /// Owns every write the designers make to the configs table.
    /// Matching is always attempted on the configs identity first and only falls back to the
    /// configuration name scoped by ConfigTypeId, so a record of one type can never overwrite a
    /// record of another type that happens to share a name.
    /// </summary>
    internal class ConfigWriter
    {
        /// <summary>
        /// Inserts or updates a configuration record.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="transaction">The transaction the write must take part in.</param>
        /// <param name="configId">The configs identity of the record, when the designer loaded one.</param>
        /// <param name="requestedName">The configuration name the designer asked for.</param>
        /// <param name="configTypeId">The configuration type the record belongs to.</param>
        /// <param name="contents">The JSON document to store in configs.contents.</param>
        /// <param name="state">The change the designer requested, which decides whether an insert is forced.</param>
        /// <param name="logWriter">Log writer used to record how the record was matched and what happened.</param>
        /// <returns>The identity, stored name and action taken for the record.</returns>
        public async Task<ConfigWriteResult> UpsertAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            int? configId,
            string requestedName,
            int configTypeId,
            string contents,
            ConfigChangeState state,
            ILogWriter logWriter)
        {
            var normalisedName = requestedName.NormalisedConfigName();

            if (state != ConfigChangeState.Added)
            {
                var existing = await FindExistingAsync(connect, transaction, configId, normalisedName, configTypeId, logWriter);
                if (existing != null)
                {
                    if (!existing.ConfigName.IsSameConfigName(normalisedName))
                    {
                        logWriter?.LogInfo(
                            $"Config id {existing.Id} is stored as '{existing.ConfigName}' but the designer sent '{normalisedName}'. Keeping the stored name because references point at it.",
                            nameof(ConfigWriter), nameof(UpsertAsync));
                    }

                    var updateSql = @"update Configs set Contents = cast(@Contents as json), lastmodifieddate = now()
                                      where Id = @Id and ConfigTypeId = @ConfigTypeId";
                    var rowsAffected = await connect.ExecuteAsync(updateSql,
                        new { Id = existing.Id, ConfigTypeId = configTypeId, Contents = contents }, transaction);

                    if (rowsAffected == 0)
                    {
                        logWriter?.LogError(
                            $"Update of config '{existing.ConfigName}' (id {existing.Id}, type {configTypeId}) affected zero rows.",
                            nameof(ConfigWriter), nameof(UpsertAsync));
                    }
                    else
                    {
                        logWriter?.LogInfo(
                            $"Updated config '{existing.ConfigName}' (id {existing.Id}, type {configTypeId}, matched by {existing.MatchedBy}, rows {rowsAffected}).",
                            nameof(ConfigWriter), nameof(UpsertAsync));
                    }

                    return new ConfigWriteResult
                    {
                        ConfigId = existing.Id,
                        ConfigName = existing.ConfigName,
                        Action = rowsAffected == 0 ? "NotFound" : "Updated"
                    };
                }
            }

            var nameToInsert = await GetNextAvailableNameAsync(connect, transaction, normalisedName);

            var insertSql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                              values(@ConfigName, @ConfigTypeId, cast(@Contents as json), now()) returning Id";
            var insertedId = await connect.QueryFirstAsync<int>(insertSql,
                new { ConfigName = nameToInsert, ConfigTypeId = configTypeId, Contents = contents }, transaction);

            logWriter?.LogInfo(
                $"Inserted config '{nameToInsert}' (id {insertedId}, type {configTypeId}) for requested name '{normalisedName}'.",
                nameof(ConfigWriter), nameof(UpsertAsync));

            return new ConfigWriteResult
            {
                ConfigId = insertedId,
                ConfigName = nameToInsert,
                Action = "Inserted"
            };
        }

        /// <summary>
        /// Deletes a configuration record, matching on the configs identity first and falling back to
        /// the configuration name scoped by ConfigTypeId.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="transaction">The transaction the delete must take part in.</param>
        /// <param name="configId">The configs identity of the record, when the designer loaded one.</param>
        /// <param name="configName">The configuration name of the record.</param>
        /// <param name="configTypeId">The configuration type the record belongs to.</param>
        /// <param name="logWriter">Log writer used to record what happened.</param>
        /// <returns>The identity, name and action taken for the record.</returns>
        public async Task<ConfigWriteResult> DeleteAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            int? configId,
            string configName,
            int configTypeId,
            ILogWriter logWriter)
        {
            var normalisedName = configName.NormalisedConfigName();
            var existing = await FindExistingAsync(connect, transaction, configId, normalisedName, configTypeId, logWriter);

            if (existing == null)
            {
                logWriter?.LogInfo(
                    $"No config of type {configTypeId} found for id {configId?.ToString() ?? "none"} / name '{normalisedName}', nothing to delete.",
                    nameof(ConfigWriter), nameof(DeleteAsync));

                return new ConfigWriteResult { ConfigId = 0, ConfigName = normalisedName, Action = "NotFound" };
            }

            var deleteSql = @"delete from Configs where Id = @Id and ConfigTypeId = @ConfigTypeId";
            var rowsAffected = await connect.ExecuteAsync(deleteSql,
                new { Id = existing.Id, ConfigTypeId = configTypeId }, transaction);

            logWriter?.LogInfo(
                $"Deleted config '{existing.ConfigName}' (id {existing.Id}, type {configTypeId}, matched by {existing.MatchedBy}, rows {rowsAffected}).",
                nameof(ConfigWriter), nameof(DeleteAsync));

            return new ConfigWriteResult
            {
                ConfigId = existing.Id,
                ConfigName = existing.ConfigName,
                Action = rowsAffected == 0 ? "NotFound" : "Deleted"
            };
        }

        /// <summary>
        /// Reads the stored contents of a configuration record so a caller can preserve values the
        /// client never sees in their stored form, such as untranslated @Tag@ language tokens.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="transaction">The transaction the lookup must take part in.</param>
        /// <param name="configId">The configs identity to try first, when supplied.</param>
        /// <param name="configName">The configuration name to fall back to.</param>
        /// <param name="configTypeId">The configuration type to scope both lookups by.</param>
        /// <returns>The stored JSON contents, or null when the record does not exist.</returns>
        public async Task<string> ReadContentsAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            int? configId,
            string configName,
            int configTypeId)
        {
            var existing = await FindExistingAsync(connect, transaction, configId, configName.NormalisedConfigName(), configTypeId);

            if (existing == null)
            {
                return null;
            }

            var sql = @"select Contents from Configs where Id = @Id";
            return await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = existing.Id }, transaction);
        }

        /// <summary>
        /// Finds an existing configuration record by identity, then by name scoped to the configuration type.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="transaction">The transaction the lookups must take part in.</param>
        /// <param name="configId">The configs identity to try first, when supplied.</param>
        /// <param name="normalisedName">The lower case configuration name to fall back to.</param>
        /// <param name="configTypeId">The configuration type to scope both lookups by.</param>
        /// <param name="logWriter">Log writer used to report a record that exists under a different type.</param>
        /// <returns>The matched record, or null when nothing matched.</returns>
        private static async Task<ExistingConfig> FindExistingAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            int? configId,
            string normalisedName,
            int configTypeId,
            ILogWriter logWriter = null)
        {
            if (configId.HasValue && configId.Value > 0)
            {
                var byIdSql = @"select Id, ConfigName from Configs where Id = @Id and ConfigTypeId = @ConfigTypeId";
                var byId = await connect.QueryFirstOrDefaultAsync<ExistingConfig>(byIdSql,
                    new { Id = configId.Value, ConfigTypeId = configTypeId }, transaction);

                if (byId != null)
                {
                    byId.MatchedBy = "id";
                    return byId;
                }

                await WarnOnTypeMismatchAsync(connect, transaction, configId.Value, configTypeId, logWriter);
            }

            if (normalisedName.Length == 0)
            {
                return null;
            }

            var byNameSql = @"select Id, ConfigName from Configs
                              where Lower(ConfigName) = @ConfigName and ConfigTypeId = @ConfigTypeId";
            var byName = await connect.QueryFirstOrDefaultAsync<ExistingConfig>(byNameSql,
                new { ConfigName = normalisedName, ConfigTypeId = configTypeId }, transaction);

            if (byName != null)
            {
                byName.MatchedBy = "name";
            }

            return byName;
        }

        /// <summary>
        /// Reports a caller asking for a record by identity under a type that record does not belong to.
        /// </summary>
        /// <remarks>
        /// Both lookups here are scoped by type, so this mismatch makes the caller fall through to an
        /// insert under a name the table already holds, which produces a renamed duplicate rather than
        /// the update the caller intended. It went unnoticed for exactly as long as it went unlogged,
        /// so it is reported as an error even though nothing here fails.
        /// </remarks>
        /// <param name="connect">An open database connection.</param>
        /// <param name="transaction">The transaction the lookup must take part in.</param>
        /// <param name="configId">The configs identity that did not match under the requested type.</param>
        /// <param name="configTypeId">The configuration type the caller asked for.</param>
        /// <param name="logWriter">Log writer used to report the mismatch.</param>
        private static async Task WarnOnTypeMismatchAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            int configId,
            int configTypeId,
            ILogWriter logWriter)
        {
            if (logWriter == null)
            {
                return;
            }

            var sql = @"select Id, ConfigName, ConfigTypeId from Configs where Id = @Id";
            var actual = await connect.QueryFirstOrDefaultAsync<ExistingConfig>(sql, new { Id = configId }, transaction);

            if (actual == null)
            {
                return;
            }

            logWriter.LogError(
                $"Config id {configId} ('{actual.ConfigName}') belongs to ConfigType {actual.ConfigTypeId} but was requested as type {configTypeId}. The write will not match it and will insert a renamed duplicate instead.",
                nameof(ConfigWriter), nameof(FindExistingAsync));
        }

        /// <summary>
        /// Reserves a configuration name that is not yet in use anywhere in the configs table.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="transaction">The transaction the lookups must take part in.</param>
        /// <param name="baseName">The name the designer asked for.</param>
        /// <returns>A configuration name that is free at the point the transaction commits.</returns>
        private static async Task<string> GetNextAvailableNameAsync(NpgsqlConnection connect, IDbTransaction transaction, string baseName)
        {
            var queryFilters = new QueryFilterConfig();
            queryFilters.AddString("name", baseName);
            queryFilters.AddString("suffix", string.Empty);

            return await new NextFreeNameQuery().ExecuteAsync(connect, queryFilters, transaction);
        }

        /// <summary>
        /// A configuration record located by <see cref="FindExistingAsync"/>.
        /// </summary>
        private class ExistingConfig
        {
            /// <summary>
            /// Gets or sets the configs identity.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the stored configuration name.
            /// </summary>
            public string ConfigName { get; set; }

            /// <summary>
            /// Gets or sets the configuration type the record belongs to.
            /// Only read when reporting a record found under a type the caller did not ask for.
            /// </summary>
            public int ConfigTypeId { get; set; }

            /// <summary>
            /// Gets or sets how the record was located, for logging: "id" or "name".
            /// </summary>
            public string MatchedBy { get; set; }
        }
    }

    /// <summary>
    /// The outcome of a single configs write.
    /// </summary>
    internal class ConfigWriteResult
    {
        /// <summary>
        /// Gets or sets the configs identity of the record after the write.
        /// </summary>
        public int ConfigId { get; set; }

        /// <summary>
        /// Gets or sets the configuration name actually stored.
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// Gets or sets the action taken: Inserted, Updated, Deleted or NotFound.
        /// </summary>
        public string Action { get; set; }
    }
}
