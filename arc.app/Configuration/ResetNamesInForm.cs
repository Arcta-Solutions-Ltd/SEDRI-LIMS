using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormStructureConfig;
using System.Collections.Generic;

namespace arc.app.Configuration
{
    public class ResetNamesInForm : IResetNamesInForm
    {
        public FullFormConfig Reset(FullFormConfig form, string oldField, string newField, string type)
        {
            for (int pageNum = 0; pageNum < form.PagesConfig.Count; pageNum++)
            {
                form.PagesConfig[pageNum].ChangeFieldName(oldField, newField);
            }

            form.InitialQueryConfig.ResultMapperConfig.ChangeFieldName(oldField, newField, type);
            form.SaveEventConfig.MappingConfig.ChangeFieldName(oldField, newField, type);
            form.DataSectionConfig.ChangeFieldName(oldField, newField);
            form.NewSectionConfig.ChangeFieldName(oldField, newField);

            var newDisplay = new List<DisplayConfig>();
            foreach (var display in form.SaveEventConfig.Display)
            {
                if (display.Label.ToLower() == oldField.ToLower())
                {
                    display.Label = newField;
                }
                newDisplay.Add(display);
            }
            form.SaveEventConfig.Display = newDisplay;

            if (form.SaveEventConfig.ValidationRules != null)
            {
                var newRules = new List<ValidationRuleConfig>();
                foreach (var rule in form.SaveEventConfig.ValidationRules)
                {
                    if (rule.Field.ToLower() == oldField.ToLower())
                    {
                        rule.Field = newField;
                    }
                    newRules.Add(rule);
                }
                form.SaveEventConfig.ValidationRules = newRules;
            }

            return form;
        }
    }
}
