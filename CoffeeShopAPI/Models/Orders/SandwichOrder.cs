using CoffeeShopAPI.Models.Ingredients;
using CoffeeShopAPI.Models.Products;
using CoffeeShopAPI.Models.Sizes;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopAPI.Models.Orders
{
    public class SandwichOrder
    {
        // Database columns.
        [Key]
        public int SandwichOrderId { get; set; }
        [Required(ErrorMessage = "Field SandwichId cannot be empty")]
        public int SandwichId { get; set; }
        [Required(ErrorMessage = "Field SandwichSizeId cannot be empty")]
        public int SandwichSizeId { get; set; }
        public int SauceId { get; set; }
        public int SupplementsId { get; set; }
        [Required(ErrorMessage = "Field Quantity cannot be empty")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Field Price cannot be empty")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Field OrderId cannot be empty")]
        public int OrderId { get; set; }

        // Objects for relationships
        public virtual Sandwich Sandwich { get; set; }
        public virtual SandwichSize SandwichSize { get; set; }
        public virtual Sauce Sauce { get; set; }
        public virtual Supplements Supplements { get; set; }
        public virtual Order Order { get; set; }
    }
}
