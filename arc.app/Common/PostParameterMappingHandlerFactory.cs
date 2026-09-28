using System;
using arc.app.Patient;

namespace arc.app.Common
{
    /// <summary>
    /// Factory that returns post-parameter-mapping handlers by query name.
    /// </summary>
    public class PostParameterMappingHandlerFactory : IPostParameterMappingHandlerFactory
    {
        private readonly PatientSearchPostMappingHandler _patientSearchHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostParameterMappingHandlerFactory"/> class.
        /// </summary>
        /// <param name="patientSearchHandler">The PatientSearch post-mapping handler.</param>
        public PostParameterMappingHandlerFactory(PatientSearchPostMappingHandler patientSearchHandler)
        {
            _patientSearchHandler = patientSearchHandler;
        }

        /// <inheritdoc />
        public IPostParameterMappingHandler GetHandler(string queryName)
        {
            if (string.IsNullOrEmpty(queryName))
                return null;

            if (string.Equals(queryName, "PatientSearch", StringComparison.OrdinalIgnoreCase))
                return _patientSearchHandler;

            return null;
        }
    }
}
