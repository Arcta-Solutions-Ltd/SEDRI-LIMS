using arc.app.Config.Sidebar;
using arc.app.Config;
using arc.app.Configuration;
using arc.common.ExtensionMethods;
using arc.common.Models;
using System.Threading.Tasks;
using System.Linq;

namespace arc.app.Security;
public class QueryPermissions : IQueryPermissions
{
    private readonly IPermissionHandler _permissionHandler;
    private readonly IConfigCache _configCache;
    private readonly IConfigListUtils _configListUtils;

    public QueryPermissions(IPermissionHandler permissionHandler, IConfigCache configCache, IConfigListUtils configListUtils)
    {
        _permissionHandler = permissionHandler;
        _configCache = configCache;
        _configListUtils = configListUtils;
    }

    public async Task<string> GetQueryPermssionStringAsync(TokenInfoModel token, string language)
    {
        var returnString = "";

        var userName = token.Username;

        // Retrieve role permissions
        var roleWithPermissions = await _permissionHandler.GetPermissionsAsync(userName);
        var allowedSidebarItems = roleWithPermissions.MenuPermissionDetails.AllowedSidebarItems.OrEmpty();
        var allowedEvents = roleWithPermissions.EventPermissionDetails.AllowedEvents.OrEmpty();

        // Get left sidebar configuration
        var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();
        sidebarConfig.OnlyKeepTheseSidebarItems(allowedSidebarItems);

        await _configCache.LoadConfig();

        var listViewsToDisplay = _configListUtils.GetListViewConfigList([.. allowedSidebarItems], []);
        var recordViewsToDisplay = _configListUtils.GetRecordViewConfigList(listViewsToDisplay, []);
        var formsToDisplay = await _configListUtils.GetFormConfigListAsync([.. allowedEvents]);
        var (pagesToDisplay, uievents) = await _configListUtils.GetPagesAsync();

        //check whether 'patientsearchpage' exists in the page structures and set return string to 'NPS' if it does not

        if (pagesToDisplay.Any(p => p.Name is "patientsearchpage")) {
            returnString = "PS";
        }

        if (listViewsToDisplay.Any(l => l.Name is "patients"))
        {
            returnString = returnString == "" ? "PA" : returnString + ",PA";
        }

        if (listViewsToDisplay.Any(l => l.Name is "exportprofile"))
        {
            returnString = returnString == "" ? "EX" : returnString + ",EX";
        }

        if (recordViewsToDisplay.Any(l => l.Name is "specimenrecordview"))
        {
            returnString = returnString == "" ? "SP" : returnString + ",SP";
        }

        if (listViewsToDisplay.Any(l => l.Name is "archive"))
        {
            returnString = returnString == "" ? "AR" : returnString + ",AR";
        }

        if (listViewsToDisplay.Any(l => l.Name is "monitoring"))
        {
            returnString = returnString == "" ? "MO" : returnString + ",MO";
        }

        return returnString;
    }
}
