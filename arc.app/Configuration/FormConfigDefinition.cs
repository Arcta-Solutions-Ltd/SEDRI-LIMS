using arc.app.Common;
using arc.app.Config.Events;
using arc.app.Config.Forms;
using arc.app.Config.Mapper;
using arc.app.Config.Pages;
using arc.app.Config.Queries;
using arc.app.Config.Reports;
using arc.app.Config.Reports.DataSection;
using arc.app.Config.UIEvents;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.common.Utils;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public class FormConfigDefinition : IFormConfigDefinition
    {
        private readonly IFormConfigAdapter _formAdapter;
        private readonly IUIEventConfigAdapter _uieventAdapter;
        private readonly IEventAdapter _eventAdapter;
        private readonly IMapperAdapter _mapperAdapter;
        private readonly IQueryAdapter _queryAdapter;
        private readonly IPageConfigAdapter _pageAdapter;
        private readonly IFormConfigRepository _formConfigRepository;
        private readonly ICopyProperties _copyProperties;
        private readonly IDataSectionAdapter _dataSectionAdapter;
        private readonly ISectionAdapter _sectionAdapter;
        private readonly IReportAdapter _reportAdapter;
        private readonly ILogWriter _logWriter;
        private readonly IResetNamesInForm _resetNamesInForm;

        public FormConfigDefinition(IFormConfigAdapter formAdapter, IUIEventConfigAdapter uieventAdapter, IEventAdapter eventAdapter, IMapperAdapter mapperAdapter,
            IQueryAdapter queryAdapter, IPageConfigAdapter pageAdapter, IFormConfigRepository formConfigRepository, ICopyProperties copyProperties, IDataSectionAdapter dataSectionAdapter,
            ISectionAdapter sectionAdapter, IReportAdapter reportAdapter, ILogWriter logWriter, IResetNamesInForm resetNamesInForm)
        {
            _formAdapter = formAdapter;
            _uieventAdapter = uieventAdapter;
            _eventAdapter = eventAdapter;
            _mapperAdapter = mapperAdapter;
            _queryAdapter = queryAdapter;
            _pageAdapter = pageAdapter;
            _formConfigRepository = formConfigRepository;
            _copyProperties = copyProperties;
            _dataSectionAdapter = dataSectionAdapter;
            _sectionAdapter = sectionAdapter;
            _reportAdapter = reportAdapter;
            _logWriter = logWriter;
            _resetNamesInForm = resetNamesInForm;
        }

        public async Task<FullFormConfig> LoadFormAsync(string formName)
        {
            // Get Form
            var formDef = await _formAdapter.GetFormAsync(formName);
            var form = new FullFormConfig();
            _logWriter.LogInfo($"Copy all properties for form {formName}", "FormConfigDefinition", "LoadForm");
            _copyProperties.CopyAll(formDef, form);

            // Get UI Event from form
            if (!string.IsNullOrWhiteSpace(form.UIEvent))
            {
                _logWriter.LogInfo($"Copy the ui event definition {form.UIEvent}", "FormConfigDefinition", "LoadForm");
                form.UIEventConfig = await _uieventAdapter.GetEventAsync(form.UIEvent);
            }

            // Get Event from form
            _logWriter.LogInfo($"Copy the event definition {form.SaveEvent}", "FormConfigDefinition", "LoadForm");
            var eventDef = await _eventAdapter.GetEventAsync(form.SaveEvent);
            form.SaveEventConfig = new FullEventConfig();
            _logWriter.LogInfo($"Copy all properties for the event {form.SaveEvent}", "FormConfigDefinition", "LoadForm");
            _copyProperties.CopyAll(eventDef, form.SaveEventConfig);

            // Get Mapping from event
            if (!string.IsNullOrWhiteSpace(form.SaveEventConfig.Mapping))
            {
                _logWriter.LogInfo($"Copy the mapping definition {form.SaveEventConfig.Mapping} for event {form.SaveEvent}", "FormConfigDefinition", "LoadForm");
                form.SaveEventConfig.MappingConfig = await _mapperAdapter.GetMapperAsync(form.SaveEventConfig.Mapping);
            }

            // Get Initial query from form
            if (!string.IsNullOrWhiteSpace(form.InitialQuery))
            {
                _logWriter.LogInfo($"Get the query definition {form.InitialQuery}", "FormConfigDefinition", "LoadForm");
                var queryDef = await _queryAdapter.GetQueryAsync(form.InitialQuery);
                form.InitialQueryConfig = new FullQueryConfig();
                if (queryDef != null)
                {
                    _logWriter.LogInfo($"Copy all properties for the query {form.InitialQuery}", "FormConfigDefinition", "LoadForm");
                    _copyProperties.CopyAll(queryDef, form.InitialQueryConfig);
                }
            }

            // Get initial query result mapper from initial query
            if (!string.IsNullOrWhiteSpace(form.InitialQuery) && !string.IsNullOrWhiteSpace(form.InitialQueryConfig.ResultMapping))
            {
                _logWriter.LogInfo($"Get the result mapping definition {form.InitialQueryConfig.ResultMapping} for initial query {form.InitialQuery}", "FormConfigDefinition", "LoadForm");
                form.InitialQueryConfig.ResultMapperConfig = await _mapperAdapter.GetMapperAsync(form.InitialQueryConfig.ResultMapping);
            }

            // Get initial query parameter mapper from initial query
            if (!string.IsNullOrWhiteSpace(form.InitialQuery) && !string.IsNullOrWhiteSpace(form.InitialQueryConfig.ParameterMapping))
            {
                _logWriter.LogInfo($"Get the parameter mapping definition {form.InitialQueryConfig.ParameterMapping} for initial query {form.InitialQuery}", "FormConfigDefinition", "LoadForm");
                form.InitialQueryConfig.ParameterMapperConfig = await _mapperAdapter.GetMapperAsync(form.InitialQueryConfig.ParameterMapping);
            }

            // Get datasection for the form
            if (!string.IsNullOrWhiteSpace(form.DataSection))
            {
                _logWriter.LogInfo($"Get the data section {form.DataSection}", "FormConfigDefinition", "LoadForm");
                form.DataSectionConfig = await _dataSectionAdapter.GetSectionAsync(form.DataSection);

                //Get the report sections which use the datasection
                _logWriter.LogInfo("Get all sections", "FormConfigDefinition", "LoadForm");
                var sectionList = await _sectionAdapter.GetAllSectionsAsync();
                foreach (var section in sectionList)
                {
                    if (section.DataSection.IsSameConfigName(form.DataSection))
                    {
                        _logWriter.LogInfo($"Add report section {section.Name}", "FormConfigDefinition", "LoadForm");
                        form.ReportSectionConfigList.Add(section);
                    }
                }

                if (form.ReportSectionConfigList.Count == 0
                    && form.NewSectionConfig != null
                    && form.NewSectionConfig.DataSection.IsSameConfigName(form.DataSection))
                {
                    form.ReportSectionConfigList.Add(form.NewSectionConfig);
                    _logWriter.LogWarning(
                        $"No report section matched dataSection={form.DataSection} for form={formName}; using NewSectionConfig {form.NewSectionConfig.Name}",
                        "FormConfigDefinition",
                        "LoadForm");
                }

                var matchedSectionNames = form.ReportSectionConfigList.Count > 0
                    ? string.Join(",", form.ReportSectionConfigList.Select(s => s.Name))
                    : "(none)";
                _logWriter.LogInfo(
                    $"Form {formName} dataSection={form.DataSection} matched {form.ReportSectionConfigList.Count} report section(s): {matchedSectionNames}",
                    "FormConfigDefinition",
                    "LoadForm");

                //Get the reports which use the section

                if (form.ReportSectionConfigList.Count > 0)
                {
                    _logWriter.LogInfo("Get all reports", "FormConfigDefinition", "LoadForm");
                    var reportList = await _reportAdapter.GetAllReportsAsync();
                    foreach (var report in reportList)
                    {
                        var found = false;
                        foreach (var section in form.ReportSectionConfigList)
                        {
                            found = found || report.ContainsSection(section.Name);
                        }
                        if (found)
                        {
                            _logWriter.LogInfo($"Add report {report.Name}", "FormConfigDefinition", "LoadForm");
                            form.ReportConfigList.Add(report);
                        }
                    }
                }
            }

            // Get Pages from form
            var pages = new List<PageConfig>();
            foreach (var page in form.Pages)
            {
                _logWriter.LogInfo($"Get the page {page}", "FormConfigDefinition", "LoadForm");
                var nextPage = await _pageAdapter.GetPageAsync(page);
                pages.Add(nextPage);
            }

            form.PagesConfig = pages;

            //Get the record view query related to the form

            if (form.SaveEventConfig.TableName.ToLower() == "specimen")
            {
                form.RecordViewQueryConfig = new FullQueryConfig();
                var queryDef = await _queryAdapter.GetQueryAsync("specimenforspecimenview");
                _copyProperties.CopyAll(queryDef, form.RecordViewQueryConfig);

                // Get the result mapper for the view
                _logWriter.LogInfo($"Get the record view result mapping definition for specimens", "FormConfigDefinition", "LoadForm");
                form.RecordViewQueryConfig.ResultMapperConfig = await _mapperAdapter.GetMapperAsync(form.RecordViewQueryConfig.ResultMapping);
            }

            return form;
        }

        /// <summary>
        /// Renames all configs on a cloned form using canonical names from <paramref name="names"/>.
        /// </summary>
        /// <param name="formToCopy">The source form configuration being cloned.</param>
        /// <param name="names">Canonical names derived from the reserved form config name.</param>
        /// <param name="formType">Form type string (for example <c>directtest</c> or <c>culturetest</c>).</param>
        /// <param name="title">Display title for the cloned test.</param>
        /// <param name="description">Description for the cloned test.</param>
        /// <returns>The renamed form configuration ready for persistence.</returns>
        public async Task<FullFormConfig> ResetNamesToNewFormAsync(FullFormConfig formToCopy, CloneTestNames names, string formType, string title, string description)
        {
            var eventBase = names.SaveEventName;

            formToCopy.UIEvent = names.UIEventName;
            formToCopy.UIEventConfig.Name = names.UIEventName;
            formToCopy.UIEventConfig.Action = names.FormConfigName;
            formToCopy.Name = names.FormConfigName;
            formToCopy.SaveEvent = names.SaveEventName;
            formToCopy.SaveEventConfig.EventName = names.SaveEventName;
            formToCopy.SaveEventConfig.Mapping = eventBase + "mapping";
            formToCopy.SaveEventConfig.MappingConfig.Name = eventBase + "mapping";
            formToCopy.SaveEventConfig.Description = title;
            formToCopy.InitialQuery = eventBase + "byidquery";
            formToCopy.InitialQueryConfig.Query = eventBase + "byidquery";
            formToCopy.NewSectionConfig.DataSection = names.DataSectionName;
            formToCopy.NewSectionConfig.Description = eventBase;
            formToCopy.NewSectionConfig.HeadingText = title;
            formToCopy.DataSection = names.DataSectionName;
            formToCopy.DataSectionConfig.Name = names.DataSectionName;

            if (!string.IsNullOrWhiteSpace(formToCopy.InitialQueryConfig.ResultMapping))
            {
                _logWriter.LogInfo("Rename initial query result mapping", "FormConfigDefinition", "ResetNamesToNewForm");
                formToCopy.InitialQueryConfig.ResultMapping = eventBase + "resultmapping";
                formToCopy.InitialQueryConfig.ResultMapperConfig.Name = eventBase + "resultmapping";
            }
            if (!string.IsNullOrWhiteSpace(formToCopy.InitialQueryConfig.ParameterMapping))
            {
                _logWriter.LogInfo("Rename initial query parameter mapping", "FormConfigDefinition", "ResetNamesToNewForm");
                formToCopy.InitialQueryConfig.ParameterMapping = eventBase + "parametermapping";
                formToCopy.InitialQueryConfig.ParameterMapperConfig.Name = eventBase + "parametermapping";
            }
            if (formToCopy.NewSectionConfig != null)
            {
                _logWriter.LogInfo("Rename new section", "FormConfigDefinition", "ResetNamesToNewForm");
                formToCopy.NewSectionConfig.Id = names.ReportSectionName;
                formToCopy.NewSectionConfig.Name = names.ReportSectionName;
            }

            _logWriter.LogInfo(
                $"Clone naming: form={names.FormConfigName} saveEvent={names.SaveEventName} dataSection={names.DataSectionName} section={names.ReportSectionName} uievent={names.UIEventName}",
                "FormConfigDefinition",
                "ResetNamesToNewForm");

            //Reset field names

            _logWriter.LogInfo("Rename field names", "FormConfigDefinition", "ResetNamesToNewForm");
            var pageArray = new List<string>();
            formToCopy.PagesConfig[0].PageTitle = title;
            formToCopy.PagesConfig[0].Text = description;
            for (int pageNum = 0; pageNum < formToCopy.PagesConfig.Count; pageNum++)
            {
                formToCopy.PagesConfig[pageNum].Name = eventBase + "page" + pageNum;
                pageArray.Add(eventBase + "page" + pageNum);

                for (int colNum = 0; colNum < formToCopy.PagesConfig[pageNum].Columns.Count; colNum++)
                {
                    for (int groupNum = 0; groupNum < formToCopy.PagesConfig[pageNum].Columns[colNum].FormGroups.Count; groupNum++)
                    {
                        for (int fieldNum = 0; fieldNum < formToCopy.PagesConfig[pageNum].Columns[colNum].FormGroups[groupNum].Fields.Count; fieldNum++)
                        {
                            var queryFilter = new QueryFilterConfig();
                            var fieldName = formToCopy.PagesConfig[pageNum].Columns[colNum].FormGroups[groupNum].Fields[fieldNum].Id;

                            if (fieldName.ToLower() != "printonreport")
                            {
                                queryFilter.AddString("Name", fieldName);
                                var newFieldName = await _formConfigRepository.GetNextAvailableFieldNameAsync(queryFilter);
                                formToCopy = _resetNamesInForm.Reset(formToCopy, formToCopy.PagesConfig[pageNum].Columns[colNum].FormGroups[groupNum].Fields[fieldNum].Id, newFieldName, formToCopy.PagesConfig[pageNum].Columns[colNum].FormGroups[groupNum].Fields[fieldNum].Type);
                                formToCopy.DataSectionConfig.RenameGrid(fieldName, newFieldName);
                                formToCopy.NewSectionConfig.RenameGrid(fieldName, newFieldName);
                            }
                        }
                    }
                }
            }

            formToCopy.Pages = pageArray;
            formToCopy.Formtype = formType;
            formToCopy.Title = title;

            return formToCopy;
        }

    }
}
