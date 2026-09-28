using arc.app.Config.Views.DiaryViews;
using arc.common;
using arc.common.Models.Monitoring;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Monitoring
{
    public class QueueToDiaryMapper : IMap
    {
        private readonly IDiaryFactory _diaryFactory;
        private string _diaryName;

        public QueueToDiaryMapper (string diaryName, IDiaryFactory diaryFactory)
        {
            _diaryFactory = diaryFactory;
            _diaryName = diaryName;
        }

        public string Map(string source)
        {
            //Make into list of queue entries

            var queueItems = JsonConvert.DeserializeObject<List<DiaryQueryModel>>(source);

            //Get the diary definition

            var diary = _diaryFactory.Create(_diaryName);

            // For each queue entry translate to diary definition row 

            var diaryEntryList = new List<DiaryViewModel>();
            foreach(var item in queueItems)
            {
                //var matchingRecord = diary.Text.First(d => int.Parse(d.Id) == item.EventId);
                var matchingRecords = diary.Text.Where(d => int.Parse(d.Id) == item.EventId);
                if (matchingRecords.Count() > 0)
                {
                    var matchingRecord = matchingRecords.First();
                    var newItem = new DiaryViewModel { Icon = matchingRecord.Icon, Id = item.Id, Timestamp = item.Added };

                    var linkStartPosition = matchingRecord.Text.IndexOf("<l>");
                    if (linkStartPosition != -1)
                    {
                        var linkEndPosition = matchingRecord.Text.IndexOf("</l>");
                        newItem.StartText = FormatReturnString(matchingRecord.Text.Substring(0, linkStartPosition), item);
                        newItem.LinkText = FormatReturnString(matchingRecord.Text.Substring(linkStartPosition+3, linkEndPosition - linkStartPosition-3), item);
                        newItem.EndText = FormatReturnString(matchingRecord.Text.Substring(linkEndPosition+4, matchingRecord.Text.Length - linkEndPosition - 4), item);
                    } else
                    {
                        newItem.StartText = FormatReturnString(matchingRecord.Text, item);
                    }

                    diaryEntryList.Add(newItem);
                }
            }

            // Transform back to a json string

            return JsonConvert.SerializeObject(diaryEntryList);

        }

        private string FormatReturnString(string stringToFormat, DiaryQueryModel item)
        {
            return stringToFormat.Replace("<user>", item.Username).Replace("<accessionnumber>", item.AccessionNumber).Replace("<state>", item.State);
        }
    }
}
