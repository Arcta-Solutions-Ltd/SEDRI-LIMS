using System.Collections.Generic;

namespace arc.common.Models.Config
{
    public class AddWorkflowEntryModel
    {
        public string EventField { get; set; }
        public string EntryStates { get; set; }
        public string DefaultExitStates { get; set; }
        public string Event { get; set; }
        public string Id { get; set; }
        public string View { get; set; }
        public string Action { get; set; }
        public WorkflowRuleGridModel WorkflowRuleGrid { get; set; }
        public List<StatePassThroughGrid> StatePassThroughGrid { get; set; }
    }

    public class WorkflowRuleGridModel
    {
        public List<WorkflowRuleModel> WorkflowRuleGrid { get; set; }
    }

    public class WorkflowRuleModel
    {
        public string ExitState { get; set; }
        public string Field { get; set; }
        public string StringValue { get; set; }
        public string NumberValue { get; set; }
        public string ListValue { get; set; }
    }

    public class StatePassThroughGrid 
    {
        public string ExitState { get; set; }
        public string EntryState { get; set; }
    }
}

