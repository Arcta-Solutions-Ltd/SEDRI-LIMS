using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Provides the configuration definition for the "Edit Table" page,
    /// including layout, fields, and metadata used in dynamic UI rendering.
    /// </summary>
    internal class EditTablePageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON string that defines the structure and content of the "Edit Table" page.
        /// The Description field is presented to the user with the label "Name" (<c>@GenNam@</c>);
        /// only the Description column is updated when saving — the Name column in the database
        /// is not changed during an edit operation.
        /// </summary>
        /// <returns>
        /// A JSON string representing the page configuration, including:
        /// - Page name and title
        /// - Column layout with field widths
        /// - A single form field for Description (labelled as Name)
        /// </returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'edittablepage',
                            pageTitle: '@TabEdiD@',
                            text: '@TabEdiE@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [                                               
                                                { id: 'Description', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@TabEntB@' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
