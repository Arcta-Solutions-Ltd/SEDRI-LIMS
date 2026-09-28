using arc.app.Common;
using arc.common.Models.Config;
using arc.common.Models.Reports.ReportDesigner;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    /// <summary>
    /// Writes one workflow document back to the configs table inside a single explicit transaction.
    /// </summary>
    /// <remarks>
    /// The write goes through <see cref="ConfigWriter"/>, which matches on the configs identity first
    /// and only falls back to the configuration name scoped by ConfigTypeId. That is what stops a
    /// workflow save landing on a different workflow, or on a report or section record that happens to
    /// share its name.
    /// </remarks>
    internal class SaveWorkflowDesignerConfigCommand : ICommandWithTypeReturningType<SaveWorkflowDesignerConfigModel, WorkflowDesignerSaveResultModel>
    {
        /// <summary>
        /// The ConfigTypeId workflow documents are stored under.
        /// </summary>
        internal const int WorkflowConfigTypeId = 18;

        private readonly ConfigWriter _configWriter = new();

        /// <summary>
        /// Saves the workflow document.
        /// </summary>
        /// <param name="connect">The database connection. Opened here so the write and its lookups share one transaction.</param>
        /// <param name="command">The workflow the designer is saving.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <returns>What was written, so the designer can reconcile before it refetches.</returns>
        public async Task<WorkflowDesignerSaveResultModel> ExecuteAsync(NpgsqlConnection connect, SaveWorkflowDesignerConfigModel command, ILogWriter logWriter = null)
        {
            var result = new WorkflowDesignerSaveResultModel();

            if (command?.Workflow == null)
            {
                logWriter?.LogError(
                    "Workflow designer save was called with no workflow document. Nothing was written.",
                    nameof(SaveWorkflowDesignerConfigCommand), nameof(ExecuteAsync));

                result.Workflow = new ConfigSaveResultModel
                {
                    ConfigTypeId = WorkflowConfigTypeId,
                    RequestedName = command?.ConfigName,
                    State = ConfigChangeState.Modified,
                    Action = "Skipped",
                    SkipReason = "No workflow document was supplied."
                };

                return result;
            }

            var document = PrepareDocument(command.Workflow);
            result.StepCount = CountSteps(document);

            logWriter?.LogInfo(
                $"Saving workflow '{command.ConfigName}' (id {command.ConfigId}) with {result.StepCount} step(s).",
                nameof(SaveWorkflowDesignerConfigCommand), nameof(ExecuteAsync));

            await connect.OpenAsync();
            using var transaction = await connect.BeginTransactionAsync();

            try
            {
                var writeResult = await _configWriter.UpsertAsync(
                    connect,
                    transaction,
                    command.ConfigId,
                    command.ConfigName,
                    WorkflowConfigTypeId,
                    document.ToString(Formatting.None),
                    ConfigChangeState.Modified,
                    logWriter);

                await transaction.CommitAsync();

                result.Workflow = new ConfigSaveResultModel
                {
                    ConfigId = writeResult.ConfigId,
                    ConfigTypeId = WorkflowConfigTypeId,
                    RequestedName = command.ConfigName,
                    SavedName = writeResult.ConfigName,
                    State = ConfigChangeState.Modified,
                    Action = writeResult.Action
                };

                logWriter?.LogInfo(
                    $"Committed workflow save: '{writeResult.ConfigName}' (id {writeResult.ConfigId}) was {writeResult.Action.ToLower()} with {result.StepCount} step(s).",
                    nameof(SaveWorkflowDesignerConfigCommand), nameof(ExecuteAsync));

                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                logWriter?.LogError(
                    $"Rolled back the save of workflow '{command.ConfigName}' (id {command.ConfigId}): {e.Message}",
                    nameof(SaveWorkflowDesignerConfigCommand), nameof(ExecuteAsync));
                throw;
            }
        }

        /// <summary>
        /// Removes the properties that belong to the configs row rather than to the document.
        /// </summary>
        /// <remarks>
        /// <c>Id</c> is the configs identity and is carried on the row, so storing it inside contents
        /// would leave a second copy that nothing maintains and that would be wrong the moment a
        /// workflow was exported and imported elsewhere.
        /// </remarks>
        /// <param name="workflow">The document the designer sent.</param>
        /// <returns>A copy of the document with the row level properties stripped.</returns>
        private static JObject PrepareDocument(JObject workflow)
        {
            var document = (JObject)workflow.DeepClone();
            document.Remove("Id");
            document.Remove("id");
            return document;
        }

        /// <summary>
        /// Counts the steps in the document being stored, so a save that silently truncated the
        /// workflow is visible in the log and to the caller.
        /// </summary>
        /// <param name="document">The document about to be written.</param>
        /// <returns>The number of steps, or zero when the document has none.</returns>
        private static int CountSteps(JObject document)
        {
            var steps = document.GetValue("Steps", StringComparison.OrdinalIgnoreCase) as JArray;
            return steps?.Count ?? 0;
        }
    }
}
