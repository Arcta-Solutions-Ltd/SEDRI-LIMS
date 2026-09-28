using System.Linq;

namespace arc.app.Exports.ExportFieldProcessors
{
    internal class HierarchyProcessor
    {
        public string GetHeader(string headerName)
        {
            var header = "";
            var headingList = headerName.Split(",");
            foreach (var heading in headingList)
            {
                header += header == "" ? heading.Trim() : "|" + heading.Trim();
            }
            return header;
        }

        public string GetLine(string headerName, string locationString)
        {
            var headingCount = headerName.Split(",").Count();
            var returnValue = "";

            int counter = 0;
            var locationList = locationString.Split(":");
            foreach (var location in locationList)
            {
                if (counter < headingCount)
                {
                    returnValue += returnValue == "" ? location.Trim() : "|" + location.Trim();
                }
                counter++;
            }
            while (counter < headingCount)
            {
                returnValue += returnValue == "" ? "" : "|";
                counter++;
            }
            return returnValue;
        } 
    }
}
