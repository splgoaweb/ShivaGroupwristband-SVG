using Microsoft.AspNetCore.Mvc;
using Nop.Services.SVG;
namespace Nop.Web.Controllers
{
    public class SVGController : BasePublicController
    {
        #region Fields

        private readonly ISVGContentService _svgcontentService;

        #endregion

        #region Ctor

        public SVGController(ISVGContentService svgcontentService)
        {
            _svgcontentService = svgcontentService;
        }

        #endregion
        public IActionResult Index()
        {
            return View();
        }

        #region Methods


        public ActionResult GetSVG(int productId)
        {
            var svgcontent = _svgcontentService.GetContentByProductId(productId);
            if (svgcontent == null)
                return Json(new { error = "No poll answer found with the specified id" });

            ViewBag.svgcontent = svgcontent.SvgContent;
            return View();
        }
        public ActionResult GetSVGpage(int productId)
        {
            var svgcontent = _svgcontentService.GetContentByProductId(productId);
            if (svgcontent == null)
                return Json(new { error = "No poll answer found with the specified id" });

            ViewBag.svgcontent = svgcontent.SvgContent;
            return View();
        }

        #endregion Methods
    }
}
