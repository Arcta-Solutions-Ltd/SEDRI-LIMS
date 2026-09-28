using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.PagesConfig
{
    /// <summary>
    /// Represents the configuration for a page in the application.
    /// This class defines the structure, layout, and behavior of a page including its fields, columns, and navigation controls.
    /// </summary>
    public class PageConfig
    {
        /// <summary>
        /// Gets or sets the unique name identifier for the page configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title displayed at the top of the page.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the title displayed in the view/UI component.
        /// </summary>
        public string ViewTitle { get; set; }

        /// <summary>
        /// Gets or sets additional descriptive text for the page.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page can be finished optionally.
        /// </summary>
        public bool OptionalFinish { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page configuration is custom crafted.
        /// </summary>
        public bool Crafted { get; set; }

        /// <summary>
        /// Gets or sets the required field validation rule for the page.
        /// </summary>
        public string Required { get; set; }

        /// <summary>
        /// Gets or sets the specific rule that determines if the page is required.
        /// </summary>
        public string RequiredRule { get; set; }

        /// <summary>
        /// Gets or sets the error message to display when required validation fails.
        /// </summary>
        public string RequiredError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be displayed with wider layout.
        /// </summary>
        public bool Wider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be displayed with wide layout.
        /// </summary>
        public bool Wide { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be displayed with extra wide layout.
        /// </summary>
        public bool ExtraWide { get; set; }

        /// <summary>
        /// Gets or sets the initial entry state of the page (e.g., "new", "edit", "view").
        /// </summary>
        public string EntryState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page can be collapsed.
        /// </summary>
        public bool Collapsible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be initially expanded.
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets the name of the query associated with this page.
        /// </summary>
        public string QueryName { get; set; }

        /// <summary>
        /// Gets or sets which parent record the request selection page lists requests for, either
        /// <c>patient</c> or <c>admission</c>. Only read by the request selection page.
        /// </summary>
        public string RequestScope { get; set; }

        /// <summary>
        /// Gets or sets the configuration for actions available on this page.
        /// </summary>
        public string ConfigureActions { get; set; }

        /// <summary>
        /// Gets or sets the id of the page group this page belongs to, for example <c>patient</c>.
        /// Null or empty when the page is ungrouped. Pages sharing a group id must stay together
        /// in the form page order. Not exposed directly in the configuration UI; for user added
        /// pages it is derived from <see cref="TableName"/>.
        /// </summary>
        public string PageGroup { get; set; }

        /// <summary>
        /// Gets or sets the one based locked position of this page within its group.
        /// Zero means the page is a freely orderable member that must follow all anchors.
        /// </summary>
        public int GroupAnchor { get; set; }

        /// <summary>
        /// Gets or sets the name of the database table associated with this page.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the next button on this page.
        /// </summary>
        public NextButtonConfig NextButton { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the cancel button on this page.
        /// </summary>
        public NextButtonConfig CancelButton { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the previous button on this page.
        /// </summary>
        public NextButtonConfig PrevButton { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the next item button on this page.
        /// </summary>
        public NextButtonConfig NextItemButton { get; set; }

        /// <summary>
        /// Gets or sets the list of column configurations that define the page layout structure.
        /// Each column can contain form groups and fields.
        /// </summary>
        public List<ColumnConfig> Columns { get; set; } = new List<ColumnConfig>();

        /// <summary>
        /// Gets a field configuration by name (case-insensitive search across all columns, formgroups, and fields).
        /// </summary>
        /// <param name="fieldName">The field identifier to search for.</param>
        /// <returns>The matching <see cref="FieldConfig"/> or null if not found.</returns>
        internal FieldConfig GetField(string fieldName)
        {
            if (Columns == null || string.IsNullOrEmpty(fieldName)) return null;

            foreach (var column in Columns)
            {
                if (column.FormGroups == null) continue;
                foreach (var formGroup in column.FormGroups)
                {
                    if (formGroup.Fields == null) continue;
                    foreach (var field in formGroup.Fields)
                    {
                        if (!string.IsNullOrEmpty(field?.Id) && field.Id.Equals(fieldName, System.StringComparison.OrdinalIgnoreCase))
                        {
                            return field;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Ensures that PrintOnReport is in its own final form group on the page and is the last field.
        /// If PrintOnReport exists, it is moved to the last column's final formgroup, which is made dedicated.
        /// </summary>
        /// <param name="printOnReportField">Optional field definition for PrintOnReport. If provided, uses this instead of looking it up on the page.</param>
        public void EnsurePrintOnReportIsLast(FieldConfig printOnReportField = null)
        {
            if (Columns == null || Columns.Count == 0) return;

            var field = printOnReportField ?? GetField("PrintOnReport");
            if (field == null) return; // no PrintOnReport on this page

            var lastColumn = Columns.Last();
            // Remove from original location
            DeleteField(field.Id);

            // ensure at least one formgroup in last column
            if (lastColumn.FormGroups == null || lastColumn.FormGroups.Count == 0)
            {
                lastColumn.FormGroups = new List<FormGroupConfig> { new FormGroupConfig { Key = "fg1" } };
            }

            // If the last group's not dedicated, create a new final group for PrintOnReport
            var finalGroup = lastColumn.FormGroups.Last();
            if (finalGroup.Fields.Count != 0)
            {
                finalGroup = new FormGroupConfig { Key = $"fg{lastColumn.FormGroups.Count + 1}" };
                lastColumn.FormGroups.Add(finalGroup);
            }

            // Make it the only field in the final group
            finalGroup.DeleteAllFields();
            finalGroup.AddField(field);
        }

        /// <summary>
        /// Adds a new field to the first column's last form group.
        /// User-defined form groups are preserved: new fields are added to the last form group
        /// (or the last non-PrintOnReport group). Form structure is managed via Form Definition
        /// (add/edit/delete form group); this method does not create or flatten form groups.
        /// </summary>
        /// <param name="newField">The field configuration to add.</param>
        /// <param name="orderIndex">The position to insert the field at. If -1, adds to the end.</param>
        public void AddField(FieldConfig newField, int orderIndex = -1)
        {
            // Determine target column
            var targetColumn = Columns.First();
            // If the last formgroup of the last column is dedicated to PrintOnReport, avoid adding into it
            var lastColumn = Columns.Last();
            var lastGroup = lastColumn.GetLastFormGroup();
            var lastGroupHasPrintOnly = lastGroup.Fields.Count == 1 && lastGroup.Fields[0].Id.Equals("PrintOnReport", System.StringComparison.OrdinalIgnoreCase);

            if (lastGroupHasPrintOnly)
            {
                // Add to previous formgroup in last column if available; otherwise create a new group before the final group
                if (lastColumn.FormGroups.Count > 1)
                {
                    var prevGroup = lastColumn.FormGroups[lastColumn.FormGroups.Count - 2];
                    prevGroup.AddField(newField, orderIndex);
                }
                else
                {
                    // Insert a new group before the final group
                    var newGroup = new FormGroupConfig { Key = $"fg{lastColumn.FormGroups.Count + 1}" };
                    lastColumn.FormGroups.Insert(lastColumn.FormGroups.Count - 1, newGroup);
                    newGroup.AddField(newField, orderIndex);
                }
            }
            else
            {
                var formGroupToAddTo = targetColumn.GetLastFormGroup();
                formGroupToAddTo.AddField(newField, orderIndex);
            }

            // Enforce final state for PrintOnReport on this page
            EnsurePrintOnReportIsLast();
        }

        /// <summary>
        /// Adds a new column to the page configuration.
        /// </summary>
        /// <param name="newColumn">The column configuration to add.</param>
        public void AddColumn(ColumnConfig newColumn)
        {
            Columns.Add(newColumn);
        }

        /// <summary>
        /// Adds a new form group to the last column.
        /// </summary>
        /// <param name="newFormGroup">The form group configuration to add.</param>
        public void AddFormGroup(FormGroupConfig newFormGroup)
        {
            var formColumnToAddTo = Columns.Last();
            formColumnToAddTo.AddFormGroup(newFormGroup);
        }

        /// <summary>
        /// Edits an existing field by finding it by ID and replacing it with the updated field configuration.
        /// </summary>
        /// <param name="fieldToEdit">The field configuration with updated values.</param>
        public void EditField(FieldConfig fieldToEdit)
        {
            for (int colNum = 0; colNum < Columns.Count; colNum++)
            {
                for (int groupNum = 0; groupNum < Columns[colNum].FormGroups.Count; groupNum++)
                {
                    for (int fieldNum = 0; fieldNum < Columns[colNum].FormGroups[groupNum].Fields.Count; fieldNum++)
                    {
                        if (Columns[colNum].FormGroups[groupNum].Fields[fieldNum].Id.ToLower() == fieldToEdit.Id.ToLower())
                        {
                            Columns[colNum].FormGroups[groupNum].Fields[fieldNum] = fieldToEdit;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a field from all form groups across all columns by field name.
        /// </summary>
        /// <param name="fieldName">The name/ID of the field to delete.</param>
        public void DeleteField(string fieldName)
        {
            for (int colNum = 0; colNum < Columns.Count; colNum++)
            {
                for (int groupNum = 0; groupNum < Columns[colNum].FormGroups.Count; groupNum++)
                {
                    Columns[colNum].FormGroups[groupNum].DeleteField(fieldName);
                }
            }
        }

        /// <summary>
        /// Deletes all fields from all form groups across all columns.
        /// </summary>
        public void DeleteAllFields()
        {
            for (int colNum = 0; colNum < Columns.Count; colNum++)
            {
                for (int groupNum = 0; groupNum < Columns[colNum].FormGroups.Count; groupNum++)
                {
                    Columns[colNum].FormGroups[groupNum].DeleteAllFields();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of fields from all columns and form groups, optionally filtered by field type.
        /// </summary>
        /// <param name="typeToMatch">The field type to filter by. If empty or null, returns all fields.</param>
        /// <param name="tableNameInForm">The table name to assign to fields if TableName is not set.</param>
        /// <returns>A list of field configurations matching the criteria.</returns>
        public List<FieldConfig> GetFieldList(string typeToMatch = "", string tableNameInForm = null)
        {
            //if (Name.Is("PatientDetailsPageConfig"))
            //{
            //}
            var fieldList = new List<FieldConfig>();
            var hasTypeFilter = !string.IsNullOrEmpty(typeToMatch);
            var resolvedTableName = string.IsNullOrEmpty(TableName) ? tableNameInForm : TableName;
            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    if (column?.FormGroups == null)
                    {
                        continue;
                    }

                    foreach (var group in column.FormGroups)
                    {
                        if (group?.Fields == null)
                        {
                            continue;
                        }

                        if (!hasTypeFilter)
                        {
                            foreach (var field in group.Fields)
                            {
                                if (field == null)
                                {
                                    continue;
                                }

                                field.TableName = resolvedTableName;
                                fieldList.Add(field);
                            }
                        }
                        else
                        {
                            foreach (var field in group.Fields)
                            {
                                if (!string.IsNullOrEmpty(field.Type) && string.Equals(field.Type, typeToMatch, System.StringComparison.OrdinalIgnoreCase))
                                {
                                    field.TableName = resolvedTableName;
                                    fieldList.Add(field);
                                }
                            }
                        }
                    }
                }
            }
            return fieldList;
        }

        /// <summary>
        /// Changes the name of a field and updates any references to it in ParentList properties.
        /// </summary>
        /// <param name="oldName">The current name/ID of the field to rename.</param>
        /// <param name="newName">The new name/ID to assign to the field.</param>
        public void ChangeFieldName(string oldName, string newName)
        {
            if ((!string.IsNullOrEmpty(oldName) && oldName.Equals("PrintOnReport", System.StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(newName) && newName.Equals("PrintOnReport", System.StringComparison.OrdinalIgnoreCase)))
            {
                // Disallow renaming from/to PrintOnReport
                return;
            }
            for (int colNum = 0; colNum < Columns.Count; colNum++)
            {
                for (int groupNum = 0; groupNum < Columns[colNum].FormGroups.Count; groupNum++)
                {
                    for (int fieldNum = 0; fieldNum < Columns[colNum].FormGroups[groupNum].Fields.Count; fieldNum++)
                    {
                        if (Columns[colNum].FormGroups[groupNum].Fields[fieldNum].Id == oldName)
                        {
                            Columns[colNum].FormGroups[groupNum].Fields[fieldNum].Id = newName;
                        }
                        if (Columns[colNum].FormGroups[groupNum].Fields[fieldNum].ParentList == oldName)
                        {
                            Columns[colNum].FormGroups[groupNum].Fields[fieldNum].ParentList = newName;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reorders fields in the page according to the specified field list order.
        /// This method creates a new single column with fields in the specified order.
        /// </summary>
        /// <param name="newFieldListOrder">A list of field names in the desired order.</param>
        public void ReorderFields(List<string> newFieldListOrder)
        {
            var orderingObject = new ReOrderFieldsInPage(Columns);
            var newColumn = orderingObject.ReorderFields(newFieldListOrder);
            Columns = new List<ColumnConfig>();
            Columns.Add(newColumn);
            // Preserve PrintOnReport invariant after reorder
            EnsurePrintOnReportIsLast();
        }

        /// <summary>
        /// Retrieves a list of grid field configurations for a specific field name across all columns.
        /// </summary>
        /// <param name="fieldName">The name of the field to search for.</param>
        /// <returns>A list of grid field configurations matching the field name.</returns>
        public List<FieldGridConfig> GetGridFieldList(string fieldName)
        {
            var returnList = new List<FieldGridConfig>();
            foreach (var column in Columns)
            {
                returnList.AddRange(column.GetGridFieldList(fieldName));
            }
            return returnList;
        }

        public List<string> GetUIEvents()
        {
            var uiEvents = Columns.SelectMany(group => group.GetUIEvents()).ToList();
            return uiEvents;
        }

    }
}
