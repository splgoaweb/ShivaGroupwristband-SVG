using Nop.Core.Domain.Orders;
using Nop.Services.Events;
using Nop.Plugin.NopFeatures.ShipRocket.Service;
using Nop.Plugin.NopFeatures.ShipRocket.Domain;
using Nop.Services.Configuration;
using Nop.Core;

namespace Nop.Plugin.NopFeatures.ShipRocket.Event
{
    public class OrderPaidsEvent : IConsumer<OrderPlacedEvent>, IConsumer<OrderPaidEvent>
    {
        #region Fields
        private readonly IShipRocketService _ShipRocketService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        #endregion

        #region Methods
        public OrderPaidsEvent(
            IShipRocketService ShipRocketService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            this._ShipRocketService = ShipRocketService;
            this._settingService = settingService;
            this._storeContext = storeContext;
        }
        #endregion

        /// <summary>
        /// Order Placed Event
        /// </summary>
        /// <param name="eventMessage"></param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ShipRocketSettings = _settingService.LoadSetting<ShipRocketSetting>(storeScope);

            if (ShipRocketSettings.Enable)
            {

                var order = eventMessage.Order;

                if (order.PaymentMethodSystemName == "Payments.CashOnDelivery")
                {

                    var oldorder = _ShipRocketService.GetShiprocketOrderByOrderId(order.Id);

                    if (oldorder == null)
                    {
                        NopShiprocketOrder SO = new NopShiprocketOrder()
                        {
                            OrderId = order.Id,
                            ShiprocketStatues = false
                        };
                        _ShipRocketService.InsertShiprocketOrder(SO);
                    }

                }
            }
        }

        /// <summary>
        /// Order Paid Event
        /// </summary>
        /// <param name="eventMessage"></param>
        public void HandleEvent(OrderPaidEvent eventMessage)
        {
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ShipRocketSettings = _settingService.LoadSetting<ShipRocketSetting>(storeScope);

            if (ShipRocketSettings.Enable)
            {
                var order = eventMessage.Order;

                if (order.PaymentMethodSystemName != "Payments.CashOnDelivery")
                {
                    var oldorder = _ShipRocketService.GetShiprocketOrderByOrderId(order.Id);

                    if (oldorder == null)
                    {
                        NopShiprocketOrder SO = new NopShiprocketOrder()
                        {
                            OrderId = order.Id,
                            ShiprocketStatues = false
                        };
                        _ShipRocketService.InsertShiprocketOrder(SO);
                    }
                }
            }
        }

    }

}
