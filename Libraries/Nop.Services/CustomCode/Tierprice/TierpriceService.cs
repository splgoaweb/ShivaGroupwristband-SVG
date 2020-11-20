using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Services.CustomCode.Tierprice
{
    /// <summary>
    /// Category service
    /// </summary>
    public partial class TierpriceService : ITierpriceService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<TierPrice> _tierPriceRepository;
        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IRepository<ShoppingCartItem> _sciRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public TierpriceService(CatalogSettings catalogSettings,
            ICacheKeyService cacheKeyService,
            IRepository<TierPrice> tierPriceRepository,
            IEventPublisher eventPublisher,
            ICustomerService customerService,
            ICategoryService categoryService,
            IProductService productService,
            IRepository<ShoppingCartItem> sciRepository,
            ShoppingCartSettings shoppingCartSettings,
            IStaticCacheManager staticCacheManager,
            IWebHelper webHelper)
        {
            _catalogSettings = catalogSettings;
            _cacheKeyService = cacheKeyService;
            _tierPriceRepository = tierPriceRepository;
            _eventPublisher = eventPublisher;
            _customerService = customerService;
            _categoryService = categoryService;
            _productService = productService;
            _sciRepository = sciRepository;
            _shoppingCartSettings = shoppingCartSettings;
            _staticCacheManager = staticCacheManager;
            _webHelper = webHelper;
        }

        #endregion

        #region " Tier price "
        /// <summary>
        /// Gets a tier prices by category identifier
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        public virtual IList<TierPrice> GetTierPricesByCategory(int categoryId)
        {
            return _tierPriceRepository.Table.Where(tp => tp.CategoryId == categoryId)
                .ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(NopCatalogDefaults.CategoryTierPricesCacheKey, categoryId));
        }

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        public virtual void InsertTierPrice(TierPrice tierPrice)
        {
            if (tierPrice == null)
                throw new ArgumentNullException(nameof(tierPrice));

            _tierPriceRepository.Insert(tierPrice);

            //event notification
            _eventPublisher.EntityInserted(tierPrice);
        }

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        public virtual void UpdateTierPrice(TierPrice tierPrice)
        {
            if (tierPrice == null)
                throw new ArgumentNullException(nameof(tierPrice));

            _tierPriceRepository.Update(tierPrice);

            //event notification
            _eventPublisher.EntityUpdated(tierPrice);
        }

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        public virtual void DeleteTierPrice(TierPrice tierPrice)
        {
            if (tierPrice == null)
                throw new ArgumentNullException(nameof(tierPrice));

            _tierPriceRepository.Delete(tierPrice);

            //event notification
            _eventPublisher.EntityDeleted(tierPrice);
        }

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public virtual TierPrice GetTierPriceById(int tierPriceId)
        {
            if (tierPriceId == 0)
                return null;

            return _tierPriceRepository.ToCachedGetById(tierPriceId);
        }



        // custom code start

        /// <summary>
        /// Gets a category tier prices for customer
        /// </summary>
        /// <param name="category">category</param>
        /// <param name="customer">Customer</param>
        /// <param name="storeId">Store identifier</param>
        public virtual IList<TierPrice> GetCategoryTierPrices(Category category, Customer customer, int storeId)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category));

            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            //if (!(bool)category.EnableAggregation)
            //    return null;

            //get actual tier prices
            return GetTierPricesByCategory(category.Id)
                .OrderBy(price => price.Quantity)
                .FilterByStore(storeId)
                .FilterByCustomerRole(_catalogSettings.IgnoreAcl ? Array.Empty<int>() : _customerService.GetCustomerRoleIds(customer))
                .FilterByDate()
                .RemoveDuplicatedQuantities()
                .ToList();
        }
        #endregion

        #region " Get tierPrice for Finalprice "

        public virtual FinalTierPrice GetTierPricesModel(int productId, Customer customer, int storeId, int totalquantity = 0)
        {
            //return _tierPriceRepository.Table.Where(tp => tp.CategoryId == categoryId)
            //    .ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(NopCatalogDefaults.CategoryTierPricesCacheKey, categoryId));

            var categories = _categoryService.GetProductCategoriesByProductId(productId);
            var model = new FinalTierPrice();
            if (categories.Count > 0)
            {
                // model.CategoryId = categories.FirstOrDefault().CategoryId;
                var category = _categoryService.GetCategoryById(categories.FirstOrDefault().CategoryId);

                if (category == null)
                    return model;

                model.CategoryId = category.Id;
                model.EnableAggregation = category.EnableAggregation.HasValue ? true : false;
                model.ProductId = productId;

                var currentPageUrl = _webHelper.GetThisPageUrl(true);
                //if (currentPageUrl.Contains("ProductDetails_QtyAttributeChange"))
                if (currentPageUrl.Contains("ProductDetails_QtyAttributeChange") || currentPageUrl.Contains("productdetails_attributechange"))
                {
                    model.TotalQuantity = totalquantity;

                }
                else
                {
                    if (currentPageUrl.EndsWith("cart") || currentPageUrl.Contains("checkout") || currentPageUrl.Contains("wishlist"))
                    {
                        var productCategories = _categoryService.GetAllProductCategories();
                        var shoppingcartsitems = GetShoppingCart(customer, currentPageUrl.Contains("wishlist") ? ShoppingCartType.Wishlist : ShoppingCartType.ShoppingCart, storeId);
                        var all_categories = _categoryService.GetAllCategories();
                        var products = _productService.GetAllProduct();

                        var cartItems = from sci in shoppingcartsitems
                                        join p in products on sci.ProductId equals p.Id
                                        join pcm in productCategories on p.Id equals pcm.ProductId
                                        join c in all_categories on pcm.CategoryId equals c.Id
                                        where c.Id == category.Id
                                        select new
                                        {
                                            id = c.Id,
                                            enableAggregation = c.EnableAggregation.HasValue ? c.EnableAggregation.Value : false,
                                            quantity = sci.Quantity
                                        };

                        model.TotalQuantity = cartItems.Sum(x => x.quantity);
                    }
                }
                model.CategoryTierPrices = GetTierPricesByCategory(category.Id);

                return model;
            }
            else
                return model;
        }


        public virtual IList<ShoppingCartItem> GetShoppingCart(Customer customer, ShoppingCartType? shoppingCartType = null,
           int storeId = 0, int? productId = null, DateTime? createdFromUtc = null, DateTime? createdToUtc = null)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var items = _sciRepository.Table.Where(sci => sci.CustomerId == customer.Id);

            //filter by type
            if (shoppingCartType.HasValue)
                items = items.Where(item => item.ShoppingCartTypeId == (int)shoppingCartType.Value);

            //filter shopping cart items by store
            if (storeId > 0 && !_shoppingCartSettings.CartsSharedBetweenStores)
                items = items.Where(item => item.StoreId == storeId);

            //filter shopping cart items by product
            if (productId > 0)
                items = items.Where(item => item.ProductId == productId);

            //filter shopping cart items by date
            if (createdFromUtc.HasValue)
                items = items.Where(item => createdFromUtc.Value <= item.CreatedOnUtc);
            if (createdToUtc.HasValue)
                items = items.Where(item => createdToUtc.Value >= item.CreatedOnUtc);

            var key = _cacheKeyService.PrepareKeyForShortTermCache(NopOrderDefaults.ShoppingCartCacheKey, customer, shoppingCartType, storeId, productId, createdFromUtc, createdToUtc);

            return _staticCacheManager.Get(key, () => items.ToList());
        }

        /// <summary>
        /// Gets a preferred tier price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customer">Customer</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Tier price</returns>
        public virtual TierPrice GetPreferredTierPrice(int categoryId, Customer customer, int storeId, int quantity)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            if (category == null)
                //throw new ArgumentNullException(nameof(category));

                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

            var enableAggregation = category.EnableAggregation.HasValue ? category.EnableAggregation.Value : false;
            if (!enableAggregation)
                return null;

            //get the most suitable tier price based on the passed quantity
            return GetCategoryTierPrices(category, customer, storeId)?.LastOrDefault(price => quantity >= price.Quantity);
        }

        //public virtual FinalTierPrice GetTierPricesModelForTotalQuanity(int productId, int totalquantity = 0)
        //{
        //    //return _tierPriceRepository.Table.Where(tp => tp.CategoryId == categoryId)
        //    //    .ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(NopCatalogDefaults.CategoryTierPricesCacheKey, categoryId));

        //    var categories = _categoryService.GetProductCategoriesByProductId(productId);
        //    var model = new FinalTierPrice();
        //    if (categories.Count > 0)
        //    {
        //        // model.CategoryId = categories.FirstOrDefault().CategoryId;
        //        var category = _categoryService.GetCategoryById(categories.FirstOrDefault().CategoryId);

        //        if (category == null)
        //            return model;

        //        model.CategoryId = category.Id;
        //        model.EnableAggregation = category.EnableAggregation.HasValue ? true : false;
        //        model.ProductId = productId;

        //        model.TotalQuantity = totalquantity;
        //        model.CategoryTierPrices = GetTierPricesByCategory(category.Id);

        //        return model;
        //    }
        //    else
        //        return model;
        //}
        #endregion

    }
}