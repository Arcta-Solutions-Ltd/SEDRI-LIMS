using arc.app.Common;
using arc.app.SystemConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    public class FormConfigRepository : IFormConfigRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;

        public FormConfigRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
        }

        public async Task<string> GetNextAvailableFieldNameAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningStringAsync(new NextFreeFieldNameQuery(), "Get next available field name", queryFilters);
        }

        public async Task<int> AddNewFormAsync(FullFormConfig newForm)
        {
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddNewFormCommand(), "Add a new form", newForm);
        }

        public async Task<int> UpdateFormAsync(FullFormConfig newForm)
        {
            return await _sqlCommand.CommandWithTypeQueryAsync(new UpdateFormCommand(), "Update form config", newForm);
        }

        public async Task<int> UpdatePageAsync(PageConfig pageToSave)
        {
            return await _sqlCommand.CommandWithTypeQueryAsync(new UpdatePageCommand(), "Update page definition", pageToSave);
        }

        public async Task<int> DeleteFormAsync(FullFormConfig formToDelete)
        {
            return await _sqlCommand.CommandWithTypeQueryAsync(new DeleteFormCommand(), "Delete a form", formToDelete);
        }
    }
}
