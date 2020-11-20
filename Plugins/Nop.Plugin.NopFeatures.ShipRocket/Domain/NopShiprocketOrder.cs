using Nop.Core;

namespace Nop.Plugin.NopFeatures.ShipRocket.Domain
{
    /// <summary>
    /// shiprocket order entity class
    /// </summary>
    public partial class NopShiprocketOrder : BaseEntity
    {
        public int OrderId { get; set; }

        public string ShiprocketOrderId { get; set; }

        public bool ShiprocketStatues { get; set; }

        public string ErrorResponse { get; set; }

        public string EwayBillNumber { get; set; }
    }
}