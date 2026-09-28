using arc.app.Common;
using arc.common.Models.Specimen;
using Newtonsoft.Json;
using System;

namespace arc.app.Specimen
{
    internal class ReceivedSpecimenRequestValidator : ISpecialValidator
    {
        private readonly string _message;

        public ReceivedSpecimenRequestValidator(string message)
        {
            _message = message;
        }

        public string ValidateMessage()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var data = JsonConvert.DeserializeObject<CreateSpecimenEventModel>(_message, settings);

            var collectionDate = Convert.ToDateTime(data.CollectionDate);
            var receivedDate = Convert.ToDateTime(data.ReceivedDate);

            if (receivedDate.CompareTo(collectionDate) < 0)
            {
                return "@SpeVal@";
            }
            return "";
        }
    }
}
