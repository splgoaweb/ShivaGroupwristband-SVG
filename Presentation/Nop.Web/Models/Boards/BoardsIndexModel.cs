using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Boards
{
    public partial class BoardsIndexModel : BaseNopModel
    {
        public BoardsIndexModel()
        {
            ForumGroups = new List<ForumGroupModel>();
        }

        public IList<ForumGroupModel> ForumGroups { get; set; }
    }
}