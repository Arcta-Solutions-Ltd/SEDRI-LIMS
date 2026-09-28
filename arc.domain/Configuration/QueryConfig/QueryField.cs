namespace arc.domain.Configuration.QueryConfig
{
    public class QueryField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string SourceTable { get; set; }
        public string KnownAs { get; set; }
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }

        /// <summary>
        /// Set on a joined table field that lives in that table's MoreData blob rather than a column of its
        /// own. The main table works this out from its column list, but a join has to be told, because a
        /// name that is unknown to the column list is far more often a column the list has not caught up
        /// with than a MoreData key.
        /// </summary>
        public bool FromMoreData { get; set; }
    }
}
