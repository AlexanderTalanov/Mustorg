namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Coupon")]
    public partial class Coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coupon()
        {
            Customer_Coupon = new HashSet<Customer_Coupon>();
        }

        public int Id { get; set; }

        [StringLength(1000)]
        public string Name { get; set; }

        public decimal? Offer { get; set; }

        public decimal? Condition { get; set; }

        [StringLength(10)]
        public string Background { get; set; }

        public decimal GiveawayCondition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer_Coupon> Customer_Coupon { get; set; }
    }
}
