using arc.domain.Configuration.QueryFiltersConfig;

namespace arc.data.Utils;

internal class DateSelection
{
    public string OrderBy { get; private set; }
    public string GroupBy { get; private set; }
    public string TableSelection { get; private set; }
    public string Select { get; private set; }

    public void IncorporateDateInterval(QueryFilterConfig queryFilters, string groupByField, string valueField)
    {
        _ = queryFilters.TryGetStringValue("dateinterval", out var dateInterval, ""); ;

        switch (dateInterval)
        {
            case "480":
                Select = @"extract(month from collectiondate) as month, extract(year from collectiondate) as year";
                GroupBy = groupByField + ", year, month";
                TableSelection = valueField + ", tv.month, tv.year, tv.number, TO_CHAR(TO_DATE (tv.month::text, 'MM'), 'Month') AS MonthName";
                OrderBy = "tv.month, tv.year";
                break;
            case "481":
                Select = @"extract(year from collectiondate) as year";
                GroupBy = groupByField + ", year";
                TableSelection = valueField + ", tv.year, tv.number";
                OrderBy = "tv.year";
                break;
            default:
                Select = @"extract(day from collectiondate) as day, extract(month from collectiondate) as month, extract(year from collectiondate) as year";
                GroupBy = groupByField + ", year, month, day";
                TableSelection = valueField + ", tv.day, tv.month, tv.year, tv.number, TO_CHAR(TO_DATE (tv.month::text, 'MM'), 'Month') AS MonthName";
                OrderBy = "tv.day, tv.month, tv.year";
                break;
        }
    }
}
