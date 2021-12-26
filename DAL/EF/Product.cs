namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Line = new HashSet<Order_Line>();
            Shopping_Cart = new HashSet<Shopping_Cart>();
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal? Sale { get; set; }

        public string Description { get; set; }

        public bool Availability { get; set; }

        public int Id { get; set; }

        [StringLength(50)]
        public string Guarantee { get; set; }

        public int Article { get; set; }

        public int Brand_Id { get; set; }

        public int Category_Id { get; set; }

        public string Photo { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Line> Order_Line { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shopping_Cart> Shopping_Cart { get; set; }
    }
}
