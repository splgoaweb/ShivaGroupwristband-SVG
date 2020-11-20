using Nop.Core.Domain.Orders;
using Nop.Plugin.NopFeatures.ShipRocket.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.NopFeatures.ShipRocket.Service
{
    /// <summary>
    /// Message service interface
    /// </summary>
    public partial interface IShipRocketMessageService
    {
        int SendOrderShiprocketErrorStoreOwnerNotification(Order order, int languageId,NopShiprocketOrder nopShiprocketOrder);
    }
}
