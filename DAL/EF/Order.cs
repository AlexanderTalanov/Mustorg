namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Customer_Coupon = new HashSet<Customer_Coupon>();
            Order_Line = new HashSet<Order_Line>();
        }

        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Column(TypeName = "money")]
        public decimal? Sum { get; set; }

        public int? Order_Status_Id { get; set; }

        public int? Pick_Point_Id { get; set; }

        public int? Customer_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal? Sale { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer_Coupon> Customer_Coupon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Line> Order_Line { get; set; }

        public virtual Order_Status Order_Status { get; set; }

        public virtual Pick_Point Pick_Point { get; set; }
    }
}
