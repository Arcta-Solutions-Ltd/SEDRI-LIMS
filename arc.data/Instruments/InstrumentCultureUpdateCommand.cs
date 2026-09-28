using arc.app.Common;
using arc.common.Models.Instruments;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.Instruments
{
    /// <summary>
    /// Represents a command to update instrument culture data.
    /// </summary>
    internal class InstrumentCultureUpdateCommand : ICommandWithTypeReturningInteger<ResponseModel>
    {
        /// <summary>
        /// Executes the command asynchronously to update instrument culture data.
        /// </summary>
        /// <param name="connect">The Npgsql connection to use for the update.</param>
        /// <param name="instrument">The response model containing the instrument data.</param>
        /// <param name="logWriter">The log writer for logging information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ResponseModel instrument, ILogWriter logWriter)
        {
            var growth = int.Parse(instrument.Growth);
            growth = growth == 0 ? int.Parse(instrument.DefaultGrowth): growth;

            // Update the culture data with specimen quantity and organism ID.
            var sql = @"update culture set growthid = @growth, specimenorganismid = @organismId 
                    where Id = @CultureId";

            await connect.ExecuteAsync(sql, new { growth, instrument.OrganismId, instrument.CultureId });

            await connect.ExecuteAsync("delete from ast where cultureid = @CultureId", new { instrument.CultureId });

            // Insert data for each antibiotic in the response model.
            foreach (var antibiotic in instrument.Antibiotics)
            {
                sql = @"insert into Ast(cultureid, testtype, entrytype, testmethodid, antibioticid, measurement, susceptibilityid, displayonreport,
                    miccomparison, appliedbreakpointid, guidelinesid, lastmodifieddate)
                    values(@CultureId, @TestType, @EntryType, 680, @AntibioticId, @Measurement, @SusceptibilityId, @DisplayOnReport,
                    @MicComparison, 0, 59, now()) returning id";

                var susceptibilityId = int.Parse(antibiotic.Susceptibility);
                var displayOnReport = string.IsNullOrWhiteSpace(antibiotic.PrintOnReport)
                    ? "Yes"
                    : antibiotic.PrintOnReport.Trim();
                if (!string.Equals(displayOnReport, "Yes", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(displayOnReport, "No", StringComparison.OrdinalIgnoreCase))
                    displayOnReport = "Yes";

                // Execute the insert statement and get the new AST ID.
                var astId = await connect.QueryFirstAsync(sql, new
                {
                    cultureid = instrument.CultureId,
                    testtype = "",
                    entrytype = "",
                    antibioticid = antibiotic.AntibioticId,
                    measurement = string.IsNullOrEmpty(antibiotic.MicValue) ? 0 : decimal.Parse(antibiotic.MicValue),
                    susceptibilityid = susceptibilityId,
                    miccomparison = antibiotic.MicSign,
                    displayOnReport
                });

            }

            if (instrument.ResistanceMechanisms != null)
            {
                await connect.ExecuteAsync("delete from cultureresistancemechanism where cultureid = @CultureId", new { instrument.CultureId });

                foreach (var m in instrument.ResistanceMechanisms)
                {
                    await connect.ExecuteAsync(
                        @"insert into cultureresistancemechanism (cultureid, drugfamily, phenotype, lastmodifieddate)
                          values (@CultureId, @DrugFamily, @PhenoType, now())",
                        new
                        {
                            instrument.CultureId,
                            DrugFamily = m?.DrugFamily ?? "",
                            PhenoType = m?.PhenoType ?? ""
                        });
                }
            }

            return 0;
        }
    }

}
