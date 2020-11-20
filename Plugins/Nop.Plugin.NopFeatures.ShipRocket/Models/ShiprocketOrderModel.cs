using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.NopFeatures.ShipRocket.Models
{
    public class ShiprocketOrderModel : OrderModel
    {
        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.ShiprocketOrderListColume")]
        public bool HasShiprocketOrder { get; set; }

        public int ShiprocketOrderId { get; set; }
    }
}
