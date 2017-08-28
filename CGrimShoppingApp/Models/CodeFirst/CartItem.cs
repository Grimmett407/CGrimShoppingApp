namespace CGrimShoppingApp.Models.CodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class CartItem
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual Item Item { get; set; }

    }
}
