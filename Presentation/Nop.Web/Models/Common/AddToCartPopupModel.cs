
using System.Collections.Generic;

namespace Nop.Web.Models.Common
{
    public class AddToCartPopupModel
    {
        public int productid { get; set; }
        public string picture { get; set; }
        public string name { get; set; }
        public string unitPrice { get; set; }
        public int qty { get; set; }
        public string toalPrice { get; set; }
        public bool isErrorNotification { get; set; }
        public List<string> addToCartWarnings { get; set; }

        // custom code start
        public int shoppingCartTypeId { get; set; }

        // custom code end
    }
}
