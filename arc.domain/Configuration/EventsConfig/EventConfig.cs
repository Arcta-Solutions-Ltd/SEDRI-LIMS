using arc.common.ExtensionMethods;
using arc.common.Utils;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.EventsConfig
{
    public class EventConfig
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public string BatchEvent { get; set; }

        /// <summary>
        /// When set on a batch wrapper event, the form field name populated from each selected row's id
        /// before delegating to <see cref="BatchEvent"/> (for example BreakpointId or ExpertRuleId).
        /// </summary>
        public string BatchParentIdField { get; set; }

        public string ExportQuery { get; set; }
        public string TableName { get; set; }
        public string Mapping { get; set; }
        public string StringFields { get; set; }
        public string Lists { get; set; }
        public string DefaultView { get; set; }
        public bool DoNotSaveInQueue { get; set; }

        /// <summary>
        /// When false, specimen workflow resolution is skipped for this event.
        /// When null, patient-scoped tables (patient, patientcomment, patienttag) skip workflow automatically.
        /// </summary>
        public bool? RequiresSpecimenWorkflow { get; set; }

        /// <summary>
        /// Optional path to the object inside the stored payload whose properties are treated as the
        /// display roots, for example <c>Crafted[0].Contents[0].value</c>. Crafted forms nest their
        /// whole payload, so without this the display rows have nothing to match against. When empty
        /// the payload root is used.
        /// </summary>
        public string DisplayRoot { get; set; }

        /// <summary>
        /// Optional nested collections and objects inside the payload that should be rendered as their
        /// own indented blocks. See <see cref="DisplaySectionConfig"/>.
        /// </summary>
        public List<DisplaySectionConfig> DisplaySections { get; set; } = new List<DisplaySectionConfig>();

        public List<ValidationRuleConfig> ValidationRules { get; set; } = new List<ValidationRuleConfig>();
        public IEnumerable<DataRuleEventConfig> DataRules { get; set; }
        public List<DisplayConfig> Display { get; set; } = new List<DisplayConfig>();
        public IEnumerable<TableExceptionConfig> TableExceptions { get; set; }

        /// <summary>
        /// Returns whether this event should run specimen workflow state resolution.
        /// </summary>
        /// <returns>True when workflow resolution should run; false for patient-scoped events.</returns>
        public bool UsesSpecimenWorkflowResolution()
        {
            if (RequiresSpecimenWorkflow == false)
            {
                return false;
            }

            if (RequiresSpecimenWorkflow == true)
            {
                return true;
            }

            return !EventWorkflowTableNames.IsPatientScopedTable(TableName);
        }

        public string ValidateMessage(string message)
        {
            var errorMessage = "";
            if (ValidationRules != null)
            {
                var collector = new JsonWholeStructureFieldsCollector();
                collector.GetStructure(message);
                var fieldDictionary = collector.GetFields();

                foreach (var rule in ValidationRules)
                {
                    errorMessage = rule.Evaluate(fieldDictionary);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        break;
                    }
                }
            }

            return errorMessage;
        }

        public void AddDisplayConfig(string label, string translation, string type)
        {
            var newDisplayConfig = new DisplayConfig
            {
                Label = label,
                Translation = translation,
                List = type == "combobox" | type == "dropdown" ? "Yes" : "No",
                Date = type == "date" ? true : false
            };

            Display.Add(newDisplayConfig);
        }

        public void EditDisplayConfig(string label, string translation, string type)
        {
            var newDisplayConfig = new DisplayConfig
            {
                Label = label,
                Translation = translation,
                List = type == "combobox" | type == "dropdown" ? "Yes" : "No",
                Date = type == "date" ? true : false
            };

            var found = false;
            for (int displayNum = 0; displayNum < Display.Count; displayNum++)
            {
                if (Display[displayNum].Label == label)
                {
                    Display[displayNum] = newDisplayConfig;
                    found = true;
                }
            }

            if (! found)
            {
                Display.Add(newDisplayConfig);
            }
        }

        public void ReorderDisplayConfig(List<string> fieldList)
        {
            if (fieldList != null && fieldList.Count > 0)
            {
                var newDisplay = new List<DisplayConfig>();
                foreach (var fieldLabel in fieldList)
                {
                    var displayIndex = Display.FindIndex(f => f.Translation == fieldLabel);
                    if (displayIndex != -1)
                    {
                        newDisplay.Add(new DisplayConfig()
                        {
                            Date = Display[displayIndex].Date,
                            Label = Display[displayIndex].Label,
                            Translation = Display[displayIndex].Translation,
                            List = Display[displayIndex].List,
                            Resolver = Display[displayIndex].Resolver,
                            Grid = Display[displayIndex].Grid
                        });
                    }
                }

                // Just copy the grid sub-fields across as the order will always be correct.
                foreach (var displayItem in Display)
                {
                    if (newDisplay.FindIndex(f => f.Label == displayItem.Label) == -1)
                    {
                        newDisplay.Add(displayItem);
                    }
                }
                Display = newDisplay;
            }
        }

        public void DeleteDisplayConfig(string label)
        {
            if (Display.Count > 0)
            {

                var displayConfigItemToDelete = Display.FirstOrDefault(d => d.Label.Equals(label, System.StringComparison.CurrentCultureIgnoreCase));

                if (displayConfigItemToDelete != null)
                {
                    Display.Remove(displayConfigItemToDelete);
                }
            }
        }

        public void AddValidationMessage(string field, string message, string rule)
        {
            var newRuleConfig = new ValidationRuleConfig
            {
                Field = field,
                Message = message,
                Rule = rule
            };

            ValidationRules.Add(newRuleConfig);
        }

        public void DeleteValidationMessage(string field)
        {
            if (ValidationRules.Count > 0)
            {
                var itemToDelete = ValidationRules.FirstOrDefault(d => d.Field.Equals(field, System.StringComparison.CurrentCultureIgnoreCase));
                if (itemToDelete != null)
                {
                    ValidationRules.Remove(itemToDelete);
                }
            }
        }

        public ValidationRuleConfig GetValidationRuleForField(string fieldId)
        {

            return  ValidationRules.FirstOrDefault(r => r.Field.Equals(fieldId, System.StringComparison.CurrentCultureIgnoreCase));

        }
    }
}
