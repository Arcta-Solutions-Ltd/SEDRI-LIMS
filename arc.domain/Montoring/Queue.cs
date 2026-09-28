using System;

namespace arc.domain.Montoring
{
    public class Queue
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public DateTime Added { get; set; }
        public int TopicId { get; set; }
        public int EventId { get; set; }
        public int EventStatusId { get; set; }
        public int SpecimenId { get; set; }
        public int PatientId { get; set; }
        public string Error { get; set; }
        public string Hash { get; set; }
    }
}

