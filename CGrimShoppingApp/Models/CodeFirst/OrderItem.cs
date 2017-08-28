namespace CGrimShoppingApp.Models.CodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }

        //private Order orderEntry;

        //public virtual Order GetOrderEntry()
        //{
        //    return orderEntry;
        //}

        //public virtual void SetOrderEntry(Order value)
        //{
        //    orderEntry = value;
        //}
    }
}
