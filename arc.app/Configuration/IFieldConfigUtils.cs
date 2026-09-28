using arc.common.Models.Config;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IFieldConfigUtils
    {
        Task<FullFormConfig> AddFieldToSystemAsync(string newFieldName, EditFieldModel newFieldInfo, FullFormConfig formToUpdate, string dataToSave, string pageToAddFieldTo, int orderIndex = -1);

        /// <summary>
        /// Adds an existing field to a form by reference, keeping the field's <c>Id</c> verbatim so it
        /// shares the same physical column or MoreData key as the source. Does not reserve a new field
        /// name because the identifier already exists in <c>namelist</c>.
        /// </summary>
        /// <param name="fieldToReference">Field configuration cloned from the source form.</param>
        /// <param name="formToUpdate">Target form configuration to mutate.</param>
        /// <param name="pageToAddFieldTo">Target page name id.</param>
        /// <param name="requiredErrorMessage">Required validation message carried over from the source form.</param>
        /// <param name="formGroupKey">Target form group key, or null to append to the last form group.</param>
        /// <param name="columnKey">Target column key, used with <paramref name="formGroupKey"/>.</param>
        /// <param name="orderIndex">Optional insertion index within the form group; -1 appends.</param>
        /// <returns>The updated form configuration.</returns>
        Task<FullFormConfig> AddExistingFieldToSystemAsync(
            FieldConfig fieldToReference,
            FullFormConfig formToUpdate,
            string pageToAddFieldTo,
            string requiredErrorMessage = null,
            string formGroupKey = null,
            string columnKey = null,
            int orderIndex = -1);

        Task<FullFormConfig> DeleteFieldFromSystemAsync(string fieldToDelete, FullFormConfig formToUpdate);
    }
}
