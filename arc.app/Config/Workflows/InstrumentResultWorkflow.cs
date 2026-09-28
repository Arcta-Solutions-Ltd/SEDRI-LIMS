using arc.app.Common;

namespace arc.app.Config.Workflows;
/// <summary>
/// Defines the workflow for handling instrument results.
/// Implements the <see cref="IDefinition"/> interface.
/// </summary>
internal class InstrumentResultWorkflow : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the instrument result workflow definition.
    /// </summary>
    /// <returns>
    /// A JSON string defining the workflow, including its name, description, associated table, field, steps, and entry conditions.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        Name: 'InstrumentResultWorkflow', 
                        Description: 'Instrument Result Workflow',
                        Table: 'instrumentresults',
                        Field: 'stateid',
                        Steps: [
                            { 
                                entrystate: '883',
                                event: 'acceptinstrumentresults'
                            },
                            { 
                                entrystate: '883',
                                event: 'deleteinstrumentresults'
                            }
                        ],
                        EntryConditions: []
                    }";
    }
}

