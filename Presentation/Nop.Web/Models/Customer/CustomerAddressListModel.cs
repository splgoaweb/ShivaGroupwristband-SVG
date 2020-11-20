using Nop.Web.Framework.Models;
using Nop.Web.Models.Common;
using System.Collections.Generic;

namespace Nop.Web.Models.Customer
{
    public partial class CustomerAddressListModel : BaseNopModel
    {
        public CustomerAddressListModel()
        {
            Addresses = new List<AddressModel>();
        }

        public IList<AddressModel> Addresses { get; set; }
    }
}