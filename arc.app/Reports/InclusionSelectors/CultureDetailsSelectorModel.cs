using System.Collections.Generic;

namespace arc.app.Reports.InclusionSelectors
{
    public class CultureDetailsSelectorModel
    {
        public string Id { get; set; }
        public List<CultureDetailsTest> CultureTestGrid { get; set; }
        public List<CultureDetailsAst> AstGrid { get; set; }
        public List<CultureComments> CultureCommentGrid { get; set; }
    }

    public class CultureDetailsTest
    {
        public string Id { get; set; }
        public string CultureTest { get; set; }
        public string PrintOnReport { get; set; }
    }

    /// <summary>
    /// AST row in the culture print selector grid. Parent rows use <see cref="SpecialConsiderationId"/> 0 or 973;
    /// special consideration rows use the special type list item id for id-based save to <c>specialastrow</c>.
    /// </summary>
    public class CultureDetailsAst
    {
        /// <summary>Parent AST row id (<c>AST.id</c>).</summary>
        public int Id { get; set; }

        /// <summary>Special consideration list item id; 0 or 973 for parent AST rows.</summary>
        public int SpecialConsiderationId { get; set; }

        public string AstTest { get; set; }
        public string PrintOnReport { get; set; }
    }

    public class CultureComments
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public string CommentType { get; set; }
        public string PrintOnReport { get; set; }
        public string Username { get; set; }
    }
}
