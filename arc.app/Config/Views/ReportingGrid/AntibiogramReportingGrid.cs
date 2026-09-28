using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ReportingGrid
{
    /// <summary>
    /// Provides configuration for the antibiogram reporting grid, including filters and export actions.
    /// </summary>
    internal class AntibiogramReportingGrid
    {
        /// <summary>
        /// Builds and returns a configured <see cref="ReportingGridConfig"/> for the antibiogram report.
        /// Location and Client Organisation filters use hierarchical pickers with descendant expansion on query.
        /// </summary>
        /// <returns>A deserialized <see cref="ReportingGridConfig"/> defining the antibiogram view layout and filters.</returns>
        internal ReportingGridConfig GetView()
        {

            var view = @"{
                            'name': 'antibiogram',
                            'type': 'reportgrid',
                            'buttons': [
                                { key: 'exportdata', text: '@GraExp@', icon: 'Installation', uievent: 'locationgraphuievent' }
                            ],
                            'filters': [
                                { key: 'antibiotic', placeholder: '@RepAnt@', multiSelect: true, width: 240, optionsName: 'antibiotic', fieldName: 'antibioticid', dynamic: true },
                                { key: 'organism', placeholder: '@GenOrgA@', multiSelect: true, width: 240, optionsName: 'location', fieldName: 'organismid', dynamic: true },
                                { key: 'susceptibility', placeholder: '@GenSus@', multiSelect: false, width: 140, optionsName: 'TestResult', fieldName: 'susceptibilityid' },
                                { key: 'type', placeholder: '@GenTyp@', multiSelect: false, width: 200, optionsName: 'AntibiogramType', fieldName: 'AntibiogramTypeId' },
                                { key: 'organisation', placeholder: '@GenOrgC@', multiSelect: true, width: 240, optionsName: 'OrganisationList', fieldName: 'organisationfilterid', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false, dynamic: true },
                                { key: 'location', placeholder: '@GenLoc@', multiSelect: true, width: 160, optionsName: 'LocationList', fieldName: 'LocationId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false }
                            ],
                            'dateSearch': 'range',
                            'filtersearch': false
                         }";


            var result = JsonConvert.DeserializeObject<ReportingGridConfig>(view);

            return result;
        }
    }
}
