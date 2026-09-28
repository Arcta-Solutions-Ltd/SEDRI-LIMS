using arc.app.Common;

using arc.domain.Configuration.QueryFiltersConfig;

using System.Linq;

using System.Threading.Tasks;



namespace arc.app.Patient

{

    /// <summary>

    /// Post-parameter-mapping handler for PatientSearch that logs age range and date of birth parameters.

    /// </summary>

    public class PatientSearchPostMappingHandler : IPostParameterMappingHandler

    {

        private readonly ILogWriter _logWriter;



        /// <summary>

        /// Initializes a new instance of the <see cref="PatientSearchPostMappingHandler"/> class.

        /// </summary>

        /// <param name="logWriter">The log writer.</param>

        public PatientSearchPostMappingHandler(ILogWriter logWriter)

        {

            _logWriter = logWriter;

        }



        /// <inheritdoc />

        public Task HandleAsync(string queryName, QueryFilterConfig queryFilters)

        {

            if (queryFilters?.Parameters == null)

                return Task.CompletedTask;



            var ageParamList = queryFilters.Parameters

                .Where(p => p.Key != null && (p.Key.IndexOf("AgeFrom", System.StringComparison.OrdinalIgnoreCase) >= 0 || p.Key.IndexOf("AgeTo", System.StringComparison.OrdinalIgnoreCase) >= 0))

                .ToList();



            if (ageParamList.Count == 0)

            {

                _logWriter.LogInfo("PatientSearch age params after mapping: none", nameof(PatientSearchPostMappingHandler), nameof(HandleAsync));

            }

            else

            {

                var ageParams = string.Join(", ", ageParamList.Select(p => $"{p.Key}={p.Value}"));

                _logWriter.LogInfo($"PatientSearch age params after mapping ({ageParamList.Count} keys): {ageParams}", nameof(PatientSearchPostMappingHandler), nameof(HandleAsync));

            }



            var startDate = queryFilters.Parameters.FirstOrDefault(p => p.Key != null && p.Key.Equals("startdate", System.StringComparison.OrdinalIgnoreCase))?.Value;

            var endDate = queryFilters.Parameters.FirstOrDefault(p => p.Key != null && p.Key.Equals("enddate", System.StringComparison.OrdinalIgnoreCase))?.Value;



            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))

            {

                _logWriter.LogInfo("PatientSearch DOB params after mapping: none", nameof(PatientSearchPostMappingHandler), nameof(HandleAsync));

            }

            else

            {

                _logWriter.LogInfo($"PatientSearch DOB params after mapping: startdate={startDate ?? ""}, enddate={endDate ?? ""}", nameof(PatientSearchPostMappingHandler), nameof(HandleAsync));

            }



            return Task.CompletedTask;

        }

    }

}


