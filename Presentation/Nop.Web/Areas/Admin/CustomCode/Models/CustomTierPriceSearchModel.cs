using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.CustomCode.Models
{
    /// <summary>
    /// Represents a tier price search model
    /// </summary>
    public partial class CustomTierPriceSearchModel : BaseSearchModel
    {
        #region Properties

        public int CategoryId { get; set; }

        #endregion
    }
}