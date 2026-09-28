using arc.app.Common;

namespace arc.app.Config.Workflows;

/// <summary>
/// Defines the workflow for handling Unapproved Reports.
/// Controls button visibility based on approval status.
/// Implements the <see cref="IDefinition"/> interface.
/// </summary>
public class UnapprovedReportWorkflow : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the Unapproved Report workflow definition.
    /// </summary>
    /// <returns>
    /// A JSON string defining the workflow, including its name, description, associated table, field, steps,
    /// and entry conditions.
    /// </returns>
    public string Get()
    {
        return @"{
                        Name: 'UnapprovedReportWorkflow',
                        Description: 'Unapproved Reports Workflow',
                        Table: 'reporthistory',
                        Field: 'stateid',
                        Steps: [
                            {
                                entrystate: '122,124',
                                event: 'approvereportevent'
                            },
                            {
                                entrystate: '122,123',
                                event: 'unapprovereportevent'
                            }
                        ],
                        EntryConditions: []
                    }";
    }
}
