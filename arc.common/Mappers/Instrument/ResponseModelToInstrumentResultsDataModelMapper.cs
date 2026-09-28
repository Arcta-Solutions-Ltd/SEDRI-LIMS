using arc.common.Models.Instruments;
using arc.common.Utils;
using arc.data.model.Instruments;

namespace arc.common.Mappers.Instrument
{
    /// <summary>
    /// Maps the ResponseModel onto the InstrumentResultsDataModel.
    /// </summary>
    public class ResponseModelToInstrumentResultsDataModelMapper : IMapType<ResponseModel, InstrumentResultsDataModel>
    {
        /// <summary>
        /// Maps a <see cref="ResponseModel"/> object to a <see cref="InstrumentResultsDataModel"/> object.
        /// </summary>
        /// <param name="source">The <see cref="ResponseModel"/> object to map.</param>
        /// <returns>The mapped <see cref="InstrumentResultsDataModel"/> object.</returns>
        public InstrumentResultsDataModel Map(ResponseModel source)
        {
            var result = new InstrumentResultsDataModel { 
                InstrumentProfile = source.ProfileName,
                Barcode = source.Identifier,
                CultureId = source.CultureId,
                SpecimenId = source.SpecimenId,
                RawResult = ArcJson.Serialize(source)
            };

            return result;
        }
    }


}
