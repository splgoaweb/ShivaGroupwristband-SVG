using Nop.Core.Domain.Catalog;
using Nop.Web.Areas.Admin.CustomCode.Models;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Web.Areas.Admin.CustomCode.Factories
{
    /// <summary>
    /// Represents the TierPrice model factory
    /// </summary>
    public partial interface ITirePriceModelFactory
    {
        /// <summary>
        /// Prepare paged tier price list model
        /// </summary>
        /// <param name="searchModel">Tier price search model</param>
        /// <param name="category">category</param>
        /// <returns>Tier price list model</returns>
        TierPriceListModel PrepareTierPriceListModel(CustomTierPriceSearchModel searchModel, Category category);
        /// <summary>
        /// Prepare tier price model
        /// </summary>
        /// <param name="model">Tier price model</param>
        /// <param name="category">category</param>
        /// <param name="tierPrice">Tier price</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Tier price model</returns>
        TierPriceModel PrepareTierPriceModel(TierPriceModel model,
            Category category, TierPrice tierPrice, bool excludeProperties = false);

    }
}