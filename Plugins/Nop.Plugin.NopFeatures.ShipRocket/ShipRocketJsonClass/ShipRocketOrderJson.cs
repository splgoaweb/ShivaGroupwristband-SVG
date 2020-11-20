using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass
{
    public partial class ShipRocketOrderJson : BaseNopModel
    {
        public ShipRocketOrderJson()
        {
            order_items = new List<ShipRocketOrderItem>();
        }

        public string order_id { get; set; }
        public string order_date { get; set; }
        public string pickup_location { get; set; }
        public string channel_id { get; set; }
        public string billing_customer_name { get; set; }
        public string billing_last_name { get; set; }
        public string billing_address { get; set; }
        public string billing_address_2 { get; set; }
        public string billing_city { get; set; }
        public string billing_pincode { get; set; }
        public string billing_state { get; set; }
        public string billing_country { get; set; }
        public string billing_email { get; set; }
        public string billing_phone { get; set; }
        public bool shipping_is_billing { get; set; }
        public string shipping_customer_name { get; set; }
        public string shipping_last_name { get; set; }
        public string shipping_address { get; set; }
        public string shipping_address_2 { get; set; }
        public string shipping_city { get; set; }
        public string shipping_pincode { get; set; }
        public string shipping_country { get; set; }
        public string shipping_state { get; set; }
        public string shipping_email { get; set; }
        public string shipping_phone { get; set; }
        public IList<ShipRocketOrderItem> order_items { get; set; }
        public string payment_method { get; set; }
        public int shipping_charges { get; set; }
        public int giftwrap_charges { get; set; }
        public int transaction_charges { get; set; }
        public int total_discount { get; set; }
        public int sub_total { get; set; }
        public decimal length { get; set; }
        public decimal breadth { get; set; }
        public decimal height { get; set; }
        public double weight { get; set; }
        public string ewaybill_no { get; set; }



    }


    public class ShipRocketOrderItem
    {
        public string name { get; set; }
        public string sku { get; set; }
        public int units { get; set; }
        public string selling_price { get; set; }
        public string discount { get; set; }
        public string tax { get; set; }
        public int hsn { get; set; }
    }
}