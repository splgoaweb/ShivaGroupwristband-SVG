using Nop.Core.Domain.Catalog;
using System.Collections.Generic;

namespace Nop.Services.CustomCode.Tierprice
{
    public class FinalTierPrice
    {
        public FinalTierPrice()
        {
            CategoryTierPrices = new List<TierPrice>();
        }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public int TotalQuantity { get; set; }
        public bool EnableAggregation { get; set; }
        public IList<TierPrice> CategoryTierPrices { get; set; }
    }
}
