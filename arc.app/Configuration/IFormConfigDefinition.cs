using arc.common.Models.Config;
using arc.domain.Configuration.FormStructureConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IFormConfigDefinition
    {
        Task<FullFormConfig> LoadFormAsync(string formName);

        /// <summary>
        /// Renames all configs on a cloned form using canonical names from <see cref="CloneTestNames"/>.
        /// </summary>
        Task<FullFormConfig> ResetNamesToNewFormAsync(FullFormConfig formToCopy, CloneTestNames names, string formType, string title, string description);
    }
}
