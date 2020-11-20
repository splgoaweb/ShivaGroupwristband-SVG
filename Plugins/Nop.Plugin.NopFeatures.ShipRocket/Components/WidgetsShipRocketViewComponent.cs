using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.NopFeatures.ShipRocket.Components
{
    [ViewComponent(Name = "WidgetsShipRocket")]
    public class WidgetsShipRocketViewComponent : NopViewComponent
    {
        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            return Content("");
        }

        #endregion
    }
}
