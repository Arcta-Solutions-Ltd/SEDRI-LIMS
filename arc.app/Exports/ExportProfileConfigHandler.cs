using arc.app.Common;
using arc.app.Config;
using arc.app.Config.UIEvents;
using arc.app.Configuration;
using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.ListsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public class ExportProfileConfigHandler : IExportProfileConfigHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILanguageHandler _languageHandler;

        public ExportProfileConfigHandler(IServiceProvider serviceProvider, ILanguageHandler languageHandler)
        {
            _serviceProvider = serviceProvider;
            _languageHandler = languageHandler;
        }

        public async Task<List<OptionsConfig>> GetFieldsAsync(TokenInfoModel token)
        {
            var listViewFactory = _serviceProvider.GetService<IListViewConfigFactory>();
            var uiEventConfigAdapter = _serviceProvider.GetService<IUIEventConfigAdapter>();
            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var fieldsToRemove = new List<string> { "LocationSearch", "PatientRefSearch", "SurnameSearch", "Values", "Decision", "DisplayInReport", "printonreport", "Action", "BenchReadDay0Action", "BenchReadDay1Action", "CannedComment", "Comment", "CommentType", "PrintOnReport" };

            var view = await listViewFactory.GetViewAsync("specimens");
            var uiEventList = view.GetUIEventList();

            var optionList = new List<OptionsConfig>();
            optionList.AddRange(await AddCustomFieldTypesAsync(token.LanguageId));

            foreach (var uievent in uiEventList)
            {
                var uiEventConfig = await uiEventConfigAdapter.GetEventAsync(uievent);

                if (uiEventConfig.Type.Is("form"))
                {
                    var formConfig = await formConfigDefinition.LoadFormAsync(uiEventConfig.Action);
                    var description = await _languageHandler.TranslateAsync(formConfig.SaveEventConfig.Description, token.LanguageId);
                    var fieldList = formConfig.GetFieldsForForm();

                    var cultureField = fieldList.FirstOrDefault(f => f.Id.ToLower() == "culturetype");
                    if (cultureField != null)
                    {
                        cultureField.Id = "typeid";
                    }

                    var newOptionList = fieldList.FindAll(a => !string.IsNullOrWhiteSpace(a.Label))
                       .Select(async f => new OptionsConfig
                       {
                           Key = $"{f.Id}|{formConfig.Name}|{f.TableName}|{formConfig.SaveEventConfig.Description}",
                           Text = await _languageHandler.TranslateAsync(f.Label, token.LanguageId) + " (" + (f.TableName.Is("Tests") || f.TableName.Is("CultureTests") ? description : await TableTranslatorAsync(f.TableName, token.LanguageId)) + ")",
                           ParentKey = f.Id
                       }); ;
                    foreach (var item in newOptionList)
                    {
                        optionList.Add(await item);

                    }
                }
            }
            var filteredOptions = optionList.DistinctBy(f => f.ParentKey)
                //.DistinctBy(f => f.Text)
                .FilterOutWeeds(w => w.ParentKey, fieldsToRemove)
                .Select(f => new OptionsConfig { Key = f.Key, Text = f.Text })
                .OrderBy(o => o.Text)
                .ToList();
            return filteredOptions;
        }

        private async Task<List<OptionsConfig>> AddCustomFieldTypesAsync(string languageId)
        {
            return new List<OptionsConfig>
            {
                new OptionsConfig
                {
                    Key = "LocationHierarchy|Custom|Specimen|Multicolumn",
                    Text = await _languageHandler.TranslateAsync("@ExpLoc@",languageId),
                    ParentKey = "Custom1"
                },
                new OptionsConfig
                {
                    Key = "WhonetAntibiotic|Custom|Culture|Multicolumn",
                    Text = await _languageHandler.TranslateAsync("@ExpWhoA@",languageId),
                    ParentKey = "Custom2"
                },
                new OptionsConfig
                {
                    Key = "SpecimenOrganism|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@GenOrgA@",languageId),
                    ParentKey = "Custom3"
                },
                new OptionsConfig
                {
                    Key = "OrganisationHierarchy|Custom|Specimen|Multicolumn",
                    Text = await _languageHandler.TranslateAsync("@ExpOrg@",languageId),
                    ParentKey = "Custom4"
                },
                new OptionsConfig
                {
                    Key = "CultureNumber|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpCulNum@",languageId),
                    ParentKey = "Custom6"
                },
                new OptionsConfig
                {
                    Key = "OrganismNameOrGrowth|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpGroOrg@", languageId),
                    ParentKey = "Custom7"
                },
                new OptionsConfig
                {
                    Key = "OrganismPreferredName|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@OrgPreA@", languageId),
                    ParentKey = "Custom8"
                },
                new OptionsConfig
                {
                    Key = "ApprovalDate|Custom|Specimen|Custom",
                    Text = await _languageHandler.TranslateAsync("@SpeAppD@", languageId),
                    ParentKey = "Custom9"
                },
                new OptionsConfig
                {
                    Key = "SpecimenComments|Custom|Specimen|Mulitcolumn",
                    Text = await _languageHandler.TranslateAsync("@GenComJ@", languageId),
                    ParentKey = "Custom10"
                },
                new OptionsConfig
                {
                    Key = "CultureComments|Custom|Culture|Mulitcolumn",
                    Text = await _languageHandler.TranslateAsync("@GenComK@", languageId),
                    ParentKey = "Custom11"
                },
                new OptionsConfig
                {
                    Key = "SpecimenState|Custom|Specimen|Custom",
                    Text = await _languageHandler.TranslateAsync("@SpeSpeQ@", languageId),
                    ParentKey = "Custom12"
                },
                new OptionsConfig
                {
                    Key = "ReportDate|Custom|Specimen|Custom",
                    Text = await _languageHandler.TranslateAsync("@RepDat@", languageId),
                    ParentKey = "Custom13"
                },
                new OptionsConfig
                {
                    Key = "QualitativeAntibioticSusceptibility|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpFldAnti@", languageId),
                    ParentKey = "Custom14"
                },
                new OptionsConfig
                {
                    Key = "BlankColumn|Custom|Custom|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpProFielBlank@", languageId),
                    ParentKey = "Custom15"
                },
                new OptionsConfig
                {
                    Key = "LocationCodeHierarchy|Custom|Patient|Multicolumn",
                    Text = await _languageHandler.TranslateAsync("@ExpLocCode@",languageId),
                    ParentKey = "Custom16"
                },
                new OptionsConfig
                {
                    Key = "OrganisationCodeHierarchy|Custom|Specimen|Multicolumn",
                    Text = await _languageHandler.TranslateAsync("@ExpOrgCode@",languageId),
                    ParentKey = "Custom17"
                },
                new OptionsConfig
                {
                    Key = "AntibioticId|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTAnti@",languageId),
                    ParentKey = "Custom18"
                },
                new OptionsConfig
                {
                    Key = "Dosage|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTDos@",languageId),
                    ParentKey = "Custom19"
                },
                new OptionsConfig
                {
                    Key = "GuidelinesId|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTGuid@",languageId),
                    ParentKey = "Custom20"
                },
                new OptionsConfig
                {
                    Key = "Measurement|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTMeas@",languageId),
                    ParentKey = "Custom21"
                },
                new OptionsConfig
                {
                    Key = "SusceptibilityId|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTSusc@",languageId),
                    ParentKey = "Custom22"
                },
                //new OptionsConfig
                //{
                //    Key = "ASTSpecialConsideration|Custom|AST|Custom",
                //    Text = await _languageHandler.TranslateAsync("@ExpASTSpeCon@",languageId),
                //    ParentKey = "Custom23"
                //},
                new OptionsConfig
                {
                    Key = "CategoryId|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTCat@",languageId),
                    ParentKey = "Custom24"
                },
                new OptionsConfig
                {
                    Key = "TestMethodId|Custom|AST|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpASTMeth@",languageId),
                    ParentKey = "Custom25"
                },
                new OptionsConfig
                {
                    Key = "AccessionIsolateNumber|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpAccIsoNum@",languageId),
                    ParentKey = "Custom26"
                },
                new OptionsConfig
                {
                    Key = "fullorganisationname|Custom|Specimen|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpOrgA@",languageId),
                    ParentKey = "Custom27"
                },
                new OptionsConfig
                {
                    Key = "fullyqualifiedname|Custom|Specimen|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpLocA@",languageId),
                    ParentKey = "Custom28"
                },
                new OptionsConfig
                {
                    Key = "AntibioticMeasurement|Custom|Culture|Custom",
                    Text = await _languageHandler.TranslateAsync("@ExpFldAntiB@", languageId),
                    ParentKey = "Custom29"
                },
            };
        }

        private async ValueTask<string> TableTranslatorAsync(string tableName, string languageId)
        {
            return tableName.ToLower() switch
            {
                "specimen" => await _languageHandler.TranslateAsync("@SinSpe@", languageId),
                "patient" => await _languageHandler.TranslateAsync("@SinPat@", languageId),
                "culture" => await _languageHandler.TranslateAsync("@SinCul@", languageId),
                _ => tableName,
            };
        }
    }
}
