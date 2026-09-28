using arc.app.Common;
using arc.app.Security;
using arc.common.Models;
using arc.common.Models.Organisation;
using arc.common.Models.Specimen;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace arc.app.Security
{
    internal class EditOrganisationValidator : ISpecialValidatorAsync
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly string _message;
        private readonly TokenInfoModel _token;

        public EditOrganisationValidator(IOrganisationRepository organisationRepository, string message, TokenInfoModel token)
        {
            _organisationRepository = organisationRepository;
            _message = message;
            _token = token;
        }

        public async Task<string> ValidateMessageAsync()
        {
            var organisation = JsonConvert.DeserializeObject<OrganisationModel>(_message);
            if (string.IsNullOrEmpty(organisation.ParentOrganisationId)) { return ""; }

            var orgList = await _organisationRepository.GetOrganisationsForListAsync(_token);

            var orgWalker = orgList.Where(o => o.Key == organisation.ParentOrganisationId).First();
            while (orgWalker!= null)
            {
                if (orgWalker.Key == organisation.Id) { return "@RemCir@";  }
                if (string.IsNullOrEmpty(orgWalker.ParentKey) || orgWalker.ParentKey == "0") { return ""; }
                orgWalker = orgList.Where(o => o.Key == orgWalker.ParentKey).First();
            }
            return "";
        }
    }
}
