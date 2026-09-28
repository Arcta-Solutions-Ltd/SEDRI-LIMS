using arc.common.ExtensionMethods;
using arc.common.Models.Instruments;
using arc.data.model.Culture;

namespace arc.common.Mappers.Instrument
{
    /// <summary>
    /// Maps the ResponseModel onto the CultureDataModel.
    /// </summary>
    public class ResponseModelToCultureDataModelMapper : IMapType<ResponseModel, CultureDataModel>
    {
        /// <summary>
        /// Maps a <see cref="ResponseModel"/> object to a <see cref="CultureDataModel"/> object.
        /// </summary>
        /// <param name="source">The <see cref="ResponseModel"/> object to map.</param>
        /// <returns>The mapped <see cref="CultureDataModel"/> object.</returns>
        public CultureDataModel Map(ResponseModel source)
        {
            var result = source.Map<CultureDataModel>();
            result.Id = source.CultureId;
            return result;
        }
    }
}
