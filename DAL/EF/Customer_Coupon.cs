namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_Coupon
    {
        public int Id { get; set; }

        public int? Customer_Id { get; set; }

        public int? Coupon_Id { get; set; }

        public int? Order_Id { get; set; }

        public bool? Used { get; set; }

        public virtual Coupon Coupon { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Order Order { get; set; }
    }
}
