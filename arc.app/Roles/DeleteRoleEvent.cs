using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Roles;

public class DeleteRoleEvent : IRun
{
    public IRoleRepository _roleRepository;

    public DeleteRoleEvent(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<int> RunAsync(string dataToSave, string roleId, EventModel command, EventConfig eventData = null)
    {
        await _roleRepository.DeleteRoleAsync(int.Parse(roleId));
        return int.Parse(roleId);
    }
}
