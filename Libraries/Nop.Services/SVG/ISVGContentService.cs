using Nop.Core.Domain.SVG;

namespace Nop.Services.SVG
{
    public partial interface ISVGContentService
    {
        #region SVGContent

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="productId">The Product identifier</param>
        /// <returns>Product</returns>
        SVGContent GetContentByProductId(int productId);
        #endregion
    }
}
