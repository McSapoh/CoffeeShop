using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Orders
{
    public class Order
    {
        // Database columns.
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Field Cost cannot be empty")]
        public int Cost { get; set; }
        [Required(ErrorMessage = "Field UserId cannot be empty")]
        public int UserId { get; set; }

        // Objects for relationships
        public virtual User User { get; set; }
        public virtual ICollection<CoffeeOrder> CoffeeOrders { get; set; }
        public virtual ICollection<DessertOrder> DessertOrders { get; set; }
        public virtual ICollection<SandwichOrder> SandwichOrders { get; set; }
        public virtual ICollection<SnackOrder> SnackOrders { get; set; }
        public virtual ICollection<TeaOrder> TeaOrders { get; set; }
    }
}
