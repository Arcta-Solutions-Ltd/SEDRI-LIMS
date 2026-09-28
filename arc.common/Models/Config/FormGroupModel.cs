using System.Collections.Generic;

namespace arc.common.Models.Config
{
    /// <summary>
    /// Model for add/edit form group API payloads.
    /// Reuses FieldListModel for ordered field ids; rules match domain RuleConfig structure.
    /// </summary>
    public class FormGroupModel : AddPageModel
    {
        /// <summary>
        /// Form group key identifier.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Visibility and validation rules for the form group.
        /// Structure matches domain RuleConfig: Effect, Field, Rule, Value.
        /// </summary>
        public List<FormGroupRuleModel> Rules { get; set; }

        /// <summary>
        /// Ordered list of field ids in this form group.
        /// Reuses FieldListModel pattern: Label = display, Value = field id.
        /// </summary>
        public List<FieldListModel> FieldList { get; set; }
    }

    /// <summary>
    /// Rule model for form group visibility/validation.
    /// Matches domain RuleConfig structure for mapping.
    /// </summary>
    public class FormGroupRuleModel
    {
        public string Effect { get; set; }
        public string Field { get; set; }
        public string Rule { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Model for Move to Form Group API payload.
    /// Id = formName|pageName|fieldId (source field). TargetFormGroupId = formName|pageName|columnKey|formGroupKey.
    /// </summary>
    public class MoveFieldModel
    {
        public string Id { get; set; }

        /// <summary>
        /// Target page name (id). Used by the move-field form UI; persistence uses TargetFormGroupId.
        /// </summary>
        public string TargetPageId { get; set; }

        public string TargetFormGroupId { get; set; }
    }

    /// <summary>
    /// Payload for moving a form group (subsection) to another page within the same form.
    /// Id = formName|pageName|columnKey|formGroupKey (source).
    /// TargetPageId = target page name (id, not title).
    /// </summary>
    public class MoveFormGroupModel
    {
        public string Id { get; set; }
        public string TargetPageId { get; set; }
    }
}
