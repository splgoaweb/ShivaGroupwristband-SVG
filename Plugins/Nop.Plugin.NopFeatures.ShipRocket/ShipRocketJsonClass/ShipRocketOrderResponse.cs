using Nop.Web.Framework.Models;

namespace Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass
{
    public partial class ShipRocketOrderResponse : BaseNopModel
    {
        public int order_id { get; set; }
        public int shipment_id { get; set; }
        public string status { get; set; }
        public int status_code { get; set; }
        public int onboarding_completed_now { get; set; }
    }

}



