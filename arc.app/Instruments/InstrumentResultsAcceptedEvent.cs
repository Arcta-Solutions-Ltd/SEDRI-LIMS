//using arc.app.Common;
//using arc.app.Specimen;
//using arc.common;
//using arc.common.Models.Instruments;
//using arc.domain.Configuration.EventsConfig;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.app.Instruments
//{
//    public class AcceptInstrumentResultsEvent : IRun
//    {
//        private readonly IInstrumentRepository _instrumentResultsRepository;
//        private readonly ICultureRepository _cultureRepository;

//        public AcceptInstrumentResultsEvent(IInstrumentRepository instrumentResultsRepository, ICultureRepository cultureRepository)
//        {
//            _instrumentResultsRepository = instrumentResultsRepository;
//            _cultureRepository = cultureRepository;
//        }

//        public async Task<int> Run(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
//        {
//            var savedResults = await _instrumentResultsRepository.GetSingleEntry(command.Id);
//            var commonFields = JsonConvert.DeserializeObject<InstrumentCommonModel>(savedResults.RawData);

//            switch (commonFields.TestType)
//            {
//                case "BC":
//                    var bloodCultureResult = JsonConvert.DeserializeObject<BloodCultureResultsModel>(savedResults.RawData);
//                    if (await _cultureRepository.UpdateCultureResult(bloodCultureResult) != 0)
//                    {
//                        await _instrumentResultsRepository.Delete(command.Id);
//                    }
//                    break;
//                default:
//                    break;
//            }

//            return await Task.FromResult(0);
//        }
//    }
//}
