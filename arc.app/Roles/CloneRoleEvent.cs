using arc.app.Common;
using arc.common;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Roles;

public class CloneRoleEvent : IRun
{
    private readonly IJsonUtils _jsonUtils;
    public IRoleRepository _roleRepository;

    public CloneRoleEvent(IJsonUtils jsonUtils, IRoleRepository roleRepository)
    {
        _jsonUtils = jsonUtils;
        _roleRepository = roleRepository;
    }

    public async Task<int> RunAsync(string dataToSave, string roleId, EventModel command, EventConfig eventData = null)
    {
        var roleName = _jsonUtils.GetSingleFieldValue(dataToSave, "NewRoleName");
        var roleDescription = _jsonUtils.GetSingleFieldValue(dataToSave, "NewRoleDescription");

        return await _roleRepository.CloneRoleAsync(int.Parse(roleId), roleName, roleDescription);

    }
}
