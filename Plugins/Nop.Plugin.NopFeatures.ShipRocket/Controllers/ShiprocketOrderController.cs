using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.NopFeatures.ShipRocket.Service;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Models.Orders;

namespace Nop.Plugin.NopFeatures.ShipRocket.Controllers
{
    public partial class ShiprocketOrderController : BaseAdminController
    {
        #region Fields
                
        private readonly IPermissionService _permissionService;
        private readonly IShipRocketService _shipRocketService;

        #endregion

        #region Ctor

        public ShiprocketOrderController(
            IPermissionService permissionService,
            IShipRocketService shipRocketService)
        {            
            _permissionService = permissionService;
            _shipRocketService = shipRocketService;
        }

        #endregion

        #region Method
        [HttpPost]
        public virtual IActionResult OrderList(OrderSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedDataTablesJson();

            //prepare model
            var model = _shipRocketService.PrepareOrderListModel(searchModel);

            return Json(model);
        }
        #endregion

    }
}
