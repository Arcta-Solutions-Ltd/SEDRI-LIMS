using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Add Workflow" page.
/// </summary>
internal class AddWorkflowPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the structure and layout of the "Add Workflow" page.
    /// It includes metadata such as the page's name, title, and descriptive text. 
    /// The page is divided into columns, each containing form groups with specific fields. 
    /// Fields include a combobox for selecting workflows, along with single-line text fields 
    /// for entering a name and description. Required fields and placeholders are defined to 
    /// guide user input.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its structure, 
    /// columns, form groups, and fields.
    /// </returns>
    public string Get()
    {
        return """
            {
              "name": "addworkflowpage",
              "pageTitle": "@ConAddAC@",
              "text": "@ConAddAD@",
              "columns": [
                {
                  "key": "col1",
                  "formGroups": [
                    {
                      "key": "fg1",
                      "fields": [
                        {
                          "id": "WorkflowCloneId",
                          "type": "combobox",
                          "label": "@ConWorClo@",
                          "required": true,
                          "placeholder": "@ConSelF@",
                          "optionsName": "WorkflowList",
                          "tab": true,
                          "dynamic": true
                        },
                        {
                          "id": "Name",
                          "type": "singleline",
                          "label": "@GenNam@",
                          "required": true
                        },
                        {
                          "id": "Description",
                          "type": "singleline",
                          "label": "@GenDes@",
                          "required": true
                        }
                      ]
                    }
                  ]
                }
              ]
            }
            """;
    }
}

