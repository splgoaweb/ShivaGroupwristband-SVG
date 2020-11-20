using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.NopFeatures.ShipRocket.Infrastructure
{
    public partial class ShiprocketOrdersViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.themename";


        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        { // Main testimonial view on front-end side
            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                viewLocations = new[] {
                    $"~/Plugins/NopFeatures.ShipRocket/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                    $"~/Plugins/NopFeatures.ShipRocket/Themes/{theme}/Views/Shared/{{0}}.cshtml",

                    $"~/Plugins/NopFeatures.ShipRocket/Views/{{1}}/{{0}}.cshtml",
                    $"~/Plugins/NopFeatures.ShipRocket/Views/Shared/{{0}}.cshtml"
                }.Concat(viewLocations);
            }

            // check for area (admin)
            if (context.AreaName != null)
            {
                if (context.AreaName.Equals(AreaNames.Admin))
                {
                    viewLocations = new[] {
                        $"/Plugins/NopFeatures.ShipRocket/Areas/{AreaNames.Admin}/Views/{{1}}/{{0}}.cshtml",
                        $"/Plugins/NopFeatures.ShipRocket/Areas/{AreaNames.Admin}/Views/Shared/{{0}}.cshtml",

                    }.Concat(viewLocations);

                }
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // check current area is admin
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                return;

            // theme context
            var themeContext = (IThemeContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(IThemeContext));

            // set current theme name in context
            context.Values[THEME_KEY] = themeContext.WorkingThemeName;

        }
    }

}
