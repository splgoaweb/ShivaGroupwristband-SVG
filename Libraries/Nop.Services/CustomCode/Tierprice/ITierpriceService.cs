using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using System;
using System.Collections.Generic;

namespace Nop.Services.CustomCode.Tierprice
{
    /// <summary>
    /// Category service interface
    /// </summary>
    public partial interface ITierpriceService
    {

        #region " Tier price "
        /// <summary>
        /// Gets a tier prices by category identifier
        /// </summary>
        /// <param name="productId">category identifier</param>
        IList<TierPrice> GetTierPricesByCategory(int categoryId);

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void InsertTierPrice(TierPrice tierPrice);

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void UpdateTierPrice(TierPrice tierPrice);

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void DeleteTierPrice(TierPrice tierPrice);

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        /// <returns>Tier price</returns>
        TierPrice GetTierPriceById(int tierPriceId);

        /// <summary>
        /// Gets a category tier prices for customer
        /// </summary>
        /// <param name="category">category</param>
        /// <param name="customer">Customer</param>
        /// <param name="storeId">Store identifier</param>
        IList<TierPrice> GetCategoryTierPrices(Category category, Customer customer, int storeId);

        FinalTierPrice GetTierPricesModel(int productId, Customer customer, int storeId, int totalquantity = 0);
        IList<ShoppingCartItem> GetShoppingCart(Customer customer, ShoppingCartType? shoppingCartType = null,
           int storeId = 0, int? productId = null, DateTime? createdFromUtc = null, DateTime? createdToUtc = null);

        TierPrice GetPreferredTierPrice(int categoryId, Customer customer, int storeId, int quantity);

        //FinalTierPrice GetTierPricesModelForTotalQuanity(int productId, int totalquantity = 0);
        #endregion

    }
}