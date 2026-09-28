using arc.common.Models;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports.ExportFieldProcessors
{
    internal class CommentProcessor
    {
        private List<ExportComment> _fullCommentList;
        private List<ExportComment> _commentsToUse;
        private int _headerNumber;
        private bool _isSpecimen = false;

        public async Task InitialiseAsync(IExportRepository exportRepository, QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            _fullCommentList = await exportRepository.ExportCommentAsync(queryFilters, token);
        }

        public void GetComments(ExportProfileFieldModel profile, IJsonWholeStructureFieldsCollector jsonWholeStructureFieldsCollector)
        {
            //Get moredata items
            var moredata = profile.MoreData != null ? jsonWholeStructureFieldsCollector.GetStructure(profile.MoreData) : [];

            //Get comment types
            string[] commentTypeValues = null;
            var commentType = moredata.FirstOrDefault(p => p.Label.ToLower() == "commenttype");
            if(commentType != null)
            {
                commentTypeValues = commentType.Contents.Split(",");
            }

            //Get comment formats
            string[] commentFormatValues = null;
            var commentFormats = moredata.FirstOrDefault(p => p.Label.ToLower() == "commentformat");
            if (commentFormats != null)
            {
                commentFormatValues = commentFormats.Contents.Split(",");
            }

            //Filter comments based on if specimen comments or culture comments
            if (profile.FieldName.ToLower() == "specimencomments")
            {
                _commentsToUse = _fullCommentList.Where(p => p.CultureId == null).ToList();
                _isSpecimen = true;
            }
            else
            {
                _commentsToUse = _fullCommentList.Where(p => p.CultureId != null).ToList();
            }

            if (moredata.Count > 0)
            {
                //Filter comments based on comment types
                if (commentTypeValues != null)
                {
                    _commentsToUse = _commentsToUse.Where(p =>  commentTypeValues.Any(c => c.Equals(p.CommentTypeId.ToString()))).ToList();
                }
                else 
                {
                    _commentsToUse.Clear();
                }

                //Filter comments based on comment formats
                if (commentFormatValues != null)
                {
                    var containsBothFormats = commentFormatValues.Any(p => p.Equals("1246")) && commentFormatValues.Any(p => p.Equals("1247"));
                    if (!containsBothFormats)
                    {
                        switch (commentFormatValues[0])
                        {
                            case "1246":
                                _commentsToUse = _commentsToUse.Where(p => p.CannedComment == null).ToList();
                                break;
                            case "1247":
                                _commentsToUse = _commentsToUse.Where(p => p.CannedComment != null).ToList();
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    _commentsToUse.Clear();
                }
            }
            else
            {
                _commentsToUse.Clear();
            }

            //Set header number based on specimen comments or culture comments
            var coms = _commentsToUse.GroupBy(d => d.SpecimenId).Select(g => new { ParentId = g.Key, Count = g.Count() });
            if (!_isSpecimen)
            {
                coms = _commentsToUse.GroupBy(d => d.CultureId).Select(g => new { ParentId = g.Key, Count = g.Count() });
            }

            _headerNumber = coms.Any() ? coms.Max(p => p.Count) : 0;
        }

        public string GetHeader(string headerName)
        {
            var header = "";
            var iteration = 1;

            while(iteration <= _headerNumber)
            {
                header += header == "" ? headerName : "|" + headerName + iteration;
                iteration++;
            }

            return header;
        }

        public string GetLine(int parentId)
        {
            string newLine = null;
            var iteration = 1;

            var comments = _isSpecimen ? _commentsToUse.Where(p => p.SpecimenId == parentId) : _commentsToUse.Where(p => p.CultureId == parentId);

            foreach (var comment in comments)
            {
                var value = comment.Comment ?? comment.CannedComment;
                newLine = newLine == null ? value : newLine + "|" + value;
                iteration++;
            }

            while (iteration <= _headerNumber)
            {
                newLine = newLine == null ? "" : newLine + "|";
                iteration++;
            }

            return newLine;
        }
    }
}
