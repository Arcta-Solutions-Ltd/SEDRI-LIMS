using arc.app.Common;

namespace arc.app.Config.Workflows;
/// <summary>
/// Defines the workflow for handling instrument errors.
/// Implements the <see cref="IDefinition"/> interface.
/// </summary>
public class InstrumentErrorWorkflow : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the instrument error workflow definition.
    /// </summary>
    /// <returns>
    /// A JSON string defining the workflow, including its name, description, associated table, field, steps, and entry conditions.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        Name: 'InstrumentErrorWorkflow', 
                        Description: 'Instrument Error Workflow',
                        Table: 'instrumenterrors',
                        Field: 'stateid',
                        Steps: [
                            { 
                                entrystate: '19',
                                event: 'replayinstrumenterror'
                            }
                        ],
                        EntryConditions: []
                    }";
    }
}

