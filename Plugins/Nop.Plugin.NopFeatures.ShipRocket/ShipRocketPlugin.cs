using Nop.Core;
using Nop.Core.Domain.Tasks;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Tasks;
using Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;

namespace Nop.Plugin.NopFeatures.ShipRocket
{
    public class ShipRocketPlugin : BasePlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public ShipRocketPlugin(ILocalizationService localizationService,
            IWebHelper webHelper,
            ISettingService settingService,
            IScheduleTaskService scheduleTaskService)
        {
            _localizationService = localizationService;
            _webHelper = webHelper;
            _settingService = settingService;
            _scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { PublicWidgetZones.HeadHtmlTag };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/ShipRocket/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsShipRocket";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new ShipRocketSetting
            {
                Enable = false,
                UserEmail = string.Empty,
                Password=string.Empty,
                BaseURL=string.Empty,
                MinAmountForEWayBill=0,
                ChannelId=0,
                PickUpLocation=string.Empty
            };
            _settingService.SaveSetting(settings);

            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugin.NopFeatures.ShipRocket.Enable"] = "Enable",
                ["Plugin.NopFeatures.ShipRocket.UserEmail"] = "User Email",
                ["Plugin.NopFeatures.ShipRocket.UserEmail.Hint"] = "Enter ship rocket user email.",
                ["Plugin.NopFeatures.ShipRocket.Password"] = "Password",
                ["Plugin.NopFeatures.ShipRocket.Password.Hint"] = "Enter ship rocket password",
                ["Plugin.NopFeatures.ShipRocket.BaseURL"] = "BaseURL",
                ["Plugin.NopFeatures.ShipRocket.BaseURL.Hint"] = "Enter ship rocket base url.",
                ["Plugin.NopFeatures.ShipRocket.MinAmountForEWayBill"] = "Min Amount For E-Way Bill",
                ["Plugin.NopFeatures.ShipRocket.ChannelId"] = "ChannelId",
                ["Plugin.NopFeatures.ShipRocket.ChannelId.Hint"] = "Enter ship rocket channel Id.",
                ["Plugin.NopFeatures.ShipRocket.PickUpLocation"] = "PickUp Location",
                ["Plugin.NopFeatures.ShipRocket.PickUpLocation.Hint"] = "Enter ship rocket pickup address name.",
                ["Plugin.NopFeatures.ShipRocket.ShiprocketOrderListColume"] ="Shiprocket Status & Id"
            });

            //Insert Task
            ScheduleTask task = new ScheduleTask()
            {
                Name="ShipRocket",
                Type= "Nop.Plugin.NopFeatures.ShipRocket.ScheduleTasks.ShipRocketOrderTask",
                Seconds= 86400,
                Enabled=true,
                StopOnError=false,
                LastStartUtc=null,
                LastEndUtc=null,
                LastSuccessUtc=null
            };

            _scheduleTaskService.InsertTask(task);

            base.Install();
        }


        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<ShipRocketSetting>();

            //locales
            _localizationService.DeletePluginLocaleResources("Plugin.NopFeatures.ShipRocket");

            //remove Task
            var task = _scheduleTaskService.GetTaskByType("Nop.Plugin.NopFeatures.ShipRocket.ScheduleTasks.ShipRocketOrderTask");
            if (task != null)
                _scheduleTaskService.DeleteTask(task);            

            base.Uninstall();
        }

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;
        #endregion

    }
}
