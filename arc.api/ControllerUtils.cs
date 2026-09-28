using arc.app.Security;
using arc.common.Utils;
using arc.domain.Security.Permission;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace arc.api
{
    /// <summary>
    /// Class containing helper methods used within the controllers in the application.
    /// </summary>
    public class ControllerUtils : IControllerUtils
    {
        private readonly ITokenHandler _tokenHandler;
        private readonly IJsonReplacer _jsonReplacer;
        private readonly IPermissionHandler _permissionHandler;
        private Role _roleWithPermissions = new();
        private string _contents = "";

        public ControllerUtils(ITokenHandler tokenHandler, IJsonReplacer jsonReplacer, IPermissionHandler permissionHandler)
        {
            _tokenHandler = tokenHandler;
            _jsonReplacer = jsonReplacer;
            _permissionHandler = permissionHandler;
        }

        /// <summary>
        /// Reads a text stream and puts the contents in a string variable
        /// </summary>
        /// <param name="body">Text stream</param>
        public async Task InitialiseAsync(Stream body)
        {
            using (StreamReader reader = new(body, Encoding.UTF8))
            {
                _contents = await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Gets the contents of previously loaded in the InitialiseAsync method
        /// </summary>
        /// <returns>Contents previosly loaded into the object as string</returns>
        public string GetContents() { return _contents; }

        /// <summary>
        /// Used to check that a user has access to a controller.
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>true of false depending on whether the user should have access to the controller and event tey are trying to run</returns>
        public async Task<bool> CheckAuthorisationAsync(ClaimsPrincipal user)
        {
            var token = _tokenHandler.GetTokenInfo(user);

            if (!string.IsNullOrEmpty(token.Username) )
            {
                _roleWithPermissions = await _permissionHandler.GetPermissionsAsync(token.Username);
                return CheckUserHasAccessToEvent();
            }
            
            return ! string.IsNullOrEmpty(token.Username);
        }

        /// <summary>
        /// Checks whether a user has permissions to access an event. It gets the event from the request contents previously loaded into the object through InitialiseAsync.
        /// </summary>
        /// <returns>true of false depending on whether the user should have access to the event</returns>
        private bool CheckUserHasAccessToEvent()
        {
            var eventName = _jsonReplacer.GetValueInJsonString(_contents, "event");

            if (! string.IsNullOrEmpty(eventName))
            {
                if (string.Equals(eventName, "savehomedashboardevent", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return _roleWithPermissions.EventPermissionDetails.AllowedEvents.Contains(eventName.ToLower());
            }
            return true;
        }
    }
}
