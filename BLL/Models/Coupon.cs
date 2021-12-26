using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;


namespace BLL.Models
{
    public class CouponModel
    {
        public CouponModel() { }

        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public int? CouponId { get; set; }

        public int? OrderId { get; set; }

        public bool Used { get; set; }

        public string CouponName { get; set; }

        public decimal? Offer { get; set; }

        public decimal? Condition { get; set; }

        public string Background { get; set; }

        public string ViewText { get; set; }
    }
}
