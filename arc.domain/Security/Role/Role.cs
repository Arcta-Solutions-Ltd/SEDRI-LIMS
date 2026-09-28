using arc.domain.ExtensionMethods;
using arc.domain.Security.Role;
using Newtonsoft.Json;

namespace arc.domain.Security.Permission
{
    /// <summary>
    /// Represents a role with associated permissions and details.
    /// </summary>
    public class Role
    {
        private string _menuPermissions = "";
        private string _eventPermissions = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            MenuPermissionDetails = new MenuPermissionDetails();
            EventPermissionDetails = new EventPermissionDetails();
        }

        /// <summary>
        /// Gets or sets the unique identifier for the role.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is enabled.
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        public string RoleDescription { get; set; }

        /// <summary>
        /// Gets or sets the menu permissions as a JSON string.
        /// When set, it also deserializes the JSON string to <see cref="MenuPermissionDetails"/>.
        /// </summary>
        public string MenuPermission
        {
            get { return _menuPermissions; }
            set
            {
                _menuPermissions = value ?? string.Empty;
                MenuPermissionDetails = string.IsNullOrWhiteSpace(_menuPermissions)
                    ? new MenuPermissionDetails()
                    : JsonConvert.DeserializeObject<MenuPermissionDetails>(_menuPermissions).NormalizeMenu();
            }
        }

        /// <summary>
        /// Gets or sets the event permissions as a JSON string.
        /// When set, it also deserializes the JSON string to <see cref="EventPermissionDetails"/>.
        /// </summary>
        public string EventPermission
        {
            get { return _eventPermissions; }
            set
            {
                _eventPermissions = value ?? string.Empty;
                EventPermissionDetails = string.IsNullOrWhiteSpace(_eventPermissions)
                    ? new EventPermissionDetails()
                    : JsonConvert.DeserializeObject<EventPermissionDetails>(_eventPermissions).NormalizeEvents();
            }
        }

        /// <summary>
        /// Gets or sets additional data related to the role.
        /// </summary>
        public string MoreData { get; set; }

        /// <summary>
        /// Gets the details of the menu permissions.
        /// </summary>
        public MenuPermissionDetails MenuPermissionDetails { get; private set; }

        /// <summary>
        /// Gets the details of the event permissions.
        /// </summary>
        public EventPermissionDetails EventPermissionDetails { get; private set; }

        /// <summary>
        /// Adds additional menu permissions to the current role.
        /// </summary>
        /// <param name="permissionsToAdd">The permissions to add as a JSON string.</param>
        public void AddMenuPermissions(string permissionsToAdd)
        {
            if (string.IsNullOrWhiteSpace(permissionsToAdd))
            {
                return;
            }

            var newPermissionDetails = JsonConvert.DeserializeObject<MenuPermissionDetails>(permissionsToAdd).NormalizeMenu();
            MenuPermissionDetails.CombineSidebarItems(newPermissionDetails.AllowedSidebarItems);
        }

        /// <summary>
        /// Adds additional event permissions to the current role.
        /// </summary>
        /// <param name="permissionsToAdd">The permissions to add as a JSON string.</param>
        public void AddEventPermissions(string permissionsToAdd)
        {
            if (string.IsNullOrWhiteSpace(permissionsToAdd))
            {
                return;
            }

            var newPermissionDetails = JsonConvert.DeserializeObject<EventPermissionDetails>(permissionsToAdd).NormalizeEvents();
            EventPermissionDetails.CombineEventItems(newPermissionDetails.AllowedEvents);
        }

        /// <summary>
        /// Ensures menu and event permission detail collections are non-null empty arrays when unset.
        /// </summary>
        public void NormalizePermissionDetails()
        {
            MenuPermissionDetails = MenuPermissionDetails.NormalizeMenu();
            EventPermissionDetails = EventPermissionDetails.NormalizeEvents();
        }
    }

}
