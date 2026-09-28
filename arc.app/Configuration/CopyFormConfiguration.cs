using arc.app.Common;
using arc.app.Config.Reports;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public class CopyFormConfiguration : ICopyFormConfiguration
    {
        private readonly IConfigRepository _configRepository;
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IConfigExtractionUtils _configExtractionUtils;
        private readonly IFormConfigRepository _formConfigRepository;
        private readonly IReportAdapter _reportAdapter;
        private readonly ILogWriter _logWriter;

        public CopyFormConfiguration(IConfigRepository configRepository, IFormConfigDefinition formConfigDefinition, IConfigExtractionUtils configExtractionUtils, IFormConfigRepository formConfigRepository, IReportAdapter reportAdapter, ILogWriter logWriter)
        {
            _configExtractionUtils = configExtractionUtils;
            _configRepository = configRepository;
            _formConfigDefinition = formConfigDefinition;
            _formConfigRepository = formConfigRepository;
            _reportAdapter = reportAdapter;
            _logWriter = logWriter;
        }

        public async Task Copy(string dataToSave, string formType, int formTypeKey)
        {
            try
            {
                var newTest = JsonConvert.DeserializeObject<UpdateTestConfigModel>(dataToSave);

                var configName = newTest.Title.Replace(" ", "").RemoveSpecialCharacters().Trim();
                if (configName.Length > 50)
                {
                    configName = configName[..50];
                }

                //Get next available name
                var queryFilter = new QueryFilterConfig();
                queryFilter.AddString("Name", configName);
                queryFilter.AddString("Suffix", "form");
                configName = await _configRepository.GetNextAvailableNameAsync(queryFilter);
                configName = configName.Trim();
                var cloneNames = CloneTestNamingExtensions.DeriveFromReservedFormName(configName);

                //Get the form clone
                var formToCopy = await _formConfigDefinition.LoadFormAsync(newTest.TestToCloneId);
                formToCopy = SetSpecialValuesBasedOnType(formToCopy, formType);

                //Create new report section

                if (formType == "directtest" || formType == "culturetest")
                {
                    formToCopy.NewSectionConfig = new ReportSectionConfig();
                    if (formToCopy.ReportSectionConfigList.Count > 0)
                    {
                        formToCopy.NewSectionConfig = formToCopy.ReportSectionConfigList.First();
                    }
                    formToCopy.NewSectionConfig.Id = cloneNames.ReportSectionName;
                    formToCopy.NewSectionConfig.Name = cloneNames.ReportSectionName;
                    formToCopy.NewSectionConfig.DataSection = cloneNames.DataSectionName;
                    formToCopy.NewSectionConfig.Description = cloneNames.SaveEventName;
                }

                //Add report to report section

                if (formType == "directtest" || formType == "culturetest")
                {
                    await EnsureDefaultSpecimenReportInListAsync(formToCopy);
                }

                for (var x = 0; x < formToCopy.ReportConfigList.Count; x++)
                {
                    if (formToCopy.ReportConfigList[x].Name.ToLower() == "defaultspecimenreport")
                    {
                        if (formType == "directtest")
                        {
                            formToCopy.ReportConfigList[x].AddMainSection(cloneNames.ReportSectionName);
                        }

                        if (formType == "culturetest")
                        {
                            formToCopy.ReportConfigList[x].AddOrganismSection(cloneNames.ReportSectionName);
                        }
                    }
                }

                //Update save event Topic to match test type
                if (formType == "directtest")
                {
                    formToCopy.SaveEventConfig.Topic = "Tests";
                }
                else if (formType == "culturetest")
                {
                    formToCopy.SaveEventConfig.Topic = "CultureTests";
                }

                //Rename form element so that it is a new form
                formToCopy = await _formConfigDefinition.ResetNamesToNewFormAsync(formToCopy, cloneNames, formType, newTest.Title, newTest.Description);

                //Create add new form command and link into the config repository and then call it
                formToCopy.Type = formTypeKey;
                await _formConfigRepository.AddNewFormAsync(formToCopy);

                var dataSectionName = formToCopy.DataSection;
                var categoryName = formType == "directtest"
                    ? ListViewConfig.ReportCategoryNames.Main
                    : ListViewConfig.ReportCategoryNames.Organism;

                var view = await _configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(newTest.Id));
                view.AddTest(cloneNames.UIEventName);
                var whitelistUpdated = view.AddDataSectionToReportCategory(categoryName, dataSectionName);
                await _configExtractionUtils.SaveViewAsync(view);

                _logWriter.LogInfo(
                    $"Clone {formType}: view={view.Name} category={categoryName} form={cloneNames.FormConfigName} saveEvent={cloneNames.SaveEventName} dataSection={dataSectionName} section={cloneNames.ReportSectionName} uievent={cloneNames.UIEventName} whitelistUpdated={whitelistUpdated}",
                    "CopyFormConfiguration",
                    "Copy");
            } catch (Exception e)
            {
                _logWriter.LogError(e.Message, "CopyFormConfiguration", "Copy");
            }
            
        }

        private FullFormConfig SetSpecialValuesBasedOnType(FullFormConfig formToChange, string type)
        {
            if (type == "culturetest") {
                formToChange.SaveEventConfig.TableName = "CultureTests";
                formToChange.InitialQueryConfig.TableName = "CultureTests";
            }

            if (type == "directtest") {
                formToChange.SaveEventConfig.TableName = "Tests";
                formToChange.InitialQueryConfig.TableName = "Tests";
            }

            return formToChange;
        }

        /// <summary>
        /// Ensures the default specimen report is available for clone updates when the source test's
        /// report section is no longer referenced on the persisted report definition.
        /// </summary>
        /// <param name="formToCopy">The form configuration being cloned.</param>
        private async Task EnsureDefaultSpecimenReportInListAsync(FullFormConfig formToCopy)
        {
            if (formToCopy.ReportConfigList.Any(report => report.Name.IsSameConfigName("defaultspecimenreport")))
            {
                return;
            }

            var defaultReport = await _reportAdapter.GetReportAsync("defaultspecimenreport");
            formToCopy.ReportConfigList.Add(defaultReport);

            _logWriter.LogInfo(
                "Loaded defaultspecimenreport for clone because no linked report was found on the source form",
                "CopyFormConfiguration",
                "EnsureDefaultSpecimenReportInListAsync");
        }
    }
}
