using arc.app.Common;

namespace arc.app.Config.Workflows;

/// <summary>
/// Defines the workflow for handling IQC (Internal Quality Control) tests.
/// Implements the <see cref="IDefinition"/> interface.
/// </summary>
public class IqcTestWorkflow : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the IQC test workflow definition.
    /// </summary>
    /// <returns>
    /// A JSON string defining the workflow, including its name, description, associated table, field, steps, 
    /// and entry conditions.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        Name: 'IqcTestWorkflow', 
                        Description: 'Iqc Tests Workflow',
                        Table: 'iqctests',
                        Field: 'stateid',
                        Steps: [
                            { 
                                entrystate: '1067,1068',
                                event: 'runiqctest'
                            },
                            { 
                                entrystate: '1067,1068',
                                exitstate: { default: '1069' },
                                event: 'markiqctestcomplete'
                            },
                            { 
                                entrystate: '1067,1068',
                                event: 'editiqctestqcorganisms'
                            },
                            { 
                                entrystate: '1067,1068',
                                event: 'deleteiqcresult'
                            },
                            { 
                                entrystate: '1067,1068',
                                event: 'editiqcresult'
                            },
                        ],
                        EntryConditions: []
                    }";
    }
}

