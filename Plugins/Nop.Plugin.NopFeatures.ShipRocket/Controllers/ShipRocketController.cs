using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.NopFeatures.ShipRocket.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.NopFeatures.ShipRocket.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class ShipRocketController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public ShipRocketController(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Configure GET Method
        /// </summary>
        /// <returns></returns>
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ShipRocketSettings = _settingService.LoadSetting<ShipRocketSetting>(storeScope);

            var model = new ConfigurationModel
            {
                Enable = ShipRocketSettings.Enable,
                UserEmail = ShipRocketSettings.UserEmail,
                Password = ShipRocketSettings.Password,
                BaseURL = ShipRocketSettings.BaseURL,
                MinAmountForEWayBill = ShipRocketSettings.MinAmountForEWayBill,
                ChannelId = ShipRocketSettings.ChannelId,
                PickUpLocation = ShipRocketSettings.PickUpLocation,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.Enable_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.Enable, storeScope);
                model.UserEmail_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.UserEmail, storeScope);
                model.Password_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.Password, storeScope);
                model.BaseURL_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.BaseURL, storeScope);
                model.MinAmountForEWayBill_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.MinAmountForEWayBill, storeScope);
                model.ChannelId_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.ChannelId, storeScope);
                model.PickUpLocation_OverrideForStore = _settingService.SettingExists(ShipRocketSettings, x => x.PickUpLocation, storeScope);
            }

            return View("~/Plugins/NopFeatures.ShipRocket/Views/Configure.cshtml", model);

        }

        /// <summary>
        /// Configure POST Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var ShipRocketSettings = _settingService.LoadSetting<ShipRocketSetting>(storeScope);

            ShipRocketSettings.Enable = model.Enable;
            ShipRocketSettings.UserEmail = model.UserEmail;
            ShipRocketSettings.Password = model.Password;
            ShipRocketSettings.BaseURL = model.BaseURL;
            ShipRocketSettings.MinAmountForEWayBill = model.MinAmountForEWayBill;
            ShipRocketSettings.ChannelId = model.ChannelId;
            ShipRocketSettings.PickUpLocation = model.PickUpLocation;

            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.Enable, model.Enable_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.UserEmail, model.UserEmail_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.BaseURL, model.BaseURL_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.MinAmountForEWayBill, model.MinAmountForEWayBill_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.ChannelId, model.ChannelId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(ShipRocketSettings, x => x.PickUpLocation, model.PickUpLocation_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        #endregion
    }
}
