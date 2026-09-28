using System;
using System.Collections.Generic;

namespace arc.common.Models.AST
{
    public class ASTCultureModel
    {
        public int SpecimenId { get; set; }
        public int SpecimenTypeId { get; set; }
        public int CultureTypeId { get; set; }
        public int LaboratoryId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public int OrganismId { get; set; }
        public List<CultureTestStatusModel> CultureTests { get; set; }
        public string TestPattern { get; set; }
        public int TestPatternId { get; set; }
        //public string DiskTestUsePattern { get; set; }
        //public string StripTestUsePattern { get; set; }
        public string ASTCommentOne { get; set; }
        public int ASTCommentOneId { get; set; }
        public string ASTCommentTwo { get; set; }
        public int ASTCommentTwoId { get; set; }
        public string ASTAdditionalNotes { get; set; }
        /// <summary>
        /// AST recorded date (yyyy-mm-dd). Loaded from Culture.ASTCompletedDate when the AST form opens.
        /// </summary>
        public string CompletedDate { get; set; }
        /// <summary>
        /// AST recorded time (HH:MM). Loaded from Culture.ASTCompletedTime when the AST form opens.
        /// </summary>
        public string CompletedTime { get; set; }
    }
}
