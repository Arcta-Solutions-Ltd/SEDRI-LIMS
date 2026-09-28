using arc.app.Common;
using arc.app.Exports.ExportFieldProcessors;
using arc.app.SystemConfig;
using arc.common.Models;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public class ExportPostQueryProcessor : IExportPostQueryProcessor
    {
        private readonly IExportRepository _exportRepository;
        private readonly IWhonetAntibioticColumns _whonetAntibioticColumns;
        private readonly IListRepository _listRepository;
        private readonly IJsonWholeStructureFieldsCollector _jsonWholeStructureFieldsCollector;
        private readonly IConfigRepository _configRepository;
        private readonly IJsonReplacer _jsonReplacer;

        public ExportPostQueryProcessor(IExportRepository exportRepository, IWhonetAntibioticColumns whonetAntibioticColumns, IListRepository listRepository, IJsonWholeStructureFieldsCollector jsonWholeStructureFieldsCollector,
            IConfigRepository configRepository, IJsonReplacer jsonReplacer)
        {
            _exportRepository = exportRepository;
            _whonetAntibioticColumns = whonetAntibioticColumns;
            _listRepository = listRepository;
            _jsonWholeStructureFieldsCollector = jsonWholeStructureFieldsCollector;
            _configRepository = configRepository;
            _jsonReplacer = jsonReplacer;
        }

        public async Task<ExportProcessResult> Process(List<string> data, List<ExportProfileFieldModel> exportProfileFields, List<FieldConfig> fieldConfigs, QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            var newData = new List<string>();
            var columnKeys = new List<string>();
            var lineCount = 1;
            var whonetProcessor = new WhonetProcessor();
            var commentProcessor = new CommentProcessor();
            var listItemProcessor = new ListItemProcessor();
            var multiSelectComboProcessor = new MultiSelectComboProcessor();
            var gridProcessor = new GridProcessor();
            var mappingProcessor = new ExportMappingProcessor();
            var astResultsProcessor = new AstResultProcessor();
            var antibioticSusceptibilityProcessor = new AntibioticSusceptibilityProcessor();

            if (exportProfileFields.Any(p => p.FieldName.ToLower() == "whonetantibiotic") || exportProfileFields.Any(p => p.FieldName.ToLower() == "qualitativeantibioticsusceptibility") || exportProfileFields.Any(p => p.FieldName.ToLower() == "antibioticmeasurement"))
            {
                await astResultsProcessor.InitialiseAsync(_exportRepository, queryFilters, token, exportProfileFields, _jsonReplacer);
            }

            if (exportProfileFields.Any(p => p.MoreData?.Contains("Mapping") ?? false))
            {
                await mappingProcessor.InitialiseAsync(_configRepository);
            }

            if (exportProfileFields.Any(p => p.FieldName.ToLower() == "whonetantibiotic"))
            {
                whonetProcessor.Initialise(_whonetAntibioticColumns, astResultsProcessor.GetASTResultsList(), queryFilters, token);
            }

            if (exportProfileFields.Any(p => p.FieldName.ToLower() == "specimencomments") || exportProfileFields.Any(p => p.FieldName.ToLower() == "culturecomments"))
            {
                await commentProcessor.InitialiseAsync(_exportRepository, queryFilters, token);
            }

            if (fieldConfigs.Any(p => p.MultiSelect) || fieldConfigs.Any(p => p.GridFields.Count > 0))
            {
                await listItemProcessor.InitialiseAsync(_listRepository);
            }

            foreach (var line in data)
            {
                var dataLine = line.Split("|");
                var newLine = "";
                var elementCount = 0;

                foreach (var profileField in exportProfileFields)
                {
                    var fieldName = profileField.FieldName.ToLower().Trim();
                    var newElement = "";
                    var fieldConfig = fieldConfigs.FirstOrDefault(p => p.Id.ToLower() == fieldName);
                    var gridCount = fieldConfig?.GridFields?.Count > 0;
                    var isMultiSelect = fieldConfig == null ? false : fieldConfig?.MultiSelect;
                    var mapping = !string.IsNullOrEmpty(profileField.MoreData) && profileField.MoreData != "{}" ? _jsonReplacer.GetValueInJsonString(profileField.MoreData, "Mapping") : "" ;
                    var antibiotic = !string.IsNullOrEmpty(profileField.MoreData) && profileField.MoreData != "{}" ? _jsonReplacer.GetValueInJsonString(profileField.MoreData, "Antibiotic") : "";
                    var WhonetQualitativeValues = !string.IsNullOrEmpty(profileField.MoreData) && profileField.MoreData != "{}" ? _jsonReplacer.GetValueInJsonString(profileField.MoreData, "UseQualitativeValues") : "No";
                    var blankColumnDefault = !string.IsNullOrEmpty(profileField.MoreData) && profileField.MoreData != "{}" ? _jsonReplacer.GetValueInJsonString(profileField.MoreData, "BlankColumnDefault") : "";
                    var includeMICComparison = !string.IsNullOrEmpty(profileField.MoreData) && profileField.MoreData != "{}" ? _jsonReplacer.GetValueInJsonString(profileField.MoreData, "IncludeMICComparison") : "No";
                    //var specialConsideration = !string.IsNullOrEmpty(profileField.MoreData) && profileField.MoreData != "{}" ? _jsonReplacer.GetValueInJsonString(profileField.MoreData, "SpecialConsiderationId") : "";

                    if (fieldName == "locationhierarchy" || fieldName == "organisationhierarchy"|| fieldName == "organisationcodehierarchy" || fieldName == "locationcodehierarchy")
                    {
                        var processor = new HierarchyProcessor();

                        if (lineCount == 1)
                        {
                            newElement = processor.GetHeader(profileField.HeaderName);
                        }
                        else
                        {
                            newElement = processor.GetLine(profileField.HeaderName, dataLine[elementCount]);
                        }
                    }
                    else if (fieldName == "whonetantibiotic")
                    {
                        if (lineCount == 1)
                        {
                            newElement = whonetProcessor.GetHeader();
                        }
                        else
                        {
                            newElement = whonetProcessor.GetLine(int.TryParse(dataLine[elementCount], out int cultureId) ? cultureId : 0, WhonetQualitativeValues);
                        }
                    } 
                    else if ((bool)isMultiSelect)
                    {
                        newElement = multiSelectComboProcessor.GetLine(listItemProcessor, dataLine[elementCount]);
                    }
                    else if (gridCount && lineCount > 1 && !string.IsNullOrEmpty(dataLine[elementCount]))
                    {
                        newElement = gridProcessor.GetLine(listItemProcessor, multiSelectComboProcessor, _jsonWholeStructureFieldsCollector, dataLine[elementCount], fieldConfig);
                    }
                    else if (fieldName == "specimencomments" || fieldName == "culturecomments")
                    {
                        commentProcessor.GetComments(profileField, _jsonWholeStructureFieldsCollector);
                        if (lineCount == 1)
                        {
                            newElement = commentProcessor.GetHeader(profileField.HeaderName);
                        }
                        else
                        {
                            newElement = commentProcessor.GetLine(int.TryParse(dataLine[elementCount], out int parentId) ? parentId : 0);
                        }
                    }
                    else if (fieldName == "qualitativeantibioticsusceptibility" || fieldName == "antibioticmeasurement")
                    {
                        WhonetQualitativeValues = fieldName == "qualitativeantibioticsusceptibility" ? "Yes" : "No";
                        if (lineCount == 1)
                        {
                            newElement = profileField.HeaderName;
                        }
                        else
                        {
                            newElement = antibioticSusceptibilityProcessor.GetLine(astResultsProcessor.GetASTResultsList(antibiotic), dataLine[elementCount], _jsonReplacer.GetValueInJsonString(profileField.MoreData, "TestMethod"),
                                _jsonReplacer.GetValueInJsonString(profileField.MoreData, "TestMethodPrecedence"), WhonetQualitativeValues, includeMICComparison);
                        }
                    }
                    else if (fieldName == "blankcolumn")
                    {
                        if (lineCount == 1)
                        {
                            newElement = profileField.HeaderName;
                        }
                        else
                        {
                            newElement = string.IsNullOrEmpty(blankColumnDefault) ? newElement : blankColumnDefault;
                        }
                    }
                    else
                    {
                        newElement = dataLine[elementCount];
                    }

                    if (!string.IsNullOrEmpty(mapping))
                    {
                        newElement = mappingProcessor.GetLine(mapping, newElement);
                    }

                    // On the header pass, record one column key per emitted column so the format
                    // writers can resolve mapping attributes to columns by field id. Fields that
                    // expand into several columns (hierarchy, WHONET, comments) repeat their key.
                    if (lineCount == 1)
                    {
                        var columnCount = string.IsNullOrEmpty(newElement) ? 1 : newElement.Split('|').Length;
                        for (var keyIndex = 0; keyIndex < columnCount; keyIndex++)
                        {
                            columnKeys.Add(fieldName);
                        }
                    }

                    newLine += elementCount > 0 ?  "|" + newElement : newElement;
                    elementCount++;
                }
                var emptyNewLine = newLine.Replace("|", "") == "";
                if(!emptyNewLine)
                {
                    newData.Add(newLine);
                }
                lineCount++;
            }
           return new ExportProcessResult { Lines = newData, ColumnKeys = columnKeys };
        }
    }
}
