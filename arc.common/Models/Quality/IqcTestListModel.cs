namespace arc.common.Models.Quality
{
    public class IqcTestListModel
    {
        public int Id { get; set; }
        public string AccessionNumber { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string CreatedDate { get; set; }
        public string CompletedDate { get; set; }
    }
}
