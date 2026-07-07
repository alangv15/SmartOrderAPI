using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderAPI.Entities.Models
{
    public class OrderDiscountDto
    {
        public int OrderDiscountId { get; set; }
        public int OrderId { get; set; }
        public int DiscountRuleId { get; set; }
        public decimal AppliedAmount { get; set; }
        public DateTime AppliedDate { get; set; }
    }
}
