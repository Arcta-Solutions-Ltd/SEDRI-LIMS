using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using arc.common.Models.Config;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.common.ExtensionMethods;
using arc.app.Configuration;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Event class for adding a page.
    /// </summary>
    internal class AddPageEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPageEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public AddPageEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the add page event asynchronously.
        /// </summary>
        /// <param name="dataToSave">The data to save.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="command">The event command model.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>An integer indicating the result of the operation.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<AddPageModel>(dataToSave);
            var formName = PageGroupUtils.ResolveFormName(dataModel?.Id) ?? PageGroupUtils.ResolveFormName(id);

            var formConfigDefinition = _serviceProvider.GetRequiredService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetRequiredService<IFormConfigRepository>();
            var configRepository = _serviceProvider.GetRequiredService<IConfigRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            var formToAddPageTo = await formConfigDefinition.LoadFormAsync(formName);

            var pageName = GeneratePageName(dataModel.Name);
            var newPageName = await GetUniquePageNameAsync(configRepository, pageName);

            var tableName = FormPageTargetTableExtensions.IsSpecimenRecordForm(formToAddPageTo.SingleItemName)
                ? NormalizeTableName(dataModel.TableName)
                : null;

            formToAddPageTo.AddPage(
                newPageName,
                dataModel.Name,
                dataModel.Description,
                "add, edit, delete",
                tableName);

            var newPage = formToAddPageTo.PagesConfig?
                .LastOrDefault(p => p != null && string.Equals(p.Name, newPageName, StringComparison.OrdinalIgnoreCase));
            try
            {
                PageGroupUtils.AssignGroupFromTableName(newPage, formToAddPageTo);
                if (!string.IsNullOrWhiteSpace(newPage?.PageGroup))
                {
                    var normalised = PageGroupUtils.Normalise(formToAddPageTo, formToAddPageTo.Pages);
                    PageGroupUtils.ApplyOrder(formToAddPageTo, normalised);
                }
            }
            catch (Exception ex)
            {
                logWriter?.LogError(
                    $"AddPageEvent: failed to place page {newPageName} in a group on form {formName}: {ex}",
                    nameof(AddPageEvent),
                    nameof(RunAsync));
            }

            logWriter?.LogInfo(
                $"AddPageEvent: form={formToAddPageTo.Name}, page={newPageName}, table={tableName}, pageGroup={newPage?.PageGroup ?? "(none)"}",
                nameof(AddPageEvent),
                nameof(RunAsync));

            await formConfigRepository.UpdateFormAsync(formToAddPageTo);

            return 0;
        }

        /// <summary>
        /// Generates a page name based on the provided name.
        /// </summary>
        /// <param name="name">The provided name.</param>
        /// <returns>The generated page name.</returns>
        private string GeneratePageName(string name)
        {
            var pageName = name.Replace(" ", "").ToLower().RemoveSpecialCharacters().Trim();
            return pageName.Length > 57 ? pageName[..57] : pageName;
        }

        /// <summary>
        /// Gets a unique page name from the configuration repository.
        /// </summary>
        /// <param name="configRepository">The configuration repository.</param>
        /// <param name="pageName">The page name to ensure uniqueness for.</param>
        /// <returns>The unique page name.</returns>
        private async Task<string> GetUniquePageNameAsync(IConfigRepository configRepository, string pageName)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("Name", pageName);
            queryFilter.AddString("Suffix", "page");

            var newPageName = await configRepository.GetNextAvailableNameAsync(queryFilter);

            return newPageName.Trim();
        }

        private static string NormalizeTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return "specimen";
            }

            var normalized = FormPageTargetTableExtensions.Normalize(tableName);
            return FormPageTargetTableExtensions.IsSpecimenFormTarget(normalized) ? normalized : "specimen";
        }
    }

}
