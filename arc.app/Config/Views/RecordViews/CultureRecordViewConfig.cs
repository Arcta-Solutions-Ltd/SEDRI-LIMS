using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Provides the configuration for rendering the culture record view in the UI.
/// </summary>
/// <remarks>
/// This configuration defines the layout, workflow, and interactive elements for culture-related records.
/// It includes metadata such as title, name, and type, along with a set of action buttons and data regions.
/// Regions include standard queries, crafted grids, and list views for culture details, isolate data,
/// AST results, instrument outputs, and user comments.
/// </remarks>
internal class CultureRecordViewConfig
{
    /// <summary>
    /// Returns the culture record view configuration as a deserialized <see cref="RecordViewConfig"/> object.
    /// </summary>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> instance representing the UI layout and behavior for culture records.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = """
            {
                "title": "@SpeCulA@",
                "name": "cultures",
                "type": "recordview",
                "singleItemName": "@SinIso@",
                "workflow": "SpecimenDefault",
                "buttons": [
                    {
                        "key": "editisolate",
                        "text": "@SpeEdiA@",
                        "icon": "Edit",
                        "onSelect": true,
                        "primaryAction": 1,
                        "uievent": "editisolateuievent",
                        "workflow": true,
                        "onFinish": "refresh"
                    },
                    {
                        "key": "ast",
                        "text": "@AstTes@",
                        "icon": "TestBeakerSolid",
                        "onSelect": true,
                        "primaryAction": 2,
                        "uievent": "ast",
                        "workflow": true,
                        "onFinish": "refresh"
                    },
                    {
                        "key": "editaliquot",
                        "text": "@SpeAliA@",
                        "icon": "Edit",
                        "onSelect": true,
                        "primaryAction": 4,
                        "uievent": "editaliquotuievent",
                        "workflow": true,
                        "onFinish": "refresh"
                    },
                    {
                        "key": "culturetests",
                        "text": "@SpeCulC@",
                        "icon": "TestExploreSolid",
                        "uievent": "culturetestuievent",
                        "workflow": true
                    },
                    {
                        "key": "editaliquot",
                        "text": "@SpeAliA@",
                        "icon": "Edit",
                        "onSelect": true,
                        "primaryAction": 3,
                        "uievent": "editaliquotuievent",
                        "workflow": true,
                        "onFinish": "refresh"
                    },
                    {
                        "key": "addculturecomment",
                        "text": "@GenAddC@",
                        "icon": "CommentAdd",
                        "primaryAction": 4,
                        "uievent": "culturecommentuievent",
                        "workflow": false,
                        "onFinish": "refresh"
                    },
                    {
                        "key": "manageattachments",
                        "text": "@GenAtts@",
                        "icon": "Attach",
                        "uievent": "managecultureattachmentsuievent",
                        "workflow": false,
                        "onFinish": "refresh"
                    }
                ],
                "regions": [
                    {
                        "id": "culturedetails",
                        "type": "standard",
                        "queryName": "cultureforcultureview"
                    },
                    {
                        "id": "isolatedetails",
                        "type": "standard",
                        "queryName": "isolateforcultureviewquery"
                    },
                    {
                        "id": "culturetests",
                        "type": "crafted",
                        "title": "@SpeCulC@",
                        "name": "directtestsgrid",
                        "queryName": "testlistforculturelite"
                    },
                    {
                        "id": "astdetails",
                        "type": "listview",
                        "title": "@AstRes@",
                        "listViewName": "ast"
                    },
                    {
                        "id": "attachments",
                        "type": "standard",
                        "queryName": "cultureattachmentsforcultureview",
                        "title": "@GenAtts@"
                    },
                    {
                        "id": "instrumentresults",
                        "type": "listview",
                        "title": "@InsIns@",
                        "listViewName": "cultureinstresults"
                    },
                    {
                        "id": "culturecomments",
                        "type": "listview",
                        "title": "@GenComE@",
                        "listViewName": "culturecomments"
                    }
                ]
            }
            """;

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}

