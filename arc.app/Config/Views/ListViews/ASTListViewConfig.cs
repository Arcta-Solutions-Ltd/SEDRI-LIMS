using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the AST list view.
    /// </summary>
    internal static class ASTListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the AST list view.
        /// </summary>
        /// <returns>The list view configuration for theAST list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'ast',
                            'type': 'ManageList',
                            'title': '@AstMan@',
                            'headerText': '@AstCre@.',
                            'queryName': 'ASTListByCultureId',
                            'displayIfEmpty': false,
                            'parentId': 'CultureId',
                            'gridColumns': [
                                { key: 'alert', name: '', fieldName: '', minWidth: 20, maxWidth: 20, isResizable: false, isCollapsible: false },
                                { 'key': 'column1', 'name': '@GenMet@', 'fieldName': 'TestMethod', 'minWidth': 100, 'maxWidth': 100, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@GenAnt@', 'fieldName': 'AntibioticName', 'minWidth': 160, 'maxWidth': 160, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column3', 'name': '@GenDos@', 'fieldName': 'Dosage', 'minWidth': 100, 'maxWidth': 100, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column4', 'name': '@GenMea@', 'fieldName': 'Measurement', 'minWidth': 100, 'maxWidth': 100, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column5', 'name': '@GenSus@', 'fieldName': 'Susceptibility', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column6', 'name': '@BreSpeB@', 'fieldName': 'SpecialConsideration', 'minWidth': 100, 'maxWidth': 300, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column7', 'name': '@GenInc@', 'fieldName': 'DisplayOnReport', 'minWidth': 100, 'maxWidth': 160, 'isResizable': true, isCollapsible: true }
                            ],
                            'buttons': []
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}

