using arc.app.Common;
using arc.app.Security;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Users
{
    public class DeleteUserEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteUserEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var userRepository = _serviceProvider.GetService<IUserRepository>();
            await userRepository.DeleteUserAsync(id);
            return int.Parse(id);
        }
    }
}
