using Nop.Core.Domain.Logging;
using Nop.Services.Tasks;
using System;
using Nop.Services.Logging;
using Nop.Core.Domain.Orders;
using System.Linq;
using Nop.Services.Orders;
using System.Collections.Generic;
using Nop.Services.Configuration;
using Nop.Core;
using Newtonsoft.Json;
using Nop.Services.Shipping;
using Nop.Services.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Plugin.NopFeatures.ShipRocket.Service;
using Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass;
using Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass.noeway;
using Nop.Data;
using Nop.Plugin.NopFeatures.ShipRocket.Domain;
using Nop.Services.Catalog;

namespace Nop.Plugin.NopFeatures.ShipRocket.ScheduleTasks
{
    public partial class ShipRocketOrderTask : IScheduleTask
    {
        #region #region Fields

        private readonly ILogger _LoggerService;
        private readonly IOrderService _OrderService;
        private readonly IShipRocketService _ShipRocketService;
        private readonly ShipRocketSetting _ShipRocketSeting;
        private readonly IShipmentService _shipmentService = EngineContext.Current.Resolve<IShipmentService>();
        private readonly IOrderProcessingService _orderProcessingService = EngineContext.Current.Resolve<IOrderProcessingService>();
        private readonly IOrderService _orderService = EngineContext.Current.Resolve<IOrderService>();
        private readonly ICustomerActivityService _customerActivityService = EngineContext.Current.Resolve<ICustomerActivityService>();
        private readonly ILocalizationService _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
        private readonly IWorkContext workContext = EngineContext.Current.Resolve<IWorkContext>();
        private readonly ILogger _logger = EngineContext.Current.Resolve<ILogger>();
        private readonly IShipRocketMessageService _ShipRocketMessageService;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IRepository<OrderNote> _orderNoteRepository;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        #endregion

        #region Constructors

        public ShipRocketOrderTask(ILogger LoggerService,
             IOrderService OrderService,
              IShipRocketService ShipRocketService,
              ShipRocketSetting shiprocketsetting,
              IShipRocketMessageService ShipRocketMessageService,
              IAddressService addressService,
              ICountryService countryService,
              IStateProvinceService stateProvinceService,
              IRepository<OrderNote> orderNoteRepository,
              ISettingService settingService,
              IStoreContext storeContext)
        {
            this._LoggerService = LoggerService;
            this._OrderService = OrderService;
            this._ShipRocketService = ShipRocketService;
            this._ShipRocketSeting = shiprocketsetting;
            this._ShipRocketMessageService = ShipRocketMessageService;
            this._addressService = addressService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._orderNoteRepository = orderNoteRepository;
            this._settingService = settingService;
            this._storeContext = storeContext;
        }
        #endregion

        /// <summary>
        /// ScheduleTask Execute Method
        /// </summary>
        public void Execute()
        {
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ShipRocketSettings = _settingService.LoadSetting<ShipRocketSetting>(storeScope);

            if (ShipRocketSettings.Enable)
            {
                var ShiprocketUserName = _ShipRocketSeting.UserEmail;
                var ShipRocketBaseUrl = _ShipRocketSeting.BaseURL;
                var ShipRocketPassword = _ShipRocketSeting.Password;

                if (ShiprocketUserName != null && ShipRocketBaseUrl != null && ShipRocketPassword != null)
                {
                    try
                    {
                        var allOrder = _ShipRocketService.GetAllShiprocketOrder();

                        if (allOrder.Count() > 0)
                        {
                            ShipRocketApiConfiguration rocket = new ShipRocketApiConfiguration();

                            var token = rocket.GetTocket(ShipRocketBaseUrl, ShiprocketUserName, ShipRocketPassword);

                            if (token.Contains("token"))
                            {
                                ShipRocketTokenResponse Responses = JsonConvert.DeserializeObject<ShipRocketTokenResponse>(token);

                                if (Responses != null)
                                {
                                    if (!string.IsNullOrEmpty(Responses.token))
                                    {

                                        foreach (var a in allOrder)
                                        {
                                            var order = _OrderService.GetOrderById(a.OrderId);

                                            if (!order.Deleted)
                                            {
                                                if (order.PaymentMethodSystemName == "Payments.CashOnDelivery")
                                                {
                                                    ProccedShiprocket(order, Responses.token.Trim(), ShipRocketBaseUrl, a, true);
                                                }
                                                else
                                                {
                                                    ProccedShiprocket(order, Responses.token.Trim(), ShipRocketBaseUrl, a);
                                                }
                                            }

                                        }

                                    }
                                }
                            }
                            else
                            {
                                _LoggerService.InsertLog(LogLevel.Error, "There was a problem for getting token" + token);

                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        _LoggerService.InsertLog(LogLevel.Error, "Getting Exception in shiprocket scheduletask" + Ex.Message, Ex.InnerException.ToString());

                    }
                }
                else
                {
                    _LoggerService.InsertLog(LogLevel.Error, "Can not get Ship rocket credential please enter this credential in all setting", "ShipRocketSeting.UserName, ShipRocketSeting.UserName,ShipRocketSeting.Password");
                }

                List<int> ssid = new List<int>();
                ssid.Add(30);
                var pendingOrder = _OrderService.SearchOrders(ssIds: ssid);
                ShipRocketApiConfiguration rocket1 = new ShipRocketApiConfiguration();
                var token1 = rocket1.GetTocket(ShipRocketBaseUrl, ShiprocketUserName, ShipRocketPassword);
                if (token1.Contains("token"))
                {
                    ShipRocketTokenResponse Responses = JsonConvert.DeserializeObject<ShipRocketTokenResponse>(token1);

                    foreach (var p in pendingOrder)
                    {
                        var orderShiprocketMap = _ShipRocketService.GetShiprocketOrderByOrderId(p.Id);
                        if (orderShiprocketMap != null)
                        {
                            var orderStatus = rocket1.GetShiprocketOrderStatus(Responses.token.Trim(), ShipRocketBaseUrl, Convert.ToInt32(orderShiprocketMap.ShiprocketOrderId));

                            if (!string.IsNullOrEmpty(orderStatus))
                            {
                                if (orderStatus == "DELIVERED")
                                {
                                    var Shipments = _shipmentService.GetShipmentsByOrderId(p.Id);
                                    foreach (var shipment in Shipments)
                                    {
                                        try
                                        {
                                            if (!shipment.DeliveryDateUtc.HasValue)
                                            {
                                                _orderProcessingService.Deliver(Shipments.FirstOrDefault(), true);

                                                _customerActivityService.InsertActivity("EditOrder",
                                                    string.Format(_localizationService.GetResource("ActivityLog.EditOrder"), p.CustomOrderNumber), p);
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            var customer = workContext.CurrentCustomer;
                                            _logger.Error(exc.Message, exc, customer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method procced order to shiprocket
        /// </summary>
        /// <param name="order"></param>
        /// <param name="ShipRocketToken"></param>
        /// <param name="ShipRocketBaseUrl"></param>
        /// <param name="nopshiprocket"></param>
        /// <param name="Iscod"></param>
        public void ProccedShiprocket(Order order, string ShipRocketToken, string ShipRocketBaseUrl, NopShiprocketOrder nopshiprocket, bool Iscod = false)
        {
            try
            {
                var storeScope = _storeContext.ActiveStoreScopeConfiguration;
                var ShipRocketSettings = _settingService.LoadSetting<ShipRocketSetting>(storeScope);

                ShipRocketApiConfiguration rocket = new ShipRocketApiConfiguration();
                var dimention = GetProductDimentions(order);

                var Shippingcharge = Convert.ToInt32(order.OrderShippingInclTax);

                var discount = (order.OrderDiscount > 0)? Convert.ToInt32(order.OrderDiscount) : Convert.ToInt32(order.OrderSubTotalDiscountInclTax);

                var OrderBillingAddress = _addressService.GetAddressById(order.BillingAddressId);
                var billingCountry = _countryService.GetCountryById((OrderBillingAddress.CountryId.HasValue) ? (int)OrderBillingAddress.CountryId : 0);
                var BillingStateProvince = _stateProvinceService.GetStateProvinceById((OrderBillingAddress.StateProvinceId.HasValue) ? (int)OrderBillingAddress.StateProvinceId : 0);

                var OrderShippingAddress = _addressService.GetAddressById((order.ShippingAddressId.HasValue) ? (int)order.ShippingAddressId : 0);
                var shippingCountry = _countryService.GetCountryById((OrderBillingAddress.CountryId.HasValue) ? (int)OrderShippingAddress.CountryId : 0);
                var ShipingStateProvince = _stateProvinceService.GetStateProvinceById((OrderShippingAddress.StateProvinceId.HasValue) ? (int)OrderShippingAddress.StateProvinceId : 0);

                var ShiprockerOrder = new ShipRocketOrderJsonNoEway
                {
                    order_id = "Nop Order " + order.Id,
                    order_date = Convert.ToString(order.CreatedOnUtc),
                    pickup_location = ShipRocketSettings.PickUpLocation,
                    channel_id = Convert.ToString(ShipRocketSettings.ChannelId),
                    billing_customer_name = OrderBillingAddress == null ? "" : OrderBillingAddress.FirstName,
                    billing_last_name = OrderBillingAddress == null ? "" : OrderBillingAddress.LastName,
                    billing_address = OrderBillingAddress == null ? "" : OrderBillingAddress.Address1,
                    billing_address_2 = OrderBillingAddress == null ? "" : OrderBillingAddress.Address2,
                    billing_city = OrderBillingAddress == null ? "" : OrderBillingAddress.City,
                    billing_pincode = OrderBillingAddress == null ? "" : OrderBillingAddress.ZipPostalCode,
                    billing_state = OrderBillingAddress == null ? "" : BillingStateProvince == null ? "India" : BillingStateProvince.Name,
                    billing_country = OrderBillingAddress == null ? "" : billingCountry.Name,
                    billing_email = OrderBillingAddress == null ? "" : OrderBillingAddress.Email,
                    billing_phone = OrderBillingAddress == null ? "" : OrderBillingAddress.PhoneNumber,
                    shipping_is_billing = true,
                    shipping_customer_name = OrderShippingAddress == null ? "" : OrderShippingAddress.FirstName,
                    shipping_last_name = OrderShippingAddress == null ? "" : OrderShippingAddress.LastName,
                    shipping_address = OrderShippingAddress == null ? "" : OrderShippingAddress.Address1,
                    shipping_address_2 = OrderShippingAddress == null ? "" : OrderShippingAddress.Address2,
                    shipping_city = OrderShippingAddress == null ? "" : OrderShippingAddress.City,
                    shipping_pincode = OrderShippingAddress == null ? "" : OrderShippingAddress.ZipPostalCode,
                    shipping_country = OrderShippingAddress == null ? "" : shippingCountry.Name,
                    shipping_state = OrderShippingAddress == null ? "" : ShipingStateProvince == null ? "India" : ShipingStateProvince.Name,
                    shipping_email = OrderShippingAddress == null ? "" : OrderShippingAddress.Email,
                    shipping_phone = OrderShippingAddress == null ? "" : OrderShippingAddress.PhoneNumber,

                    order_items = GetOrderItemFromOrder(order),

                    payment_method = Iscod == true ? "COD" : "Prepaid",
                    shipping_charges = Shippingcharge,
                    giftwrap_charges = 0,
                    transaction_charges = Convert.ToInt32(order.PaymentMethodAdditionalFeeInclTax),
                    total_discount = discount,
                    sub_total = Convert.ToInt32(order.OrderSubtotalInclTax),
                    length = dimention.length,
                    breadth = dimention.breadth,
                    height = dimention.height,
                    weight = dimention.weight
                };

                var result = rocket.createShiprocketOrdernoeway(ShipRocketToken, ShipRocketBaseUrl, ShiprockerOrder);

                if (!string.IsNullOrEmpty(result))
                {
                    if (!result.Contains("message"))
                    {
                        ShipRocketOrderResponse Responses = JsonConvert.DeserializeObject<ShipRocketOrderResponse>(result);

                        if (Responses.order_id > 0 && Responses.shipment_id > 0)
                        {
                            var OrderNote = (from a in _orderNoteRepository.Table
                                             where a.OrderId.Equals(order.Id)
                                             select a).FirstOrDefault();
                            if (OrderNote != null)
                            {
                                OrderNote.CreatedOnUtc = DateTime.UtcNow;
                                OrderNote.Note = "Ship Rocket Order Id-" + Responses.order_id + "Ship Rocket shipment Id-" + Responses.shipment_id;
                                OrderNote.OrderId = order.Id;
                                _orderNoteRepository.Update(OrderNote);
                            }

                            nopshiprocket.ShiprocketOrderId = Convert.ToString(Responses.order_id);
                            nopshiprocket.ShiprocketStatues = true;
                            _ShipRocketService.UpdateShiprocketOrder(nopshiprocket);
                        }
                    }
                    else
                    {
                        _LoggerService.InsertLog(LogLevel.Error, "Error log while creating shiprocket order for order no: " + order.Id, result);
                        nopshiprocket.ErrorResponse = result;
                        _ShipRocketService.UpdateShiprocketOrder(nopshiprocket);
                        _ShipRocketMessageService.SendOrderShiprocketErrorStoreOwnerNotification(order, workContext.WorkingLanguage.Id, nopshiprocket);
                    }
                }
            }
            catch (Exception Exe)
            {
                _LoggerService.InsertLog(LogLevel.Error, "Error log while executing order paid event :-" + order.Id, Exe.Message + Exe.InnerException);
            }
        }

        /// <summary>
        /// get method for order item from order
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        public IList<ShipRocketOrderItems> GetOrderItemFromOrder(Order Order)
        {
            IList<ShipRocketOrderItems> shiprocket = new List<ShipRocketOrderItems>();
            var OrderItems = _orderService.GetOrderItems(Order.Id);
            int i = 0;
            foreach (var item in OrderItems)
            {
                var Product = _orderService.GetProductByOrderItemId(item.Id);

                ShipRocketOrderItems rocketorder = new ShipRocketOrderItems
                {
                    discount = Convert.ToString(item.DiscountAmountInclTax),
                    hsn = 12345,
                    name = Product.Name,
                    selling_price = Convert.ToString(item.PriceInclTax),
                    sku = Product.Sku+'_'+i,
                    tax = "0",
                    units = item.Quantity
                };
                shiprocket.Add(rocketorder);
                i++;
            }
            return shiprocket;
        }

        /// <summary>
        /// order dimention class
        /// </summary>
        public class OrderDimention
        {
            public decimal length { get; set; }
            public decimal breadth { get; set; }
            public decimal height { get; set; }
            public double weight { get; set; }
        }

        /// <summary>
        /// Method to get product dimentions
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public OrderDimention GetProductDimentions(Order order)
        {
            var length = 0;
            var breadth = 0;
            var height = 0;
            Double weight = 0;

            var OrderItems = _orderService.GetOrderItems(order.Id);
            foreach (var a in OrderItems)
            {
                var Product = _orderService.GetProductByOrderItemId(a.Id);
                length = length + (Convert.ToInt32(Product.Length) * a.Quantity);
                breadth = breadth + (Convert.ToInt32(Product.Width) * a.Quantity);
                height = height + (Convert.ToInt32(Product.Height) * a.Quantity);
                weight = weight + (Convert.ToInt32(Product.Weight) * a.Quantity);
            }

            if (length == 0)
                length = 1;

            if (breadth == 0)
                breadth = 1;

            if (height == 0)
                height = 1;

            if (weight == 0)
                weight = 1;

            OrderDimention dimention = new OrderDimention
            {
                breadth = breadth,
                weight = weight,
                height = height,
                length = length,
            };

            return dimention;
        }
    }
}