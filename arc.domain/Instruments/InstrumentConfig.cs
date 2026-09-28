using System.Collections.Generic;

namespace arc.domain.Instruments
{
    /// <summary>
    /// Root document for instrument profile configuration (list of <see cref="SingleInstrumentConfig"/>).
    /// </summary>
    public class InstrumentConfig
    {
        public List<SingleInstrumentConfig> Instruments { get; set; } = new List<SingleInstrumentConfig>();

        /// <summary>
        /// Appends a new instrument profile entry.
        /// </summary>
        public void AddInstrument(SingleInstrumentConfig newInstrument)
        {
            Instruments.Add(newInstrument);
        }

        /// <summary>
        /// Copies all fields from <paramref name="newInstrument"/> onto the existing entry with the same <see cref="SingleInstrumentConfig.InstrumentName"/>.
        /// </summary>
        public void ReplaceInstrument(SingleInstrumentConfig newInstrument)
        {
            foreach (var instrument in Instruments)
            {
                if (instrument.InstrumentName == newInstrument.InstrumentName)
                {
                    instrument.LaboratoryId = newInstrument.LaboratoryId;
                    instrument.SpecimenTypeId = newInstrument.SpecimenTypeId;
                    instrument.CultureTestId = newInstrument.CultureTestId;
                    instrument.CultureTypeId = newInstrument.CultureTypeId;
                    instrument.ConfigListId = newInstrument.ConfigListId;
                    instrument.AllowAstOverwrite = newInstrument.AllowAstOverwrite;
                    instrument.DefaultGrowth = newInstrument.DefaultGrowth;
                    instrument.NeedsApproval = newInstrument.NeedsApproval;
                    instrument.AllowIdOverwrite = newInstrument.AllowIdOverwrite;
                    instrument.AntibioticGroupId = newInstrument.AntibioticGroupId;
                    instrument.DirectTestId = newInstrument.DirectTestId;
                    instrument.IgnoreUnrecognisedAntibiotics = newInstrument.IgnoreUnrecognisedAntibiotics;
                    instrument.InstrumentMachineId = newInstrument.InstrumentMachineId;
                    instrument.InstrumentName = newInstrument.InstrumentName;
                    instrument.InterfaceTypeId = newInstrument.InterfaceTypeId;
                    instrument.IsEnabled = newInstrument.IsEnabled;
                    instrument.OrganismGroupId = newInstrument.OrganismGroupId;
                    instrument.ExportProfileId = newInstrument.ExportProfileId;
                    instrument.InterfaceCriteriaAndOr = newInstrument.InterfaceCriteriaAndOr;
                    instrument.InterfaceCriteria = newInstrument.InterfaceCriteria;
                }
            };
        }

        /// <summary>
        /// Removes the given profile entry from the list (reference equality).
        /// </summary>
        public void RemoveInstrument(SingleInstrumentConfig newInstrument)
        {
            Instruments.Remove(newInstrument);
        }
    }
}
