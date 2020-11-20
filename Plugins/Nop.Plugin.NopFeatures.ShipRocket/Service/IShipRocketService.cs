using Nop.Plugin.NopFeatures.ShipRocket.Domain;
using Nop.Plugin.NopFeatures.ShipRocket.Models;
using Nop.Web.Areas.Admin.Models.Orders;
using System.Collections.Generic;

namespace Nop.Plugin.NopFeatures.ShipRocket.Service
{
    /// <summary>
    /// Shiprocket service interface
    /// </summary>
    public interface IShipRocketService
    {

        void InsertShiprocketOrder(NopShiprocketOrder ShiprocketOrder);

        void UpdateShiprocketOrder(NopShiprocketOrder ShiprocketOrder);

        NopShiprocketOrder GetShiprocketOrderById(int ShiprocketOrderId);

        NopShiprocketOrder GetShiprocketOrderByOrderId(int OrderId);


        void DeleteShiprocketOrder(NopShiprocketOrder ShiprocketOrder);

        IList<NopShiprocketOrder> GetAllShiprocketOrder();

        NopShiprocketOrder GetAllSuccessShiprocketOrder(int OrderId);

        NopShiprocketOrder GetShiprocketErrorOrderByOrderId(int OrderId);

        
        /// <summary>
        /// Calculate how much reward points will be earned/reduced based on Product amount
        /// </summary>
        /// <param name="amount">Amount (in primary store currency)</param>
        /// <returns>Number of reward points</returns>
        int CalculateRewardPoints(decimal amount);

        /// <summary>
        /// Prepare paged order list model
        /// </summary>
        /// <param name="searchModel">Order search model</param>
        /// <returns>Order list model</returns>
        ShiprocketOrderListModel PrepareOrderListModel(OrderSearchModel searchModel);
    }
}


