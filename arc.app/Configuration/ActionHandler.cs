using arc.app.Actions;
using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.WorkflowsConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Resolves workflow <see cref="ActionConditionConfig"/> entries and runs the matching <see cref="IAction"/>
    /// with a <see cref="QueryFilterConfig"/> built from the specimen record id and workflow parameter map.
    /// </summary>
    public class ActionHandler : IActionHandler
    {
        private readonly IActionFactory _actionFactory;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionHandler"/> class.
        /// </summary>
        /// <param name="actionFactory">Factory that resolves actions by name.</param>
        /// <param name="logWriter">Application log writer for operational diagnostics.</param>
        public ActionHandler(IActionFactory actionFactory, ILogWriter logWriter)
        {
            _actionFactory = actionFactory;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Runs the configured side-effect for the current workflow transition.
        /// Builds query parameters from the authoritative record id (specimen id) plus each entry in
        /// <paramref name="action"/>.<see cref="ActionConditionConfig.Parameters"/> (excluding any
        /// <c>id</c> key from configuration so the server-resolved id always wins).
        /// </summary>
        /// <param name="action">Workflow action metadata including name and parameter map.</param>
        /// <param name="message">Original event message JSON (used to obtain an id when not passed explicitly).</param>
        /// <param name="newRecordId">Preferred record id model from the event pipeline.</param>
        /// <param name="token">Current user and laboratory context.</param>
        public async Task CarryOutAction(ActionConditionConfig action, string message, IdModel newRecordId, TokenInfoModel token)
        {
            var recordId = newRecordId != null && newRecordId.Id != "" ? newRecordId : JsonConvert.DeserializeObject<IdModel>(message);

            if (string.IsNullOrEmpty(action?.Action))
            {
                return;
            }

            if (recordId == null || string.IsNullOrEmpty(recordId.Id))
            {
                _logWriter.LogInfo(
                    $"Warning: workflow action '{action.Action}' skipped — no record id in context.",
                    nameof(ActionHandler),
                    nameof(CarryOutAction));
                return;
            }

            var newAction = _actionFactory.Get(action.Action, token);
            if (newAction == null)
            {
                _logWriter.LogInfo(
                    $"Warning: no IAction registered for workflow action '{action.Action}'.",
                    nameof(ActionHandler),
                    nameof(CarryOutAction));
                return;
            }

            var filter = BuildQueryFilter(recordId, action);

            var paramKeys = action.Parameters == null || action.Parameters.Count == 0
                ? "(none)"
                : string.Join(", ", action.Parameters.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase));

            _logWriter.LogInfo(
                $"Workflow action '{action.Action}' recordId={recordId.Id} parameterKeys={paramKeys}",
                nameof(ActionHandler),
                nameof(CarryOutAction));

            await newAction.Do(filter, token);
        }

        private static QueryFilterConfig BuildQueryFilter(IdModel recordId, ActionConditionConfig action)
        {
            var list = new List<QueryValuesConfig>
            {
                new() { Key = "id", Value = recordId.Id },
            };

            if (action.Parameters != null)
            {
                foreach (var pair in action.Parameters)
                {
                    if (string.Equals(pair.Key, "id", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    list.Add(new QueryValuesConfig { Key = pair.Key, Value = pair.Value });
                }
            }

            return new QueryFilterConfig { Parameters = list };
        }
    }
}
