using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.CustomCode.Tierprice;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.CustomCode.Models;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Linq;


namespace Nop.Web.Areas.Admin.CustomCode.Factories
{
    /// <summary>
    /// Represents the category model factory implementation
    /// </summary>
    public partial class TirePriceModelFactory : ITirePriceModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly ILocalizationService _localizationService;
        private readonly ITierpriceService _tierpriceService;


        #endregion

        #region Ctor

        public TirePriceModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            ICustomerService customerService,
            IStoreService storeService,
            IProductService productService,
            ILocalizationService localizationService,
            ITierpriceService tierpriceService)
        {
            _customerService = customerService;
            _storeService = storeService;
            _productService = productService;
            _localizationService = localizationService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _tierpriceService = tierpriceService;
        }

        /// <summary>
        /// Prepare paged tier price list model
        /// </summary>
        /// <param name="searchModel">Tier price search model</param>
        /// <param name="product">Product</param>
        /// <returns>Tier price list model</returns>
        public virtual TierPriceListModel PrepareTierPriceListModel(CustomTierPriceSearchModel searchModel, Category category)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            //get tier prices
            var tierPrices = _tierpriceService.GetTierPricesByCategory(category.Id)
                .OrderBy(price => price.StoreId).ThenBy(price => price.Quantity).ThenBy(price => price.CustomerRoleId)
                .ToList().ToPagedList(searchModel);

            //prepare grid model
            var model = new TierPriceListModel().PrepareToGrid(searchModel, tierPrices, () =>
            {
                return tierPrices.Select(price =>
                {
                    //fill in model values from the entity
                    var tierPriceModel = price.ToModel<TierPriceModel>();

                    //fill in additional values (not existing in the entity)   
                    tierPriceModel.Store = price.StoreId > 0
                        ? (_storeService.GetStoreById(price.StoreId)?.Name ?? "Deleted")
                        : _localizationService.GetResource("Admin.Catalog.Products.TierPrices.Fields.Store.All");
                    tierPriceModel.CustomerRoleId = price.CustomerRoleId ?? 0;
                    tierPriceModel.CustomerRole = price.CustomerRoleId.HasValue
                        ? _customerService.GetCustomerRoleById(price.CustomerRoleId.Value).Name
                        : _localizationService.GetResource("Admin.Catalog.Products.TierPrices.Fields.CustomerRole.All");

                    return tierPriceModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare tier price model
        /// </summary>
        /// <param name="model">Tier price model</param>
        /// <param name="product">Product</param>
        /// <param name="tierPrice">Tier price</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Tier price model</returns>
        public virtual TierPriceModel PrepareTierPriceModel(TierPriceModel model,
            Category category, TierPrice tierPrice, bool excludeProperties = false)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (tierPrice != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = tierPrice.ToModel<TierPriceModel>();
                }
            }

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(model.AvailableStores);

            //prepare available customer roles
            _baseAdminModelFactory.PrepareCustomerRoles(model.AvailableCustomerRoles);

            return model;
        }

        #endregion




    }
}