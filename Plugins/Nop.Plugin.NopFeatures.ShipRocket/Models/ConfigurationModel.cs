using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.NopFeatures.ShipRocket.Models
{
    /// <summary>
    /// Configuration Model
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.Enable")]
        public bool Enable { get; set; }
        public bool Enable_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.UserEmail")]
        public string UserEmail { get; set; }
        public bool UserEmail_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.Password")]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.BaseURL")]
        public string BaseURL { get; set; }
        public bool BaseURL_OverrideForStore { get; set; }
                
        public decimal MinAmountForEWayBill { get; set; }
        public bool MinAmountForEWayBill_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.ChannelId")]
        public int ChannelId { get; set; }
        public bool ChannelId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugin.NopFeatures.ShipRocket.PickUpLocation")]
        public string PickUpLocation { get; set; }
        public bool PickUpLocation_OverrideForStore { get; set; }
    }
}
