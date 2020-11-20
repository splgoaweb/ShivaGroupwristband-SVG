using Nop.Core.Configuration;

namespace Nop.Plugin.NopFeatures.ShipRocket
{
    /// <summary>
    /// Shiprocket setting class
    /// </summary>
    public class ShipRocketSetting : ISettings
    {
        public bool Enable { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string BaseURL { get; set; }
        public decimal MinAmountForEWayBill { get; set; }
        public int ChannelId { get; set; }
        public string PickUpLocation { get; set; }

        public string WidgetZone { get; set; }
    }
}
