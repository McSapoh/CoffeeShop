using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models
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
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
