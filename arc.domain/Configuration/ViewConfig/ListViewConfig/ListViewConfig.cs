using System.Collections.Generic;
using System.Linq;
using arc.common.ExtensionMethods;
using arc.common.Models.User;
using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.Common;

namespace arc.domain.Configuration.ViewConfig.ListViewConfig
{
    /// <summary>
    /// Represents the configuration for a list view, including various settings and options.
    /// </summary>
    public class ListViewConfig : BaseViewConfig
    {
        /// <summary>
        /// Gets or sets the unique identifier for the list view configuration.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the list view.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the query associated with the list view.
        /// </summary>
        public string QueryName { get; set; }

        /// <summary>
        /// Gets or sets the single query used in the list view.
        /// </summary>
        public string SingleQuery { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier for hierarchical data structures.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the field that the data in the list view should be grouped by.
        /// </summary>
        public string GroupBy { get; set; }

        /// <summary>
        /// Gets or sets the text to be displayed in the group header.
        /// </summary>
        public string GroupText { get; set; }

        /// <summary>
        /// Gets or sets the title of the list view.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the header text for the list view.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the item type displayed in the list view.
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter search is enabled.
        /// </summary>
        public bool FilterSearch { get; set; }

        /// <summary>
        /// Gets or sets the date search configuration.
        /// </summary>
        public string DateSearch { get; set; }

        /// <summary>
        /// Gets or sets the label for the date search filter (e.g. a language tag like @PatDat@ for Date of Birth).
        /// When not set, the default date label is used.
        /// </summary>
        public string DateSearchLabel { get; set; }

        /// <summary>
        /// Gets or sets the display summary configuration.
        /// </summary>
        public string DisplaySummary { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the add button.
        /// </summary>
        public string AddButton { get; set; }

        /// <summary>
        /// Language tag for the add (+) column-header icon tooltip. When omitted, defaults to @ConAddJ@.
        /// </summary>
        public string AddButtonTooltip { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the edit button.
        /// </summary>
        public string EditButton { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the delete button.
        /// </summary>
        public string DeleteButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list view should be displayed if it is empty.
        /// </summary>
        public bool DisplayIfEmpty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multi-select is enabled.
        /// </summary>
        public bool MultiSelect { get; set; }

        /// <summary>
        /// Gets or sets the list of tests associated with the list view.
        /// </summary>
        public List<string> Tests { get; set; }

        /// <summary>
        /// Gets or sets the list of reports associated with the list view.
        /// </summary>
        public List<string> Reports { get; set; }

        /// <summary>
        /// Gets or sets the report categories available for reports in this view,
        /// with the allowed datasections for each category.
        /// </summary>
        public List<ReportCategoryAvailabilityConfig> ReportCategories { get; set; }

        /// <summary>
        /// Gets or sets the comma-separated list of header names that can be included on reports in this view.
        /// </summary>
        public string AllowedHeaders { get; set; }

        /// <summary>
        /// Gets or sets the comma-separated list of footer names that can be included on reports in this view.
        /// </summary>
        public string AllowedFooters { get; set; }

        /// <summary>
        /// Gets or sets the grid columns configuration for the list view.
        /// </summary>
        public List<GridColumnsConfig> GridColumns { get; set; }

        /// <summary>
        /// Gets or sets the group-level menu actions available for this list view.
        /// Each item represents an action (key, text, icon, uievent) that applies to a group.
        /// </summary>
        public List<ButtonConfig> GroupMenu { get; set; }

        /// <summary>
        /// Gets or sets the filter configurations for the list view.
        /// </summary>
        public List<FilterConfig> Filters { get; set; }

        /// <summary>
        /// Gets or sets the filter presets configurations for the list view.
        /// </summary>
        public List<FilterPresetConfig> FilterPresets { get; set; }

        /// <summary>
        /// Gets or sets the user-specific column layout (visibility, order, widths) for this list view.
        /// Applied from preferences.ColumnLayouts[Name] during config load.
        /// </summary>
        public ColumnLayoutConfig ColumnLayout { get; set; }

        /// <summary>
        /// Gets or sets the search fields for the list view.
        /// </summary>
        public List<string> SearchFields { get; set; }

        /// <summary>
        /// Gets or sets the number ranges configuration for the list view.
        /// </summary>
        public List<NumberRangeConfig> NumberRanges { get; set; }

        /// <summary>
        /// Specifies the record view associated with the list view
        /// </summary>
        /// <returns>The name of the record view associated with the list view</returns>
        public string RecordView { get; set; }

        /// <summary>
        /// Retrieves a list of UI events from the current configuration, including buttons, tests, and add button.
        /// </summary>
        /// <returns>A list of UI event names.</returns>

        public List<string> GetUIEventList()
        {
            var eventList = new List<string>();

            // Iterate through all buttons and add their UI events to the event list
            foreach (var button in Buttons)
            {
                eventList.AddRange(button.GetUIEventList());
            }

            // Add test UI events to the event list
            if (Tests != null)
            {
                eventList.AddRange(Tests);
            }

            // Add the add button UI event to the event list, if it is not null or empty
            if (!string.IsNullOrEmpty(AddButton))
            {
                eventList.Add(AddButton);
            }

            // Add group menu UI events
            if (GroupMenu != null)
            {
                var groupEvents = GroupMenu.Select(g => g.UIEvent).Where(e => !string.IsNullOrEmpty(e)).ToList();
                eventList.AddRange(groupEvents);
            }

            return eventList;
        }


        /// <summary>
        /// Retrieves a list of UI events at the current level, including those within sub-menus.
        /// </summary>
        /// <returns>A list of UI event names at the current level.</returns>
        public List<string> GetThisLevelOnlyUIEventList()
        {
            var eventList = Buttons.Select(b => b.UIEvent).Where(b => b != null).ToList();

            var eventListWithSubMenus = Buttons.Where(b => b.UIEvent == null).ToList();
            foreach (var item in eventListWithSubMenus)
            {
                if (item.Buttons != null)
                {
                    var subEventList = item.Buttons.Select(b => b.UIEvent).Where(b => b != null).ToList();
                    eventList.AddRange(subEventList);
                }
            }

            return eventList;
        }

        /// <summary>
        /// Removes buttons from the configuration that are not in the allowed UI events list.
        /// </summary>
        /// <param name="uiEvents">The list of allowed UI events.</param>
        public void RemoveButtonsNotInUIEventList(List<UIEventConfig> uiEvents)
        {
            Buttons = CreateNewButtonArray(Buttons, uiEvents);

            if (GroupMenu != null)
            {
                GroupMenu = CreateNewButtonArray(GroupMenu, uiEvents);
            }
        }

        /// <summary>
        /// Updates the default cultures for a given specimen type by adding new values.
        /// </summary>
        /// <param name="specimenTypeId">The ID of the specimen type.</param>
        /// <param name="values">A comma-separated string of culture values.</param>
        //public void UpdateDefaultCultures(int specimenTypeId, string values)
        //{
        //    DeleteDefaultCulture(specimenTypeId);
        //    DefaultCultures.Add(new DefaultTestConfig { SpecimenType = specimenTypeId, Values = values.Split(",").ToList() });
        //}

        /// <summary>
        /// Deletes the default culture for a given specimen type.
        /// </summary>
        /// <param name="specimenTypeId">The ID of the specimen type.</param>
        //public void DeleteDefaultCulture(int specimenTypeId)
        //{
        //    var foundList = DefaultCultures.Where(c => c.SpecimenType == specimenTypeId);

        //    if (foundList.Any())
        //    {
        //        DefaultCultures.Remove(foundList.First());
        //    }
        //}

        /// <summary>
        /// Adds a new test to the list of tests if it does not already exist.
        /// </summary>
        /// <param name="newTest">The name of the new test to add.</param>
        public void AddTest(string newTest)
        {
            newTest = newTest.ToLower();
            if (!Tests.Contains(newTest))
            {
                Tests.Add(newTest);
            }
        }


        /// <summary>
        /// Removes a test from the list of tests.
        /// </summary>
        /// <param name="testToRemove">The name of the test to remove.</param>
        public void RemoveTest(string testToRemove)
        {
            Tests.Remove(testToRemove);
        }

        /// <summary>
        /// Adds a new report to the list of reports.
        /// </summary>
        /// <param name="newReport">The name of the new report to add.</param>
        public void AddReport(string newReport)
        {
            newReport = newReport.ToLower();
            if (!Reports.Contains(newReport))
            {
                Reports.Add(newReport);
            }
        }

        /// <summary>
        /// Removes a report from the list of reports.
        /// </summary>
        /// <param name="reportToRemove">The name of the report to remove.</param>
        public void RemoveReport(string reportToRemove)
        {
            reportToRemove = reportToRemove.ToLower();
            Reports.Remove(reportToRemove);
        }

        /// <summary>
        /// Stable report category names used on list views that expose report designer data section whitelists.
        /// </summary>
        public static class ReportCategoryNames
        {
            /// <summary>Single-instance category for direct-test report sections.</summary>
            public const string Main = "Main";

            /// <summary>Repeatable category for culture/isolate-test report sections.</summary>
            public const string Organism = "Organism";
        }

        /// <summary>
        /// Registers a data section on the named report category for this view.
        /// </summary>
        /// <param name="categoryName">The report category name (for example Main or Organism).</param>
        /// <param name="dataSectionName">The stable data section config name to whitelist.</param>
        /// <returns>True when the whitelist was updated; false when the category was missing or the name was already present.</returns>
        public bool AddDataSectionToReportCategory(string categoryName, string dataSectionName)
        {
            if (ReportCategories == null || string.IsNullOrWhiteSpace(categoryName) || string.IsNullOrWhiteSpace(dataSectionName))
            {
                return false;
            }

            var category = ReportCategories.FirstOrDefault(c => c.Name.IsSameConfigName(categoryName));
            if (category == null)
            {
                return false;
            }

            return category.AddDataSection(dataSectionName);
        }

        /// <summary>
        /// Removes a data section from the named report category for this view.
        /// </summary>
        /// <param name="categoryName">The report category name (for example Main or Organism).</param>
        /// <param name="dataSectionName">The stable data section config name to remove.</param>
        /// <returns>True when the whitelist was updated; false when the category or name was not found.</returns>
        public bool RemoveDataSectionFromReportCategory(string categoryName, string dataSectionName)
        {
            if (ReportCategories == null || string.IsNullOrWhiteSpace(categoryName) || string.IsNullOrWhiteSpace(dataSectionName))
            {
                return false;
            }

            var category = ReportCategories.FirstOrDefault(c => c.Name.IsSameConfigName(categoryName));
            if (category == null)
            {
                return false;
            }

            return category.RemoveDataSection(dataSectionName);
        }

        /// <summary>
        /// Updates the default tests for a given specimen type.
        /// </summary>
        /// <param name="specimenTypeId">The ID of the specimen type.</param>
        /// <param name="values">A comma-separated string of test values.</param>
        //public void UpdateDefaultTests(int specimenTypeId, string values)
        //{
        //    DeleteDefaultTest(specimenTypeId);
        //    DefaultTests.Add(new DefaultTestConfig { SpecimenType = specimenTypeId, Values = [.. values.Split(",")] });
        //}

        /// <summary>
        /// Deletes the default test for a given specimen type.
        /// </summary>
        /// <param name="specimenTypeId">The ID of the specimen type.</param>
        //public void DeleteDefaultTest(int specimenTypeId)
        //{
        //    var foundList = DefaultTests.Where(c => c.SpecimenType == specimenTypeId);

        //    if (foundList.Any())
        //    {
        //        DefaultTests.Remove(foundList.First());
        //    }
        //}

        /// <summary>
        /// Updates the default culture tests for a given culture type.
        /// </summary>
        /// <param name="cultureTypeId">The ID of the culture type.</param>
        /// <param name="values">A comma-separated string of test values.</param>
        //public void UpdateDefaultCultureTests(int cultureTypeId, string values)
        //{
        //    DeleteDefaultCultureTest(cultureTypeId);
        //    DefaultCultureTests.Add(new DefaultCultureTestConfig { CultureType = cultureTypeId, Values = values.Split(",").ToList() });
        //}

        /// <summary>
        /// Deletes the default culture test for a given culture type.
        /// </summary>
        /// <param name="cultureTypeId">The ID of the culture type.</param>
        //public void DeleteDefaultCultureTest(int cultureTypeId)
        //{
        //    var foundList = DefaultCultureTests.Where(c => c.CultureType == cultureTypeId);

        //    if (foundList.Any())
        //    {
        //        DefaultCultureTests.Remove(foundList.First());
        //    }
        //}

        /// <summary>
        /// Creates a new array of button configurations that are allowed based on the provided UI events.
        /// </summary>
        /// <param name="buttons">The list of original button configurations to filter.</param>
        /// <param name="uiEvents">The list of allowed UI events.</param>
        /// <returns>A new list of button configurations that are allowed.</returns>
        private List<ButtonConfig> CreateNewButtonArray(List<ButtonConfig> buttons, List<UIEventConfig> uiEvents)
        {
            var newButtonList = new List<ButtonConfig>();

            foreach (var button in buttons)
            {
                if (button.UIEvent != null)
                {
                    bool allowed = uiEvents.Any(s => s.Name.ToLower() == button.UIEvent.ToLower());
                    if (allowed)
                    {
                        newButtonList.Add(button);
                    }
                }
                else if (button.Buttons != null)
                {
                    button.Buttons = CreateNewButtonArray(button.Buttons.ToList(), uiEvents);
                    newButtonList.Add(button);
                }
            }

            return newButtonList;
        }
    }

    /// <summary>
    /// Defines a report category and the datasections that are available within that category for a view.
    /// </summary>
    public class ReportCategoryAvailabilityConfig
    {
        /// <summary>
        /// Gets or sets the category name (e.g., Main, Organism, Final).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the comma-separated list of datasection names available for this category.
        /// </summary>
        public string DataSections { get; set; }

        /// <summary>
        /// Gets or sets the category type. Use 'Main' for single-instance categories
        /// and 'Many' for repeatable categories (e.g., Organism).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Appends a data section name to the comma-separated whitelist if not already present.
        /// Matching uses normalised config names (trim + lower case).
        /// </summary>
        /// <param name="dataSectionName">The stable data section config name to add.</param>
        /// <returns>True when the whitelist was updated; false when the name was already present or invalid.</returns>
        public bool AddDataSection(string dataSectionName)
        {
            var normalisedName = dataSectionName.NormalisedConfigName();
            if (normalisedName.Length == 0)
            {
                return false;
            }

            var names = ParseDataSectionNames(DataSections).ToList();
            if (names.Any(existing => existing.IsSameConfigName(normalisedName)))
            {
                return false;
            }

            names.Add(normalisedName);
            DataSections = JoinDataSectionNames(names);
            return true;
        }

        /// <summary>
        /// Removes a data section name from the comma-separated whitelist if present.
        /// </summary>
        /// <param name="dataSectionName">The stable data section config name to remove.</param>
        /// <returns>True when the whitelist was updated; false when the name was not found or invalid.</returns>
        public bool RemoveDataSection(string dataSectionName)
        {
            var normalisedName = dataSectionName.NormalisedConfigName();
            if (normalisedName.Length == 0)
            {
                return false;
            }

            var names = ParseDataSectionNames(DataSections).ToList();
            var removed = names.RemoveAll(existing => existing.IsSameConfigName(normalisedName));
            if (removed == 0)
            {
                return false;
            }

            DataSections = JoinDataSectionNames(names);
            return true;
        }

        private static IEnumerable<string> ParseDataSectionNames(string dataSections)
        {
            if (string.IsNullOrWhiteSpace(dataSections))
            {
                return Enumerable.Empty<string>();
            }

            return dataSections
                .Split(',')
                .Select(name => name.Trim())
                .Where(name => name.Length > 0);
        }

        private static string JoinDataSectionNames(IEnumerable<string> names)
        {
            return string.Join(",", names.Where(name => !string.IsNullOrWhiteSpace(name)).Select(name => name.Trim()));
        }
    }
}
