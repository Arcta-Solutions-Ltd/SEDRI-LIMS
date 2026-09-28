using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using System.Collections.Generic;

namespace arc.app.Config
{
    public interface IGetListsConfiguration
    {
        List<ListConfig> GetConfiguration(List<ListViewConfig> viewsToDisplay, List<PageConfig> pagesToDisplay, List<GraphConfig> graphsToDisplay, List<ReportingGridConfig> reportingGridsToDisplay, TokenInfoModel token);
    }
}
