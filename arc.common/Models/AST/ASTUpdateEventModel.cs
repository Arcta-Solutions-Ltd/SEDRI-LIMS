using System;
using System.Collections.Generic;

namespace arc.common.Models.AST
{
    public class ASTUpdateEventModel
    {
        public int CultureId { get; set; }
        public string BL { get; set; }
        public int BLId { get; set; }
        public string ESBL { get; set; }
        public int ESBLId { get; set; }
        public string Carbapenemase { get; set; }
        public int CarbapenemaseId { get; set; }
        public string TestPattern { get; set; }
        public int TestPatternId { get; set; }
        public string DiskTestUsePattern { get; set; }
        public string StripTestUsePattern { get; set; }
        public string ASTCommentOne { get; set; }
        public int ASTCommentOneId { get; set; }
        public string ASTCommentTwo { get; set; }
        public int ASTCommentTwoId { get; set; }
        public string ASTAdditionalNotes { get; set; }
        public string CompletedDate { get; set; }
        public string CompletedTime { get; set; }
        public string DeleteBlankRows { get; set; }
        public List<ASTModel> ASTResults { get; set; }

        /// <summary>
        /// Optional snapshot of manual rows suppressed from <see cref="ASTResults"/> by applied expert rules; stored in
        /// <c>cultureastexpertruleevalcontext</c> for reload and expert evaluation context.
        /// </summary>
        public ExpertRuleEvalContextModel? ExpertRuleEvalContext { get; set; }
    }
}
