using arc.app.Common;
using arc.app.Config.Forms;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Builds the list of request forms whose specimen types can be restricted by laboratory configuration.
/// The eligible forms are held as values of the RequestFormList list so a site can extend them through table
/// maintenance, and the option key is the form name rather than a list item id because that is what the
/// formspecimentypeoption configuration stores.
/// </summary>
/// <param name="listRepository">Supplies the eligible form names.</param>
/// <param name="formConfigAdapter">Resolves each form name to its display title.</param>
internal class RequestFormListQuery(IListRepository listRepository, IFormConfigAdapter formConfigAdapter)
{
    private readonly IListRepository _listRepository = listRepository;
    private readonly IFormConfigAdapter _formConfigAdapter = formConfigAdapter;

    /// <summary>
    /// Retrieves the request forms as combobox options keyed on form name.
    /// </summary>
    /// <returns>An option per eligible form, titled with the form's own title where one can be resolved.</returns>
    internal async Task<List<OptionsConfig>> GetListAsync()
    {
        var options = new List<OptionsConfig>();

        foreach (var item in await _listRepository.GetListValuesAsync("RequestFormList", true))
        {
            var formName = item.Text?.Trim();
            if (string.IsNullOrEmpty(formName))
            {
                continue;
            }

            var form = await _formConfigAdapter.GetFormAsync(formName);
            options.Add(new OptionsConfig { Key = formName, Text = form?.Title ?? formName });
        }

        return options;
    }
}
