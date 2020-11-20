using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Components
{
    public class AddToCartPopupViewComponent : NopViewComponent
    {
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ICurrencyService _currencyService;

        public AddToCartPopupViewComponent(IPermissionService permissionService,
            IPictureService pictureService,
            IProductService productService,
            IWorkContext workContext,
            IPriceFormatter priceFormatter,
            ICurrencyService currencyService)
        {
            _permissionService = permissionService;
            _pictureService = pictureService;
            _productService = productService;
            _workContext = workContext;
            _priceFormatter = priceFormatter;
            _currencyService = currencyService;
        }

        public IViewComponentResult Invoke(bool isErrorNotification, List<string> addToCartWarnings,
            IFormCollection form, int productId, int shoppingCartTypeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart))
                return Content("");

            int withAttributeqty = 0;
            int withoutAttributeqty = 0;
            string price = string.Empty;
            string unitprice = string.Empty;

            foreach (var key in form.Keys)
            {
                if (key.Equals($"price-value-{productId}", StringComparison.InvariantCultureIgnoreCase))
                    price = Request.Form[key];
                if (key.Equals($"unit-price-value-{productId}", StringComparison.InvariantCultureIgnoreCase))
                    unitprice = Request.Form[key];
                if (key.Equals($"product_total_qty_{productId}", StringComparison.InvariantCultureIgnoreCase))
                    int.TryParse((string)form[key], out withAttributeqty);
                if (key.Equals($"addtocart_{productId}.EnteredQuantity", StringComparison.InvariantCultureIgnoreCase))
                    int.TryParse((string)form[key], out withoutAttributeqty);
            }

            var product = _productService.GetProductById(productId);
            var pictures = _pictureService.GetPicturesByProductId(productId).FirstOrDefault();
            var ImageUrl = pictures != null ? _pictureService.GetPictureUrl(pictures.Id, 200) : _pictureService.GetDefaultPictureUrl(200);

            if (string.IsNullOrEmpty(unitprice))
            {
                unitprice = price;
                string totalprice = price.Split()[1];
                decimal totalprice_Calc = Convert.ToDecimal(totalprice.Trim()) * (withoutAttributeqty > 0 ? withoutAttributeqty : withAttributeqty);
                var finaltotalprice_Calc = _currencyService.ConvertFromPrimaryStoreCurrency(totalprice_Calc, _workContext.WorkingCurrency);
                price = _priceFormatter.FormatPrice(finaltotalprice_Calc);
            }

            var model = new AddToCartPopupModel()
            {
                productid = productId,
                isErrorNotification = isErrorNotification,
                addToCartWarnings = addToCartWarnings,
                picture = ImageUrl,
                name = product?.Name,
                unitPrice = unitprice,
                qty = withoutAttributeqty > 0 ? withoutAttributeqty : withAttributeqty,
                toalPrice = price,
                shoppingCartTypeId = shoppingCartTypeId
            };

            return View(model);
        }
    }
}
