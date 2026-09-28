using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Config;

/// <summary>
/// Provides configuration for retrieving lists based on various display parameters.
/// </summary>
public class GetListsConfiguration : IGetListsConfiguration
{
    private readonly IListConfigFactory _listConfigFactory;
    private readonly IDataListFactory _datalistFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetListsConfiguration"/> class.
    /// </summary>
    /// <param name="listConfigFactory">Factory for retrieving list configurations.</param>
    /// <param name="datalistFactory">Factory for retrieving data lists.</param>
    public GetListsConfiguration(IListConfigFactory listConfigFactory, IDataListFactory datalistFactory)
    {
        _listConfigFactory = listConfigFactory;
        _datalistFactory = datalistFactory;
    }

    /// <summary>
    /// Retrieves the configuration of lists to display based on provided parameters.
    /// </summary>
    /// <param name="viewsToDisplay">List view configurations.</param>
    /// <param name="pagesToDisplay">Page configurations.</param>
    /// <param name="graphsToDisplay">Graph configurations.</param>
    /// <param name="reportingGridsToDisplay">Reporting grid configurations.</param>
    /// <param name="token">Token containing user information.</param>
    /// <returns>A list of configured lists.</returns>
    public List<ListConfig> GetConfiguration(List<ListViewConfig> viewsToDisplay, List<PageConfig> pagesToDisplay, List<GraphConfig> graphsToDisplay, List<ReportingGridConfig> reportingGridsToDisplay, TokenInfoModel token)
    {
        var listOfLists = new List<string> { "completelaborglist" };
        var listsToDisplay = new List<ListConfig>();

        foreach (var view in viewsToDisplay)
        {
            if (view.Filters != null)
            {
                var options = view.Filters.Where(f => !string.IsNullOrEmpty(f.OptionsName)).Select(f => f.OptionsName.ToLower());
                if (options.Any())
                {
                    listOfLists.AddRange(options);
                }
            }
        }

        foreach (var page in pagesToDisplay)
        {
            if (page?.Columns != null)
            {
                foreach (var column in page.Columns)
                {
                    foreach (var formGroup in column.FormGroups)
                    {
                        var options = formGroup.Fields.Where(f => !string.IsNullOrEmpty(f.OptionsName) && !f.Dynamic).Select(f => f.OptionsName.ToLower());
                        if (options.Any())
                        {
                            listOfLists.AddRange(options);
                        }

                        var fieldGrids = formGroup.Fields.Where(f => f.Type == "fieldgrid");
                        foreach (var field in fieldGrids)
                        {
                            options = field.GridFields.Where(f => !string.IsNullOrEmpty(f.OptionsName)).Select(f => f.OptionsName.ToLower());
                            if (options.Any())
                            {
                                listOfLists.AddRange(options);
                            }
                        }

                        if (formGroup.Fields.Any(f => f.Type?.ToLower() == "organismlist"))
                        {
                            listOfLists.AddRange(new List<string> { "specimenorganism", "specimenorganismcode" });
                        }
                    }
                }
            }
        }

        foreach (var graph in graphsToDisplay)
        {
            if (graph.Filters != null)
            {
                var options = graph.Filters.Where(f => !string.IsNullOrEmpty(f.OptionsName)).Select(f => f.OptionsName.ToLower());
                if (options.Any())
                {
                    listOfLists.AddRange(options);
                }
            }
        }

        foreach (var grid in reportingGridsToDisplay)
        {
            if (grid.Filters != null)
            {
                var options = grid.Filters.Where(f => !string.IsNullOrEmpty(f.OptionsName)).Select(f => f.OptionsName.ToLower());
                if (options.Any())
                {
                    listOfLists.AddRange(options);
                }
            }
        }

        listOfLists.Add("YearSetting");
        listOfLists.Add("dashboardtimerangeunit");
        listOfLists = listOfLists.Distinct().ToList();

        foreach (var list in listOfLists)
        {
            var newList = _listConfigFactory.GetList(list) ?? _datalistFactory.GetListAsync(list, true, token).Result;
            listsToDisplay.Add(newList);
        }
        
        return listsToDisplay;
    }
}
