using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Plugin.NopFeatures.ShipRocket.Domain;
using Nop.Plugin.NopFeatures.ShipRocket.Models;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.NopFeatures.ShipRocket.Service
{
    /// <summary>
    /// Vendor service
    /// </summary>
    public partial class ShipRocketService : IShipRocketService
    {
        #region Fields

        private readonly IRepository<NopShiprocketOrder> _NopShiprocketOrderRepository;
        // Custom Code
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly IAddressService _addressService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceFormatter _priceFormatter;

        #endregion

        #region Ctor


        public ShipRocketService(IRepository<NopShiprocketOrder> NopShiprocketOrderRepository,
            RewardPointsSettings rewardPointsSettings,
            IAddressService addressService,
            IWorkContext workContext,
            IDateTimeHelper dateTimeHelper,
            IProductService productService,
            IOrderService orderService,
            IStoreService storeService,
            ILocalizationService localizationService,
            IPriceFormatter priceFormatter)
        {
            this._NopShiprocketOrderRepository = NopShiprocketOrderRepository;
            // Custom Code
            this._rewardPointsSettings = rewardPointsSettings;
            this._addressService = addressService;
            this._workContext = workContext;
            this._dateTimeHelper = dateTimeHelper;
            this._productService = productService;
            this._orderService = orderService;
            this._storeService = storeService;
            this._localizationService = localizationService;
            this._priceFormatter = priceFormatter;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Getting Shiprocket order by Id
        /// </summary>
        /// <param name="ShiprocketOrderId"></param>
        /// <returns></returns>
        public virtual NopShiprocketOrder GetShiprocketOrderById(int ShiprocketOrderId)
        {
            if (ShiprocketOrderId == 0)
                return null;

            return _NopShiprocketOrderRepository.GetById(ShiprocketOrderId);
        }

        /// <summary>
        /// Delete a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void DeleteShiprocketOrder(NopShiprocketOrder ShiprocketOrder)
        {
            if (ShiprocketOrder == null)
                throw new ArgumentNullException(nameof(ShiprocketOrder));

            _NopShiprocketOrderRepository.Delete(ShiprocketOrder);
        }

        /// <summary>
        /// Inserts a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void InsertShiprocketOrder(NopShiprocketOrder ShiprocketOrder)
        {
            if (ShiprocketOrder == null)
                throw new ArgumentNullException(nameof(ShiprocketOrder));

            _NopShiprocketOrderRepository.Insert(ShiprocketOrder);


        }

        /// <summary>
        /// Updates the vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void UpdateShiprocketOrder(NopShiprocketOrder ShiprocketOrder)
        {
            if (ShiprocketOrder == null)
                throw new ArgumentNullException(nameof(ShiprocketOrder));

            _NopShiprocketOrderRepository.Update(ShiprocketOrder);


        }

        /// <summary>
        ///  Getting Shiprocket order by order Id
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public virtual NopShiprocketOrder GetShiprocketOrderByOrderId(int OrderId)
        {
            if (OrderId == 0)
                return null;

            var query = (from a in _NopShiprocketOrderRepository.Table
                         where a.OrderId == OrderId
                         select a).FirstOrDefault();

            return query;
        }

        /// <summary>
        ///  Getting Shiprocket order error by order Id
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public virtual NopShiprocketOrder GetShiprocketErrorOrderByOrderId(int OrderId)
        {
            if (OrderId == 0)
                return null;

            var query = (from a in _NopShiprocketOrderRepository.Table
                         where a.OrderId == OrderId && a.ShiprocketStatues == false
                         select a).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// get all shiprocket order
        /// </summary>
        /// <returns></returns>
        public virtual IList<NopShiprocketOrder> GetAllShiprocketOrder()
        {

            var query = (from a in _NopShiprocketOrderRepository.Table
                         where a.ShiprocketStatues == false 
                         //&& string.IsNullOrEmpty(a.ErrorResponse)
                         select a).ToList();

            return query;
        }

        /// <summary>
        /// get all successed shiprocket order
        /// </summary>
        /// <returns></returns>
        public virtual NopShiprocketOrder GetAllSuccessShiprocketOrder(int OrderId)
        {

            var query = (from a in _NopShiprocketOrderRepository.Table
                         where a.ShiprocketStatues == true && Convert.ToInt32(a.ShiprocketOrderId) > 0 && a.OrderId == OrderId
                         select a).FirstOrDefault();

            return query;
        }

        #region Get Earn Reward Points
        /// <summary>
        /// Calculate how much reward points will be earned/reduced based on certain amount spent
        /// </summary>
        /// <param name="amount">Amount (in primary store currency)</param>
        /// <returns>Number of reward points</returns>
        public virtual int CalculateRewardPoints(decimal amount)
        {
            if (!_rewardPointsSettings.Enabled)
                return 0;

            if (_rewardPointsSettings.PointsForPurchases_Amount <= decimal.Zero)
                return 0;

            var points = (int)Math.Truncate(amount / _rewardPointsSettings.PointsForPurchases_Amount * _rewardPointsSettings.PointsForPurchases_Points);
            return points;
        }

        #endregion

        #region Get Order List
        /// <summary>
        /// Prepare paged order list model
        /// </summary>
        /// <param name="searchModel">Order search model</param>
        /// <returns>Order list model</returns>
        public virtual ShiprocketOrderListModel PrepareOrderListModel(OrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter orders
            var orderStatusIds = (searchModel.OrderStatusIds?.Contains(0) ?? true) ? null : searchModel.OrderStatusIds.ToList();
            var paymentStatusIds = (searchModel.PaymentStatusIds?.Contains(0) ?? true) ? null : searchModel.PaymentStatusIds.ToList();
            var shippingStatusIds = (searchModel.ShippingStatusIds?.Contains(0) ?? true) ? null : searchModel.ShippingStatusIds.ToList();
            if (_workContext.CurrentVendor != null)
                searchModel.VendorId = _workContext.CurrentVendor.Id;
            var startDateValue = !searchModel.StartDate.HasValue ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(searchModel.StartDate.Value, _dateTimeHelper.CurrentTimeZone);
            var endDateValue = !searchModel.EndDate.HasValue ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(searchModel.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);
            var product = _productService.GetProductById(searchModel.ProductId);
            var filterByProductId = product != null && (_workContext.CurrentVendor == null || product.VendorId == _workContext.CurrentVendor.Id)
                ? searchModel.ProductId : 0;

            //get orders
            var orders = _orderService.SearchOrders(storeId: searchModel.StoreId,
                vendorId: searchModel.VendorId,
                productId: filterByProductId,
                warehouseId: searchModel.WarehouseId,
                paymentMethodSystemName: searchModel.PaymentMethodSystemName,
                createdFromUtc: startDateValue,
                createdToUtc: endDateValue,
                osIds: orderStatusIds,
                psIds: paymentStatusIds,
                ssIds: shippingStatusIds,
                billingPhone: searchModel.BillingPhone,
                billingEmail: searchModel.BillingEmail,
                billingLastName: searchModel.BillingLastName,
                billingCountryId: searchModel.BillingCountryId,
                orderNotes: searchModel.OrderNotes,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new ShiprocketOrderListModel().PrepareToGrid(searchModel, orders, () =>
            {
                //fill in model values from the entity
                return orders.Select(order =>
                {
                    var billingAddress = _addressService.GetAddressById(order.BillingAddressId);
                    var shiprocketOrder = GetAllSuccessShiprocketOrder(order.Id);
                    bool HasShiprocketOder = false;
                    int ShiprocketOrderId = 0;
                    if (shiprocketOrder != null)
                    {
                        HasShiprocketOder = Convert.ToInt32(shiprocketOrder.ShiprocketOrderId) > 0 ? true : false;
                        ShiprocketOrderId = Convert.ToInt32(shiprocketOrder.ShiprocketOrderId);
                    }

                    //fill in model values from the entity
                    var orderModel = new ShiprocketOrderModel
                    {
                        Id = order.Id,
                        OrderStatusId = order.OrderStatusId,
                        PaymentStatusId = order.PaymentStatusId,
                        ShippingStatusId = order.ShippingStatusId,
                        CustomerEmail = billingAddress.Email,
                        CustomerFullName = $"{billingAddress.FirstName} {billingAddress.LastName}",
                        CustomerId = order.CustomerId,
                        CustomOrderNumber = order.CustomOrderNumber,
                        HasShiprocketOrder = HasShiprocketOder,
                        ShiprocketOrderId = ShiprocketOrderId
                    };

                    //convert dates to the user time
                    orderModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    orderModel.StoreName = _storeService.GetStoreById(order.StoreId)?.Name ?? "Deleted";
                    orderModel.OrderStatus = _localizationService.GetLocalizedEnum(order.OrderStatus);
                    orderModel.PaymentStatus = _localizationService.GetLocalizedEnum(order.PaymentStatus);
                    orderModel.ShippingStatus = _localizationService.GetLocalizedEnum(order.ShippingStatus);
                    orderModel.OrderTotal = _priceFormatter.FormatPrice(order.OrderTotal, true, false);

                    return orderModel;
                });
            });

            return model;
        }
        #endregion

        #endregion
    }
}