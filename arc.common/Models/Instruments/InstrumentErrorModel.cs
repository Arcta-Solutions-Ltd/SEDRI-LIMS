namespace arc.common.Models.Instruments
{
    public class InstrumentErrorModel
    {
        public string ProfileName { get; set; } = "";
        public string Message { get; set; } = "";
        public string Description { get; set; } = "";
        public int InstrumentResultId { get; set; } = 0;
        public int DirectionId { get; set; } = 0;

        /// <summary>
        /// List item id for InstrumentErrorStatus (e.g. Failed = 11). When 0, insert defaults to Failed (11).
        /// </summary>
        public int ErrorStatusId { get; set; }
    }
}
