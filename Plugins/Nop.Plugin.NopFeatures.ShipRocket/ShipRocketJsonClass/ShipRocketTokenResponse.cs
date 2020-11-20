using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.NopFeatures.ShipRocket.ShipRocketJsonClass
{
    public class ShipRocketTokenResponse
    {       
            public int id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public int company_id { get; set; }
            public string created_at { get; set; }
            public string token { get; set; }
      
    }
}
