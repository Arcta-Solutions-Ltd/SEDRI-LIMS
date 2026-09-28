using System.Collections.Generic;

namespace arc.common.Models.Config
{
    public class EditFieldModel
    {
        public string Id { get; set; }
        public string FieldId { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public string Required { get; set; }
        public int List { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int Dpts { get; set; }
        public string DefaultToNow { get; set; }
        public int ToggleDefault { get; set; }
        public string MultiSelect { get; set; }

        /// <summary>
        /// Yes when the field shows its value without allowing entry, used for reference copies of
        /// a reused field definition.
        /// </summary>
        public string ReadOnly { get; set; }

        public string ContentTypeIds { get; set; }
        public string RequiredErrorMessage { get; set; }

        /// <summary>
        /// Optional hint text shown inside the field control at data entry.
        /// Stored as literal user text, not a language tag.
        /// </summary>
        public string Placeholder { get; set; }
        public List<GridDefinitionModel> FieldGrid { get; set; }

        // UX inputs for fieldgrid buttons (Yes/No)
        public string IncludeAddButton { get; set; }
        public string IncludeDeleteButton { get; set; }

        /// <summary>
        /// Id of the parent combobox/dropdown field on the same form when this field uses a child list.
        /// </summary>
        public string ParentList { get; set; }

        /// <summary>
        /// Yes when an "Other" option and companion details field should be created for this list field.
        /// </summary>
        public string AllowOther { get; set; }

        /// <summary>
        /// Optional label for the auto-generated Other-details companion field.
        /// Stored as literal user text; when empty the default tag @ConOtherDetails@ is used.
        /// </summary>
        public string OtherDetailsLabel { get; set; }
    }
}
