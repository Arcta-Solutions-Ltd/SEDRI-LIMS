using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Security;

/// <summary>
/// Retrieves a combined list of laboratory and organisation options for use in dropdowns or lists.
/// </summary>
internal class LabAndOrgListQuery
{
    private readonly ILaboratoryRepository _laboratoryRepository;
    private readonly IOrganisationRepository _organisationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LabAndOrgListQuery"/> class.
    /// </summary>
    /// <param name="laboratoryRepository">The laboratory repository used to fetch the unfiltered list of laboratories.</param>
    /// <param name="organisationRepository">The organisation repository used to fetch the unfiltered list of organisations.</param>
    internal LabAndOrgListQuery(ILaboratoryRepository laboratoryRepository, IOrganisationRepository organisationRepository)
    {
        _laboratoryRepository = laboratoryRepository;
        _organisationRepository = organisationRepository;
    }

    /// <summary>
    /// Asynchronously retrieves and combines the unfiltered lists of laboratories and organisations.
    /// Each laboratory's key is prefixed with "L" and each organisation's key is prefixed with "O".
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation that returns a combined list of <see cref="OptionsConfig"/>.
    /// </returns>
    internal async Task<List<OptionsConfig>> GetCompleteListAsync()
    {
        // Fetch laboratories and organisations concurrently.
        var laboratoryTask = _laboratoryRepository.GetLaboratoriesForListUnfilteredAsync();
        var organisationTask = _organisationRepository.GetOrganisationsForListUnfilteredAsync();
        await Task.WhenAll(laboratoryTask, organisationTask);

        // Prefix each item's Key with "L" for laboratories and "O" for organisations, then combine the two lists into one.
        var completeList = laboratoryTask.Result
            .Select(l => new OptionsConfig { Key = "L" + l.Key, Text = l.Text })
            .Concat(organisationTask.Result.Select(o => new OptionsConfig { Key = "O" + o.Key, Text = o.Text }))
            .ToList();

        return completeList;
    }
}
