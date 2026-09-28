using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.UIEventsConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.FormStructureConfig;

/// <summary>
/// Represents the full, mutable form configuration including pages, queries,
/// events, reports, and supporting structures used during runtime updates.
/// </summary>
public class FullFormConfig : FormConfig
{
    /// <summary>
    /// Query configuration used for initial list/data retrieval associated with the form.
    /// </summary>
    public FullQueryConfig InitialQueryConfig { get; set; }
    /// <summary>
    /// Query configuration used to build the record view and its mapper.
    /// </summary>
    public FullQueryConfig RecordViewQueryConfig { get; set; }
    /// <summary>
    /// Strongly-typed page configurations contained in this form.
    /// </summary>
    public List<PageConfig> PagesConfig { get; set; }
    /// <summary>
    /// UI event configuration associated with the form (navigation, actions).
    /// </summary>
    public UIEventConfig UIEventConfig { get; set; }
    /// <summary>
    /// Event configuration used when persisting form data.
    /// </summary>
    public FullEventConfig SaveEventConfig { get; set; }
    /// <summary>
    /// Data section configuration used by downstream components (e.g., reports).
    /// </summary>
    public DataSectionConfig DataSectionConfig { get; set; } = new DataSectionConfig();
    /// <summary>
    /// Default/seed report section configuration for creating new report sections.
    /// </summary>
    public ReportSectionConfig NewSectionConfig { get; set; } = new ReportSectionConfig();
    /// <summary>
    /// Collection of report section configurations tied to this form.
    /// </summary>
    public List<ReportSectionConfig> ReportSectionConfigList { get; set; } = new List<ReportSectionConfig>();
    /// <summary>
    /// Collection of report configurations tied to this form.
    /// </summary>
    public List<ReportConfig> ReportConfigList { get; set; } = new List<ReportConfig>();

    /// <summary>
    /// Form type discriminator used by consumers to alter behavior.
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// Returns the last page in the form configuration, or null if none exist.
    /// </summary>
    private PageConfig GetLastPage()
    {
        return PagesConfig == null || PagesConfig.Count == 0 ? null : PagesConfig.Last();
    }

    /// <summary>
    /// Gets a field configuration by name (case-insensitive search across all pages).
    /// </summary>
    /// <param name="fieldName">The field identifier to search for.</param>
    /// <returns>The matching <see cref="FieldConfig"/> or null if not found.</returns>
    private FieldConfig GetField(string fieldName)
    {
        if (PagesConfig == null || string.IsNullOrEmpty(fieldName)) return null;

        foreach (var page in PagesConfig)
        {
            var field = page.GetField(fieldName);
            if (field != null) return field;
        }
        return null;
    }

    /// <summary>
    /// Moves the PrintOnReport field to the last page, in its own final formgroup, as the last field.
    /// Creates the formgroup if needed and ensures it is last.
    /// </summary>
    private void MovePrintOnReportToLastPage()
    {
        var field = GetField("PrintOnReport");
        if (field == null) return;

        // Find which page contains the field to delete it from there
        PageConfig originPage = null;
        foreach (var page in PagesConfig)
        {
            if (page.GetField("PrintOnReport") != null)
            {
                originPage = page;
                break;
            }
        }

        if (originPage == null) return;

        // Remove from origin page
        originPage.DeleteField(field.Id);

        // Target last page and place field correctly
        var lastPage = GetLastPage();
        if (lastPage == null) return;

        lastPage.EnsurePrintOnReportIsLast(field);
    }

    /// <summary>
    /// Adds a new page with a default column and form group to the form configuration.
    /// </summary>
    /// <param name="newPageName">Unique page name.</param>
    /// <param name="title">Display title for the page.</param>
    /// <param name="text">Descriptive text for the page.</param>
    /// <param name="actions">Configure actions string (e.g., "add,edit").</param>
    /// <param name="tableName">Database table for fields on this page (specimen, patient, admission, request).</param>
    public void AddPage(string newPageName, string title, string text, string actions, string tableName = "specimen")
    {
        var newPage = new PageConfig
        {
            Name = newPageName,
            PageTitle = title,
            Text = text,
            ConfigureActions = actions,
            TableName = string.IsNullOrWhiteSpace(tableName) ? null : tableName
        };

        var newColumn = new ColumnConfig { Key = "col1" };
        var newFormGroup = new FormGroupConfig { Key = "fg1" };
        newColumn.AddFormGroup(newFormGroup);
        newPage.AddColumn(newColumn);

        PagesConfig.Add(newPage);
        Pages.Add(newPageName.ToLower());

        // After adding a page to the end, ensure PrintOnReport is on the new last page
        var field = GetField("PrintOnReport");
        if (field != null)
        {
            MovePrintOnReportToLastPage();
        }
    }

    /// <summary>
    /// Removes a page from the form configuration by name.
    /// </summary>
    /// <param name="pageName">The name of the page to remove.</param>
    public void RemovePage(string pageName)
    {
        pageName = pageName.ToLower();
        var pageToRemove = PagesConfig.Where(p => p.Name.Equals(pageName, System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        if (pageToRemove == null) return;

        var field = GetField("PrintOnReport");

        if (PagesConfig.Count == 1 && field != null)
        {
            return;
        }

        PagesConfig.Remove(pageToRemove);
        Pages.Remove(pageName);

        if (field != null)
        {
            var lastPage = GetLastPage();
            if (lastPage != null)
            {
                lastPage.EnsurePrintOnReportIsLast(field);
            }
        }
    }

    /// <summary>
    /// Gets a page configuration by name (case-insensitive). Null or unnamed pages are ignored.
    /// </summary>
    /// <param name="pageName">The page name to retrieve.</param>
    /// <returns>The matching <see cref="PageConfig"/> or null if not found.</returns>
    public PageConfig GetPage(string pageName)
    {
        if (PagesConfig == null || string.IsNullOrWhiteSpace(pageName))
        {
            return null;
        }

        pageName = pageName.ToLower();
        return PagesConfig.FirstOrDefault(p =>
            p?.Name != null && p.Name.Equals(pageName, System.StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Adds a field to the specified page and updates all dependent configurations (save event display/validation,
    /// data section, initial query, mapping configs, and record view mapper).
    /// </summary>
    /// <param name="fieldToAdd">The field configuration to add.</param>
    /// <param name="pageToAddFieldTo">Target page name for insertion.</param>
    /// <param name="errorMessage">Validation message used when the field is required.</param>
    /// <param name="orderIndex">Optional insertion index; -1 appends.</param>
    public void AddField(FieldConfig fieldToAdd, string pageToAddFieldTo, string errorMessage, int orderIndex = -1)
    {
        foreach (var page in PagesConfig)
        {
            if (page.Name.Equals(pageToAddFieldTo, System.StringComparison.CurrentCultureIgnoreCase))
            {
                var isLastPage = ReferenceEquals(page, GetLastPage());
                if (isLastPage)
                {
                    // Ensure PrintOnReport is on the last page before adding
                    var field = GetField("PrintOnReport");
                    if (field != null)
                    {
                        // Find which page contains the field
                        PageConfig fieldPage = null;
                        foreach (var p in PagesConfig)
                        {
                            if (p.GetField("PrintOnReport") != null)
                            {
                                fieldPage = p;
                                break;
                            }
                        }

                        if (fieldPage != null && !ReferenceEquals(fieldPage, page))
                        {
                            MovePrintOnReportToLastPage();
                        }
                    }
                }

                page.AddField(fieldToAdd, orderIndex);
                // Enforce PrintOnReport placement within the page
                page.EnsurePrintOnReportIsLast();
            }
        }

        SaveEventConfig.AddDisplayConfig(fieldToAdd.Id, fieldToAdd.Label, fieldToAdd.Type);
        if (fieldToAdd.GridFields != null)
        {
            foreach (var field in fieldToAdd.GridFields)
            {
                SaveEventConfig.AddDisplayConfig(field.Id, field.Id, field.Type);
            }
        }
        if (fieldToAdd.Required && !string.IsNullOrEmpty(errorMessage))
        {
            SaveEventConfig.AddValidationMessage(fieldToAdd.Id, errorMessage, "required");
        }

        DataSectionConfig.AddField(fieldToAdd.Label, fieldToAdd.Id, fieldToAdd.Type);

        if (InitialQueryConfig != null && fieldToAdd.Type != "fieldgrid" && SaveEventConfig.TableName != "Tests" && SaveEventConfig.TableName != "CultureTests")
        {
            switch (fieldToAdd.Type.ToString())
            {
                case "date":
                    InitialQueryConfig.AddField(fieldToAdd.Id, "jdate");
                    break;
                case "number":
                    InitialQueryConfig.AddField(fieldToAdd.Id, "int");
                    break;
                default:
                    InitialQueryConfig.AddField(fieldToAdd.Id, "string");
                    break;
            };
        }

        if (SaveEventConfig != null && SaveEventConfig.MappingConfig != null)
        {
            SaveEventConfig.MappingConfig.AddField(fieldToAdd.Id, fieldToAdd.Type, fieldToAdd.GridFields);
        }

        if (InitialQueryConfig != null && InitialQueryConfig.ResultMapperConfig != null)
        {
            InitialQueryConfig.ResultMapperConfig.AddField(fieldToAdd.Id, fieldToAdd.Type, fieldToAdd.GridFields);
        }
    }

    /// <summary>
    /// Deletes a field from all pages and removes associated display, validation, query and mapping entries.
    /// </summary>
    /// <param name="fieldToDelete">The field identifier to remove.</param>
    public void DeleteField(string fieldToDelete)
    {
        if (!string.IsNullOrEmpty(fieldToDelete) && fieldToDelete.Equals("PrintOnReport", System.StringComparison.OrdinalIgnoreCase))
        {
            // Never delete PrintOnReport
            return;
        }
        var fieldConfig = GetFieldFromForm(fieldToDelete);

        foreach (var page in PagesConfig)
        {
            //if (page.Name.ToLower() == pageToDeleteFieldFrom.ToLower())
            //{
            page.DeleteField(fieldToDelete);
            //}
        }

        SaveEventConfig.DeleteDisplayConfig(fieldToDelete);
        if (fieldConfig.GridFields != null)
        {
            foreach (var field in fieldConfig.GridFields)
            {
                SaveEventConfig.DeleteDisplayConfig(field.Id);
            }
        }

        SaveEventConfig.DeleteValidationMessage(fieldToDelete);
        DataSectionConfig.DeleteField(fieldToDelete);

        foreach(var reportSection in ReportSectionConfigList)
        {
            reportSection.DeleteField(fieldToDelete);
        }


        if (InitialQueryConfig != null)
        {
            InitialQueryConfig.DeleteField(fieldToDelete);

            if (InitialQueryConfig.ResultMapperConfig != null)
            {
                InitialQueryConfig.ResultMapperConfig.DeleteField(fieldToDelete, fieldConfig.Type);
            }
        }

        if (SaveEventConfig.MappingConfig != null)
        {
            SaveEventConfig.MappingConfig.DeleteField(fieldToDelete, fieldConfig.Type);
        }
    }

    /// <summary>
    /// Returns all dropdown/combobox fields across the form.
    /// </summary>
    public List<FieldConfig> GetListFieldsForForm()
    {
        var listOfFields = new List<FieldConfig>();
        foreach (var pageConfig in PagesConfig)
        {
            listOfFields.AddRange(pageConfig.GetFieldList("dropdown"));
            listOfFields.AddRange(pageConfig.GetFieldList("combobox"));
            listOfFields.AddRange(pageConfig.GetFieldList("hierarchicalpicker"));
        }
        return listOfFields;
    }

    /// <summary>
    /// Returns all fields marked as comment fields across the form.
    /// </summary>
    public List<FieldConfig> GetCommentFieldsForForm()
    {
        var listOfFields = new List<FieldConfig>();
        foreach (var pageConfig in PagesConfig)
        {
            listOfFields.AddRange(pageConfig.GetFieldList().Where(p => p.IsComment));
        }
        return listOfFields;
    }

    /// <summary>
    /// Gets a field configuration by identifier (case-insensitive).
    /// </summary>
    /// <param name="fieldName">Field identifier.</param>
    /// <returns>The matching <see cref="FieldConfig"/>.</returns>
    public FieldConfig GetFieldFromForm(string fieldName)
    {
        var listOfFields = GetFieldsForForm();
        return listOfFields.First(f => f.Id.Equals(fieldName, System.StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// Returns the list of fields for the form, optionally filtered by table name.
    /// </summary>
    /// <param name="tableName">Optional table filter; empty string returns fields for all pages/tables.</param>
    public List<FieldConfig> GetFieldsForForm(string tableName  = "")
    {
        var listOfFields = new List<FieldConfig>();
        if (PagesConfig == null)
            return listOfFields;

        var saveTableName = SaveEventConfig?.TableName;

        foreach (var pageConfig in PagesConfig)
        {
            if (pageConfig == null)
                continue;

            if (string.IsNullOrEmpty(pageConfig.TableName) || string.IsNullOrEmpty(tableName) || pageConfig.TableName.Equals(tableName, System.StringComparison.CurrentCultureIgnoreCase))
            {
                listOfFields.AddRange(pageConfig.GetFieldList(tableNameInForm: saveTableName));
            }
        }
        return listOfFields;
    }

    /// <summary>
    /// Returns field ids in the same order they appear in the form layout (pages, columns, and form groups),
    /// matching the order defined by page and form configuration in the UI. Use this to align stored test
    /// result JSON and list callout field order with the data entry form. A single id is returned for each
    /// field grid (sub-columns are not expanded in this list).
    /// </summary>
    /// <remarks>
    /// <c>PagesConfig</c> must be populated with resolved page definitions. Deserializing stored form JSON alone
    /// usually only sets the <c>Pages</c> name list, not <c>PagesConfig</c>, so this method would return no ids.
    /// </remarks>
    /// <returns>Ordered field ids; an empty list when the form has no fields or pages.</returns>
    public IReadOnlyList<string> GetOrderedFieldIdsForResults()
    {
        return GetFieldsForForm()
            .Where(f => f != null && !string.IsNullOrEmpty(f.Id))
            .Select(f => f.Id)
            .ToList();
    }

    /// <summary>
    /// Returns all grid subfield configurations across the form, optionally filtered by grid field name.
    /// </summary>
    /// <param name="fieldName">Optional grid field identifier filter.</param>
    public List<FieldGridConfig> GetGridFieldList(string fieldName="")
    {
        var returnList = new List<FieldGridConfig>();
        foreach (var page in PagesConfig)
        {
            returnList.AddRange(page.GetGridFieldList(fieldName));
        };
        return returnList;
    }
}
