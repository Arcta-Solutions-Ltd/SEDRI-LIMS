using arc.common.Models;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Models.Instruments;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.AST
{
    public interface IASTRepository
    {
        Task<int> UpdateASTAsync(string dataToSave, string username);
        Task<List<ASTModel>> GetASTTestResultsAsync(string id);
        Task<List<ASTListByCultureIdModel>> GetAstListByCultureIdAsync(QueryFilterConfig queryFilters);
        Task<SpecimenPatientModel> GetSpecimenAndPatientAsync(int id);
        Task<List<AntibioticListForReportModel>> GetAstAntibioticListAsync(QueryFilterConfig queryFilters);
        Task<List<SusceptibilityModel>> RetrieveSusceptibilitiesAsync(QueryFilterConfig parameters);

        /// <summary>
        /// Instrument-reported resistance mechanisms stored for the culture (isolate).
        /// </summary>
        Task<List<ResistanceMechanismModel>> GetResistanceMechanismsByCultureIdAsync(int cultureId);

        /// <summary>
        /// Resolves the display name of an organism by id (preferred synonym, otherwise genus/species/subspecies/serotype).
        /// Returns an empty string when the organism id is 0 or no match is found.
        /// </summary>
        Task<string> GetOrganismNameByIdAsync(int organismId);

        /// <summary>
        /// Loads persisted expert-rule evaluation context (suppressed manual rows) for the culture, if any.
        /// </summary>
        Task<ExpertRuleEvalContextModel?> GetExpertRuleEvalContextAsync(string cultureId);
    }
}
