namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order_Line
    {
        public int Id { get; set; }

        public int Product_Id { get; set; }

        public int Order_Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int Amount { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
