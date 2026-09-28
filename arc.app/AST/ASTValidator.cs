using arc.app.Coding;
using arc.app.Common;
using arc.app.Configuration;
using arc.app.Specimen;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.AST;
using arc.data.model.Laboratory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.AST
{
    internal class ASTValidator : ISpecialValidator
    {
        private readonly string _message;
        private readonly ILogWriter _logWriter;
        private readonly IAntibioticRepository _antibioticRepository;
        private readonly IGeneralRepository _generalRepository;
        private readonly ICultureRepository _cultureRepository;

        public ASTValidator(string message, ILogWriter logWriter, IAntibioticRepository antibioticRepository,
            IGeneralRepository generalRepository, ICultureRepository cultureRepository)
        {
            _message = message;
            _logWriter = logWriter;
            _antibioticRepository = antibioticRepository;
            _generalRepository = generalRepository;
            _cultureRepository = cultureRepository;
        }

        /// <summary>
        /// Validates the flattened AST save payload and returns a language tag message when invalid.
        /// </summary>
        /// <returns>Empty string when valid; otherwise a translated language tag message.</returns>
        public string ValidateMessage()
        {
            var data = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<ASTCraftedModel>>(_message);

            var ASTData = data.Crafted[0].Contents[0].Value;
            var antibioticNames = LoadAntibioticNames(ASTData.ASTResults);
            var cultureId = ASTData.CultureId;

            var participatingAntibioticIds = new HashSet<int>();

            var index = 0;
            if (ASTData.ASTResults.Count == 0)
            {
                return "@AstEmpA@";
            }
            foreach (var ASTEntry in ASTData.ASTResults)
            {
                index++;

                if (!ASTEntry.ParticipatesInDuplicateCheck())
                {
                    ASTEntry.TryGetAntibioticId(out var skippedAntibioticId);
                    _logWriter.LogInfo(
                        $"AST duplicate check skipped (no resolved susceptibility): CultureId={cultureId}, RowIndex={index}, AntibioticId={skippedAntibioticId}, ExpertRuleLine={ASTEntry.ExpertRuleLine}",
                        "ASTValidator",
                        "ValidateMessage");
                }
                else
                {
                    var exactDuplicateTag = AstDuplicateChecker.CheckExactDuplicateRow(ASTEntry, ASTData.ASTResults);
                    if (!string.IsNullOrEmpty(exactDuplicateTag))
                    {
                        ASTEntry.TryGetAntibioticId(out var antibioticId);
                        ASTEntry.TryGetTestMethodId(out var testMethodId);
                        var conflictCount = AstDuplicateChecker.CountAntibioticConflicts(ASTEntry, ASTData.ASTResults);
                        _logWriter.LogInfo(
                            $"AST exact duplicate row (@AstEmpB@): CultureId={cultureId}, RowIndex={index}, AntibioticId={antibioticId}, TestMethodId={testMethodId}, IncludeInReport={ASTEntry.IncludeInReport ?? "(null)"}, ExpertRuleLine={ASTEntry.ExpertRuleLine}, ParticipatingConflictCount={conflictCount}",
                            "ASTValidator",
                            "ValidateMessage");
                        return FormatAtAntibiotic(exactDuplicateTag, ASTEntry.Antibiotic, antibioticNames);
                    }
                }

                if (ASTEntry.Antibiotic == "")
                {
                    return "@AstEmp@" + " @ValAtl@ " + index.ToString();
                }
                if (ASTEntry.Guidelines == "")
                {
                    return FormatAtAntibiotic("@AstGui@", ASTEntry.Antibiotic, antibioticNames);
                }

                if (ASTEntry.TestMethod == "681") // Disk.
                {
                    if (ASTEntry.Dosage == 0 && !ASTEntry.ExpertRuleLine)
                    {
                        return FormatAtAntibiotic("@AstDos@", ASTEntry.Antibiotic, antibioticNames);
                    }

                    MicMeasurementExtensions.TryParseMicMeasurement(ASTEntry.Measurement, out var diskParts);
                    var diskMeasurement = diskParts.IsBlank
                        ? -1m
                        : diskParts.HasNumericValue
                            ? diskParts.NumericValue
                            : -1m;

                    if (!diskParts.IsBlank && !diskParts.HasNumericValue)
                    {
                        return FormatAtAntibiotic("@AstMea@", ASTEntry.Antibiotic, antibioticNames) + ". @AstMeaA@";
                    }

                    if (diskMeasurement != -1 && (diskMeasurement < 6 || diskMeasurement > 50))
                    {
                        return FormatAtAntibiotic("@AstMea@", ASTEntry.Antibiotic, antibioticNames) + ". @AstMeaA@";
                    }

                    var globalDuplicateTag = AstDuplicateChecker.TrackGlobalDuplicate(
                        ASTEntry,
                        participatingAntibioticIds);
                    if (!string.IsNullOrEmpty(globalDuplicateTag))
                    {
                        ASTEntry.TryGetAntibioticId(out var antibioticId);
                        ASTEntry.TryGetTestMethodId(out var testMethodId);
                        _logWriter.LogInfo(
                            $"AST duplicate antibiotic (@AstDup@): CultureId={cultureId}, RowIndex={index}, AntibioticId={antibioticId}, TestMethodId={testMethodId}, IncludeInReport={ASTEntry.IncludeInReport ?? "(null)"}, ExpertRuleLine={ASTEntry.ExpertRuleLine}",
                            "ASTValidator",
                            "ValidateMessage");
                        return FormatAtAntibiotic(globalDuplicateTag, ASTEntry.Antibiotic, antibioticNames);
                    }
                }

                if (ASTEntry.TestMethod == "680") // MIC.
                {
                    ASTEntry.Dosage = 0; // Dosage pending removal from MIC results.

                    MicMeasurementExtensions.TryParseMicMeasurement(ASTEntry.Measurement, out var micParts);
                    if (!micParts.IsBlank && (micParts.IsOperatorOnly || !micParts.HasNumericValue))
                    {
                        _logWriter.LogInfo(
                            $"AST MIC measurement validation failed: CultureId={cultureId}, RowIndex={index}, TestMethod={ASTEntry.TestMethod}, Measurement={ASTEntry.Measurement ?? "(null)"}",
                            "ASTValidator",
                            "ValidateMessage");
                        return FormatAtAntibiotic("@AstMicMea@", ASTEntry.Antibiotic, antibioticNames) + ". @AstMeaA@";
                    }

                    var globalDuplicateTag = AstDuplicateChecker.TrackGlobalDuplicate(
                        ASTEntry,
                        participatingAntibioticIds);
                    if (!string.IsNullOrEmpty(globalDuplicateTag))
                    {
                        ASTEntry.TryGetAntibioticId(out var antibioticId);
                        ASTEntry.TryGetTestMethodId(out var testMethodId);
                        _logWriter.LogInfo(
                            $"AST duplicate antibiotic (@AstDup@): CultureId={cultureId}, RowIndex={index}, AntibioticId={antibioticId}, TestMethodId={testMethodId}, IncludeInReport={ASTEntry.IncludeInReport ?? "(null)"}, ExpertRuleLine={ASTEntry.ExpertRuleLine}",
                            "ASTValidator",
                            "ValidateMessage");
                        return FormatAtAntibiotic(globalDuplicateTag, ASTEntry.Antibiotic, antibioticNames);
                    }
                }
            }

            var cultureIdForAudit = cultureId != 0 ? cultureId : int.TryParse(data.Id, out var parsedCultureId) ? parsedCultureId : 0;
            if (cultureIdForAudit != 0)
            {
                var recordAudit = LoadRecordSusceptibilityChangeAudit(cultureIdForAudit);
                var overrideValidationTag = AstSusceptibilityOverrideValidator.ValidateSavePayload(
                    ASTData.ASTResults,
                    recordAudit,
                    null,
                    cultureIdForAudit);
                if (!string.IsNullOrEmpty(overrideValidationTag))
                {
                    return overrideValidationTag;
                }
            }

            return "";
        }

        private bool LoadRecordSusceptibilityChangeAudit(int cultureId)
        {
            var culture = _cultureRepository.GetASTCultureDataAsync(cultureId.ToString()).GetAwaiter().GetResult();
            if (culture.LaboratoryId == 0)
            {
                return false;
            }

            var laboratory = _generalRepository
                .GetByIdAsync<LaboratoryDataModel>("laboratory", culture.LaboratoryId)
                .GetAwaiter()
                .GetResult();

            return laboratory?.RecordSusceptibilityChangeAudit == "Yes";
        }

        private Dictionary<int, string> LoadAntibioticNames(List<ASTModel> results)
        {
            if (results == null || results.Count == 0)
            {
                return new Dictionary<int, string>();
            }

            var ids = results
                .Select(result => result.Antibiotic)
                .Where(antibiotic => !string.IsNullOrEmpty(antibiotic))
                .Select(antibiotic => int.TryParse(antibiotic, out var id) ? id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                return new Dictionary<int, string>();
            }

            return _antibioticRepository.GetAntibioticNamesByIdsAsync(ids).GetAwaiter().GetResult();
        }

        private static string FormatAtAntibiotic(string tag, string antibioticIdString, IReadOnlyDictionary<int, string> names)
        {
            if (int.TryParse(antibioticIdString, out var id) &&
                names.TryGetValue(id, out var name) &&
                !string.IsNullOrEmpty(name))
            {
                return tag + ": " + name;
            }

            return tag + ": Antibiotic " + antibioticIdString;
        }
    }
}
